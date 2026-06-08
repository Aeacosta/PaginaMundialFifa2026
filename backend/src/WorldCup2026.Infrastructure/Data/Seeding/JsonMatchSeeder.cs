using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Infrastructure.Data.Seeding.Models;

namespace WorldCup2026.Infrastructure.Data.Seeding;

/// <summary>
/// Seeds matches from a JSON file for integration purposes
/// </summary>
public class JsonMatchSeeder : IDataSeeder
{
    private readonly WorldCupDbContext _context;
    private readonly ILogger<JsonMatchSeeder> _logger;
    private readonly string _jsonFilePath;

    public JsonMatchSeeder(
        WorldCupDbContext context, 
        ILogger<JsonMatchSeeder> logger,
        string? jsonFilePath = null)
    {
        _context = context;
        _logger = logger;
        _jsonFilePath = jsonFilePath ?? Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Data", "Seeding", "SeedData", "matches.json");
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if matches already exist
        if (await _context.Matches.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Matches already exist. Skipping JSON seeding.");
            return;
        }

        if (!File.Exists(_jsonFilePath))
        {
            _logger.LogWarning("JSON file not found at {Path}. Skipping JSON seeding.", _jsonFilePath);
            return;
        }

        try
        {
            _logger.LogInformation("Loading matches from JSON file: {Path}", _jsonFilePath);

            // Read and deserialize JSON file
            var jsonContent = await File.ReadAllTextAsync(_jsonFilePath, cancellationToken);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
            
            var matchSeedData = JsonSerializer.Deserialize<MatchSeedData>(jsonContent, options);
            
            if (matchSeedData?.Matches == null || matchSeedData.Matches.Count == 0)
            {
                _logger.LogWarning("No matches found in JSON file.");
                return;
            }

            // Load reference data
            var teams = await _context.Teams.Include(t => t.Group).ToListAsync(cancellationToken);
            var stadiums = await _context.Stadiums.ToListAsync(cancellationToken);
            var groups = await _context.Groups.ToListAsync(cancellationToken);

            var matches = new List<Match>();
            var matchesWithResults = new List<(Match match, MatchResultData resultData)>();

            foreach (var matchData in matchSeedData.Matches)
            {
                try
                {
                    var match = await CreateMatchFromDataAsync(
                        matchData,
                        teams,
                        stadiums,
                        groups,
                        cancellationToken);

                    if (match != null)
                    {
                        matches.Add(match);

                        // Store match with result data for later processing
                        if (matchData.Result != null)
                        {
                            matchesWithResults.Add((match, matchData.Result));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing match: {HomeTeam} vs {AwayTeam}",
                        matchData.HomeTeamCode, matchData.AwayTeamCode);
                }
            }

            if (matches.Count > 0)
            {
                await _context.Matches.AddRangeAsync(matches, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                // Now create match results after matches have been saved and have IDs
                if (matchesWithResults.Count > 0)
                {
                    var matchResults = new List<MatchResult>();
                    foreach (var (match, resultData) in matchesWithResults)
                    {
                        var matchResult = CreateMatchResult(match, resultData);
                        matchResults.Add(matchResult);
                    }

                    await _context.MatchResults.AddRangeAsync(matchResults, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    // Recalculate standings for groups with completed matches
                    var groupsToRecalculate = matchesWithResults
                        .Where(m => m.match.GroupId.HasValue)
                        .Select(m => m.match.GroupId!.Value)
                        .Distinct()
                        .ToList();

                    if (groupsToRecalculate.Any())
                    {
                        _logger.LogInformation("Recalculating standings for {Count} groups with completed matches", groupsToRecalculate.Count);
                        
                        foreach (var groupId in groupsToRecalculate)
                        {
                            await RecalculateGroupStandingsAsync(groupId, cancellationToken);
                        }
                        
                        await _context.SaveChangesAsync(cancellationToken);
                        _logger.LogInformation("Standings recalculated successfully");
                    }
                }

                _logger.LogInformation("Successfully seeded {Count} matches from JSON file.", matches.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding matches from JSON file.");
            throw;
        }
    }

    private async Task<Match?> CreateMatchFromDataAsync(
        MatchData matchData,
        List<Team> teams,
        List<Stadium> stadiums,
        List<Group> groups,
        CancellationToken cancellationToken)
    {
        // Find teams by code
        var homeTeam = teams.FirstOrDefault(t => 
            t.Code.Equals(matchData.HomeTeamCode, StringComparison.OrdinalIgnoreCase));
        var awayTeam = teams.FirstOrDefault(t => 
            t.Code.Equals(matchData.AwayTeamCode, StringComparison.OrdinalIgnoreCase));

        // For TBD teams, use first available team as placeholder
        if (homeTeam == null && matchData.HomeTeamCode?.StartsWith("TBD") == true)
        {
            homeTeam = teams.FirstOrDefault();
        }
        if (awayTeam == null && matchData.AwayTeamCode?.StartsWith("TBD") == true)
        {
            awayTeam = teams.Skip(1).FirstOrDefault();
        }

        if (homeTeam == null || awayTeam == null)
        {
            _logger.LogWarning("Could not find teams: {HomeTeam} vs {AwayTeam}", 
                matchData.HomeTeamCode, matchData.AwayTeamCode);
            return null;
        }

        // Find stadium by name
        var stadium = stadiums.FirstOrDefault(s => 
            s.Name.Equals(matchData.StadiumName, StringComparison.OrdinalIgnoreCase));
        
        if (stadium == null)
        {
            _logger.LogWarning("Could not find stadium: {Stadium}", matchData.StadiumName);
            stadium = stadiums.FirstOrDefault(); // Use first stadium as fallback
        }

        if (stadium == null)
        {
            _logger.LogWarning("No stadiums available for match.");
            return null;
        }

        // Find group by name (optional for knockout stages)
        Group? group = null;
        if (!string.IsNullOrEmpty(matchData.GroupName))
        {
            group = groups.FirstOrDefault(g => 
                g.Name.Equals(matchData.GroupName, StringComparison.OrdinalIgnoreCase));
        }

        // Parse enums
        if (!Enum.TryParse<MatchPhase>(matchData.Phase, true, out var phase))
        {
            phase = MatchPhase.GroupStage;
        }

        if (!Enum.TryParse<MatchStatus>(matchData.Status, true, out var status))
        {
            status = MatchStatus.Scheduled;
        }

        // Map "Completed" to "Finished" for backward compatibility
        if (matchData.Status?.Equals("Completed", StringComparison.OrdinalIgnoreCase) == true)
        {
            status = MatchStatus.Finished;
        }

        return new Match
        {
            HomeTeamId = homeTeam.Id,
            AwayTeamId = awayTeam.Id,
            StadiumId = stadium.Id,
            GroupId = group?.Id,
            Phase = phase,
            Round = matchData.Round,
            MatchDate = matchData.MatchDate,
            Status = status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private MatchResult CreateMatchResult(Match match, MatchResultData resultData)
    {
        // Determine winner
        int? winnerId = null;
        if (resultData.HomeTeamScore > resultData.AwayTeamScore)
        {
            winnerId = match.HomeTeamId;
        }
        else if (resultData.AwayTeamScore > resultData.HomeTeamScore)
        {
            winnerId = match.AwayTeamId;
        }
        // Check penalties for draws
        else if (resultData.HomeTeamPenalties.HasValue && resultData.AwayTeamPenalties.HasValue)
        {
            if (resultData.HomeTeamPenalties > resultData.AwayTeamPenalties)
            {
                winnerId = match.HomeTeamId;
            }
            else if (resultData.AwayTeamPenalties > resultData.HomeTeamPenalties)
            {
                winnerId = match.AwayTeamId;
            }
        }

        return new MatchResult
        {
            MatchId = match.Id,
            HomeTeamScore = resultData.HomeTeamScore,
            AwayTeamScore = resultData.AwayTeamScore,
            HomeTeamPenalties = resultData.HomeTeamPenalties,
            AwayTeamPenalties = resultData.AwayTeamPenalties,
            WinnerTeamId = winnerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private async Task RecalculateGroupStandingsAsync(int groupId, CancellationToken cancellationToken)
    {
        // Get all matches for the group that are finished
        var matches = await _context.Matches
            .Include(m => m.Result)
            .Where(m => m.GroupId == groupId
                && m.Status == MatchStatus.Finished
                && m.Result != null)
            .ToListAsync(cancellationToken);

        // Get all standings for the group
        var standings = await _context.Standings
            .Where(s => s.GroupId == groupId)
            .ToListAsync(cancellationToken);

        // Reset all standings
        foreach (var standing in standings)
        {
            standing.Played = 0;
            standing.Won = 0;
            standing.Drawn = 0;
            standing.Lost = 0;
            standing.GoalsFor = 0;
            standing.GoalsAgainst = 0;
            standing.GoalDifference = 0;
            standing.Points = 0;
        }

        // Calculate standings from matches
        foreach (var match in matches)
        {
            var homeStanding = standings.First(s => s.TeamId == match.HomeTeamId);
            var awayStanding = standings.First(s => s.TeamId == match.AwayTeamId);

            homeStanding.Played++;
            awayStanding.Played++;

            homeStanding.GoalsFor += match.Result!.HomeTeamScore;
            homeStanding.GoalsAgainst += match.Result.AwayTeamScore;
            awayStanding.GoalsFor += match.Result.AwayTeamScore;
            awayStanding.GoalsAgainst += match.Result.HomeTeamScore;

            if (match.Result.HomeTeamScore > match.Result.AwayTeamScore)
            {
                homeStanding.Won++;
                homeStanding.Points += 3;
                awayStanding.Lost++;
            }
            else if (match.Result.HomeTeamScore < match.Result.AwayTeamScore)
            {
                awayStanding.Won++;
                awayStanding.Points += 3;
                homeStanding.Lost++;
            }
            else
            {
                homeStanding.Drawn++;
                awayStanding.Drawn++;
                homeStanding.Points++;
                awayStanding.Points++;
            }

            homeStanding.GoalDifference = homeStanding.GoalsFor - homeStanding.GoalsAgainst;
            awayStanding.GoalDifference = awayStanding.GoalsFor - awayStanding.GoalsAgainst;
        }

        // Sort and assign positions
        var sortedStandings = standings
            .OrderByDescending(s => s.Points)
            .ThenByDescending(s => s.GoalDifference)
            .ThenByDescending(s => s.GoalsFor)
            .ToList();

        for (int i = 0; i < sortedStandings.Count; i++)
        {
            sortedStandings[i].Position = i + 1;
        }

        _context.Standings.UpdateRange(standings);
    }
}

// Made with Bob