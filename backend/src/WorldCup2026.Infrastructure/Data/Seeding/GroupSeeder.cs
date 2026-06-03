using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Infrastructure.Data.Seeding;

/// <summary>
/// Seeds group data for FIFA World Cup 2026
/// </summary>
public class GroupSeeder : IDataSeeder
{
    private readonly WorldCupDbContext _context;

    public GroupSeeder(WorldCupDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if groups already exist
        if (await _context.Groups.AnyAsync(cancellationToken))
        {
            return;
        }

        var groups = new List<Group>
        {
            new() { Name = "Group A", Description = "Group A - FIFA World Cup 2026" },
            new() { Name = "Group B", Description = "Group B - FIFA World Cup 2026" },
            new() { Name = "Group C", Description = "Group C - FIFA World Cup 2026" },
            new() { Name = "Group D", Description = "Group D - FIFA World Cup 2026" },
            new() { Name = "Group E", Description = "Group E - FIFA World Cup 2026" },
            new() { Name = "Group F", Description = "Group F - FIFA World Cup 2026" },
            new() { Name = "Group G", Description = "Group G - FIFA World Cup 2026" },
            new() { Name = "Group H", Description = "Group H - FIFA World Cup 2026" },
            new() { Name = "Group I", Description = "Group I - FIFA World Cup 2026" },
            new() { Name = "Group J", Description = "Group J - FIFA World Cup 2026" },
            new() { Name = "Group K", Description = "Group K - FIFA World Cup 2026" },
            new() { Name = "Group L", Description = "Group L - FIFA World Cup 2026" }
        };

        await _context.Groups.AddRangeAsync(groups, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

// Made with Bob
