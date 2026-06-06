using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Infrastructure.Data.Seeding;

/// <summary>
/// Seeds initial standings for all teams (all zeros at tournament start)
/// </summary>
public class StandingSeeder : IDataSeeder
{
    private readonly WorldCupDbContext _context;

    public StandingSeeder(WorldCupDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if standings already exist
        if (await _context.Standings.AnyAsync(cancellationToken))
        {
            return;
        }

        // Get all teams with their groups
        var teams = await _context.Teams
            .Where(t => t.GroupId != null)
            .ToListAsync(cancellationToken);

        if (teams.Count == 0)
        {
            throw new InvalidOperationException("Teams must exist before seeding standings");
        }

        // Create initial standing for each team (all zeros)
        var standings = teams.Select((team, index) => new Standing
        {
            TeamId = team.Id,
            GroupId = team.GroupId!.Value,
            Played = 0,
            Won = 0,
            Drawn = 0,
            Lost = 0,
            GoalsFor = 0,
            GoalsAgainst = 0,
            GoalDifference = 0,
            Points = 0,
            Position = (index % 4) + 1 // Initial position 1-4 within each group
        }).ToList();

        await _context.Standings.AddRangeAsync(standings, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

// Made with Bob