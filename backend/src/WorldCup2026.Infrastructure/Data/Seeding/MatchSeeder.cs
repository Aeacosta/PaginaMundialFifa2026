using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Infrastructure.Data.Seeding;

/// <summary>
/// Seeds initial matches for the World Cup 2026
/// </summary>
public class MatchSeeder : IDataSeeder
{
    private readonly WorldCupDbContext _context;

    public MatchSeeder(WorldCupDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if matches already exist
        if (await _context.Matches.AnyAsync(cancellationToken))
        {
            return;
        }

        // Get all groups, teams, and stadiums
        var groups = await _context.Groups.ToListAsync(cancellationToken);
        var teams = await _context.Teams.Include(t => t.Group).ToListAsync(cancellationToken);
        var stadiums = await _context.Stadiums.ToListAsync(cancellationToken);

        if (groups.Count == 0 || teams.Count == 0 || stadiums.Count == 0)
        {
            throw new InvalidOperationException("Groups, teams, and stadiums must exist before seeding matches");
        }

        var matches = new List<Match>();
        var random = new Random(42); // Fixed seed for reproducibility
        var startDate = new DateTime(2026, 6, 11, 12, 0, 0, DateTimeKind.Utc); // Tournament starts June 11, 2026

        // Generate group stage matches (each team plays 3 matches)
        foreach (var group in groups)
        {
            var groupTeams = teams.Where(t => t.GroupId == group.Id).ToList();
            if (groupTeams.Count != 4)
            {
                continue; // Skip if group doesn't have exactly 4 teams
            }

            // Round-robin: each team plays every other team once
            var matchPairs = new List<(Team home, Team away)>
            {
                (groupTeams[0], groupTeams[1]),
                (groupTeams[2], groupTeams[3]),
                (groupTeams[0], groupTeams[2]),
                (groupTeams[1], groupTeams[3]),
                (groupTeams[0], groupTeams[3]),
                (groupTeams[1], groupTeams[2])
            };

            int matchDay = 0;
            foreach (var (home, away) in matchPairs)
            {
                var stadium = stadiums[random.Next(stadiums.Count)];
                var matchDate = startDate.AddDays(matchDay * 4 + (group.Id - 1) * 0.5); // Spread matches across days
                
                matches.Add(new Match
                {
                    HomeTeamId = home.Id,
                    AwayTeamId = away.Id,
                    StadiumId = stadium.Id,
                    GroupId = group.Id,
                    Phase = MatchPhase.GroupStage,
                    Round = (matchDay / 2) + 1,
                    MatchDate = matchDate,
                    Status = matchDate < DateTime.UtcNow ? MatchStatus.Scheduled : MatchStatus.Scheduled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                matchDay++;
            }
        }

        // Add some knockout stage placeholder matches (to be determined)
        var knockoutStartDate = startDate.AddDays(20);
        
        // Round of 32 (16 matches)
        for (int i = 0; i < 16; i++)
        {
            matches.Add(new Match
            {
                HomeTeamId = teams[0].Id, // Placeholder
                AwayTeamId = teams[1].Id, // Placeholder
                StadiumId = stadiums[random.Next(stadiums.Count)].Id,
                Phase = MatchPhase.RoundOf32,
                Round = i + 1,
                MatchDate = knockoutStartDate.AddDays(i / 4),
                Status = MatchStatus.Scheduled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Round of 16 (8 matches)
        for (int i = 0; i < 8; i++)
        {
            matches.Add(new Match
            {
                HomeTeamId = teams[0].Id, // Placeholder
                AwayTeamId = teams[1].Id, // Placeholder
                StadiumId = stadiums[random.Next(stadiums.Count)].Id,
                Phase = MatchPhase.RoundOf16,
                Round = i + 1,
                MatchDate = knockoutStartDate.AddDays(24 + i / 2),
                Status = MatchStatus.Scheduled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Quarter Finals (4 matches)
        for (int i = 0; i < 4; i++)
        {
            matches.Add(new Match
            {
                HomeTeamId = teams[0].Id, // Placeholder
                AwayTeamId = teams[1].Id, // Placeholder
                StadiumId = stadiums[random.Next(stadiums.Count)].Id,
                Phase = MatchPhase.QuarterFinals,
                Round = i + 1,
                MatchDate = knockoutStartDate.AddDays(28 + i / 2),
                Status = MatchStatus.Scheduled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Semi Finals (2 matches)
        for (int i = 0; i < 2; i++)
        {
            matches.Add(new Match
            {
                HomeTeamId = teams[0].Id, // Placeholder
                AwayTeamId = teams[1].Id, // Placeholder
                StadiumId = stadiums[random.Next(stadiums.Count)].Id,
                Phase = MatchPhase.SemiFinals,
                Round = i + 1,
                MatchDate = knockoutStartDate.AddDays(32 + i),
                Status = MatchStatus.Scheduled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Third Place match
        matches.Add(new Match
        {
            HomeTeamId = teams[0].Id, // Placeholder
            AwayTeamId = teams[1].Id, // Placeholder
            StadiumId = stadiums[random.Next(stadiums.Count)].Id,
            Phase = MatchPhase.ThirdPlace,
            Round = 1,
            MatchDate = knockoutStartDate.AddDays(35),
            Status = MatchStatus.Scheduled,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        // Final
        matches.Add(new Match
        {
            HomeTeamId = teams[0].Id, // Placeholder
            AwayTeamId = teams[1].Id, // Placeholder
            StadiumId = stadiums[random.Next(stadiums.Count)].Id,
            Phase = MatchPhase.Final,
            Round = 1,
            MatchDate = knockoutStartDate.AddDays(36),
            Status = MatchStatus.Scheduled,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        await _context.Matches.AddRangeAsync(matches, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

// Made with Bob