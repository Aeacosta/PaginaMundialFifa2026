using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Interfaces;
using WorldCup2026.Infrastructure.Data;

namespace WorldCup2026.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Standing entity
/// </summary>
public class StandingRepository : Repository<Standing>, IStandingRepository
{
    public StandingRepository(WorldCupDbContext context) : base(context)
    {
    }

    public async Task<Standing?> GetByTeamIdAsync(int teamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Team)
            .Include(s => s.Group)
            .FirstOrDefaultAsync(s => s.TeamId == teamId, cancellationToken);
    }

    public async Task<IEnumerable<Standing>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Team)
            .Where(s => s.GroupId == groupId)
            .OrderBy(s => s.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Standing>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Team)
            .Include(s => s.Group)
            .OrderBy(s => s.Group.Name)
            .ThenBy(s => s.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Standing>> GetQualifiedTeamsAsync(int teamsPerGroup = 2, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Team)
            .Include(s => s.Group)
            .Where(s => s.Position <= teamsPerGroup)
            .OrderBy(s => s.Group.Name)
            .ThenBy(s => s.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<Standing?> GetWithDetailsAsync(int standingId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Team)
            .Include(s => s.Group)
            .FirstOrDefaultAsync(s => s.Id == standingId, cancellationToken);
    }

    public async Task UpdateGroupStandingsAsync(int groupId, CancellationToken cancellationToken = default)
    {
        // Get all matches for the group that are finished
        var matches = await _context.Matches
            .Include(m => m.Result)
            .Where(m => m.GroupId == groupId && m.Result != null)
            .ToListAsync(cancellationToken);

        // Get all standings for the group
        var standings = await _dbSet
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

        _dbSet.UpdateRange(standings);
    }

    public async Task RecalculateAllStandingsAsync(CancellationToken cancellationToken = default)
    {
        var groups = await _context.Groups.ToListAsync(cancellationToken);

        foreach (var group in groups)
        {
            await UpdateGroupStandingsAsync(group.Id, cancellationToken);
        }
    }
}

// Made with Bob