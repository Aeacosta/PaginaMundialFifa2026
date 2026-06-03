using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Infrastructure.Data.Seeding;

/// <summary>
/// Seeds actual qualified teams for FIFA World Cup 2026
/// Source: https://www.fifa.com/en/tournaments/mens/worldcup/canadamexicousa2026/teams
/// </summary>
public class TeamSeeder : IDataSeeder
{
    private readonly WorldCupDbContext _context;

    public TeamSeeder(WorldCupDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if teams already exist
        if (await _context.Teams.AnyAsync(cancellationToken))
        {
            return;
        }

        // Get groups for assignment
        var groups = await _context.Groups.OrderBy(g => g.Name).ToListAsync(cancellationToken);
        
        if (groups.Count != 12)
        {
            throw new InvalidOperationException("12 groups must exist before seeding teams");
        }

        var teams = new List<Team>
        {
            // Group A
            new() { Name = "Mexico", Code = "MEX", Confederation = Confederation.CONCACAF, FifaRanking = 15, GroupId = groups[0].Id },
            new() { Name = "South Africa", Code = "RSA", Confederation = Confederation.CAF, FifaRanking = 57, GroupId = groups[0].Id },
            new() { Name = "South Korea", Code = "KOR", Confederation = Confederation.AFC, FifaRanking = 23, GroupId = groups[0].Id },
            new() { Name = "Czechia", Code = "CZE", Confederation = Confederation.UEFA, FifaRanking = 33, GroupId = groups[0].Id },

            // Group B
            new() { Name = "Canada", Code = "CAN", Confederation = Confederation.CONCACAF, FifaRanking = 41, GroupId = groups[1].Id },
            new() { Name = "Bosnia and Herzegovina", Code = "BIH", Confederation = Confederation.UEFA, FifaRanking = 62, GroupId = groups[1].Id },
            new() { Name = "Qatar", Code = "QAT", Confederation = Confederation.AFC, FifaRanking = 58, GroupId = groups[1].Id },
            new() { Name = "Switzerland", Code = "SUI", Confederation = Confederation.UEFA, FifaRanking = 15, GroupId = groups[1].Id },

            // Group C
            new() { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 5, GroupId = groups[2].Id },
            new() { Name = "Morocco", Code = "MAR", Confederation = Confederation.CAF, FifaRanking = 22, GroupId = groups[2].Id },
            new() { Name = "Haiti", Code = "HAI", Confederation = Confederation.CONCACAF, FifaRanking = 85, GroupId = groups[2].Id },
            new() { Name = "Scotland", Code = "SCO", Confederation = Confederation.UEFA, FifaRanking = 39, GroupId = groups[2].Id },

            // Group D
            new() { Name = "United States", Code = "USA", Confederation = Confederation.CONCACAF, FifaRanking = 13, GroupId = groups[3].Id },
            new() { Name = "Paraguay", Code = "PAR", Confederation = Confederation.CONMEBOL, FifaRanking = 46, GroupId = groups[3].Id },
            new() { Name = "Australia", Code = "AUS", Confederation = Confederation.AFC, FifaRanking = 27, GroupId = groups[3].Id },
            new() { Name = "Türkiye", Code = "TUR", Confederation = Confederation.UEFA, FifaRanking = 29, GroupId = groups[3].Id },

            // Group E
            new() { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA, FifaRanking = 11, GroupId = groups[4].Id },
            new() { Name = "Curaçao", Code = "CUW", Confederation = Confederation.CONCACAF, FifaRanking = 82, GroupId = groups[4].Id },
            new() { Name = "Ivory Coast", Code = "CIV", Confederation = Confederation.CAF, FifaRanking = 52, GroupId = groups[4].Id },
            new() { Name = "Ecuador", Code = "ECU", Confederation = Confederation.CONMEBOL, FifaRanking = 44, GroupId = groups[4].Id },

            // Group F
            new() { Name = "Netherlands", Code = "NED", Confederation = Confederation.UEFA, FifaRanking = 6, GroupId = groups[5].Id },
            new() { Name = "Japan", Code = "JPN", Confederation = Confederation.AFC, FifaRanking = 20, GroupId = groups[5].Id },
            new() { Name = "Sweden", Code = "SWE", Confederation = Confederation.UEFA, FifaRanking = 16, GroupId = groups[5].Id },
            new() { Name = "Tunisia", Code = "TUN", Confederation = Confederation.CAF, FifaRanking = 30, GroupId = groups[5].Id },

            // Group G
            new() { Name = "Belgium", Code = "BEL", Confederation = Confederation.UEFA, FifaRanking = 3, GroupId = groups[6].Id },
            new() { Name = "Egypt", Code = "EGY", Confederation = Confederation.CAF, FifaRanking = 36, GroupId = groups[6].Id },
            new() { Name = "Iran", Code = "IRN", Confederation = Confederation.AFC, FifaRanking = 21, GroupId = groups[6].Id },
            new() { Name = "New Zealand", Code = "NZL", Confederation = Confederation.OFC, FifaRanking = 101, GroupId = groups[6].Id },

            // Group H
            new() { Name = "Spain", Code = "ESP", Confederation = Confederation.UEFA, FifaRanking = 7, GroupId = groups[7].Id },
            new() { Name = "Cape Verde", Code = "CPV", Confederation = Confederation.CAF, FifaRanking = 73, GroupId = groups[7].Id },
            new() { Name = "Saudi Arabia", Code = "KSA", Confederation = Confederation.AFC, FifaRanking = 53, GroupId = groups[7].Id },
            new() { Name = "Uruguay", Code = "URU", Confederation = Confederation.CONMEBOL, FifaRanking = 14, GroupId = groups[7].Id },

            // Group I
            new() { Name = "France", Code = "FRA", Confederation = Confederation.UEFA, FifaRanking = 2, GroupId = groups[8].Id },
            new() { Name = "Senegal", Code = "SEN", Confederation = Confederation.CAF, FifaRanking = 18, GroupId = groups[8].Id },
            new() { Name = "Iraq", Code = "IRQ", Confederation = Confederation.AFC, FifaRanking = 70, GroupId = groups[8].Id },
            new() { Name = "Norway", Code = "NOR", Confederation = Confederation.UEFA, FifaRanking = 38, GroupId = groups[8].Id },

            // Group J
            new() { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1, GroupId = groups[9].Id },
            new() { Name = "Algeria", Code = "ALG", Confederation = Confederation.CAF, FifaRanking = 37, GroupId = groups[9].Id },
            new() { Name = "Austria", Code = "AUT", Confederation = Confederation.UEFA, FifaRanking = 32, GroupId = groups[9].Id },
            new() { Name = "Jordan", Code = "JOR", Confederation = Confederation.AFC, FifaRanking = 87, GroupId = groups[9].Id },

            // Group K
            new() { Name = "Portugal", Code = "POR", Confederation = Confederation.UEFA, FifaRanking = 8, GroupId = groups[10].Id },
            new() { Name = "DR Congo", Code = "COD", Confederation = Confederation.CAF, FifaRanking = 63, GroupId = groups[10].Id },
            new() { Name = "Uzbekistan", Code = "UZB", Confederation = Confederation.AFC, FifaRanking = 68, GroupId = groups[10].Id },
            new() { Name = "Colombia", Code = "COL", Confederation = Confederation.CONMEBOL, FifaRanking = 17, GroupId = groups[10].Id },

            // Group L
            new() { Name = "England", Code = "ENG", Confederation = Confederation.UEFA, FifaRanking = 4, GroupId = groups[11].Id },
            new() { Name = "Croatia", Code = "CRO", Confederation = Confederation.UEFA, FifaRanking = 12, GroupId = groups[11].Id },
            new() { Name = "Ghana", Code = "GHA", Confederation = Confederation.CAF, FifaRanking = 61, GroupId = groups[11].Id },
            new() { Name = "Panama", Code = "PAN", Confederation = Confederation.CONCACAF, FifaRanking = 56, GroupId = groups[11].Id }
        };

        await _context.Teams.AddRangeAsync(teams, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

// Made with Bob
