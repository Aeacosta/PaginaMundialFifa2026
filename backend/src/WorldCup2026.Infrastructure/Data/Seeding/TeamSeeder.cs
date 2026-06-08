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
            // Group A - Using flagcdn.com for free flag images (256x192 resolution)
            new() { Name = "Mexico", Code = "MEX", Confederation = Confederation.CONCACAF, FifaRanking = 15, GroupId = groups[0].Id, FlagUrl = "https://flagcdn.com/w320/mx.png" },
            new() { Name = "South Africa", Code = "RSA", Confederation = Confederation.CAF, FifaRanking = 57, GroupId = groups[0].Id, FlagUrl = "https://flagcdn.com/w320/za.png" },
            new() { Name = "South Korea", Code = "KOR", Confederation = Confederation.AFC, FifaRanking = 23, GroupId = groups[0].Id, FlagUrl = "https://flagcdn.com/w320/kr.png" },
            new() { Name = "Czechia", Code = "CZE", Confederation = Confederation.UEFA, FifaRanking = 33, GroupId = groups[0].Id, FlagUrl = "https://flagcdn.com/w320/cz.png" },

            // Group B
            new() { Name = "Canada", Code = "CAN", Confederation = Confederation.CONCACAF, FifaRanking = 41, GroupId = groups[1].Id, FlagUrl = "https://flagcdn.com/w320/ca.png" },
            new() { Name = "Bosnia and Herzegovina", Code = "BIH", Confederation = Confederation.UEFA, FifaRanking = 62, GroupId = groups[1].Id, FlagUrl = "https://flagcdn.com/w320/ba.png" },
            new() { Name = "Qatar", Code = "QAT", Confederation = Confederation.AFC, FifaRanking = 58, GroupId = groups[1].Id, FlagUrl = "https://flagcdn.com/w320/qa.png" },
            new() { Name = "Switzerland", Code = "SUI", Confederation = Confederation.UEFA, FifaRanking = 15, GroupId = groups[1].Id, FlagUrl = "https://flagcdn.com/w320/ch.png" },

            // Group C
            new() { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 5, GroupId = groups[2].Id, FlagUrl = "https://flagcdn.com/w320/br.png" },
            new() { Name = "Morocco", Code = "MAR", Confederation = Confederation.CAF, FifaRanking = 22, GroupId = groups[2].Id, FlagUrl = "https://flagcdn.com/w320/ma.png" },
            new() { Name = "Haiti", Code = "HAI", Confederation = Confederation.CONCACAF, FifaRanking = 85, GroupId = groups[2].Id, FlagUrl = "https://flagcdn.com/w320/ht.png" },
            new() { Name = "Scotland", Code = "SCO", Confederation = Confederation.UEFA, FifaRanking = 39, GroupId = groups[2].Id, FlagUrl = "https://flagcdn.com/w320/gb-sct.png" },

            // Group D
            new() { Name = "United States", Code = "USA", Confederation = Confederation.CONCACAF, FifaRanking = 13, GroupId = groups[3].Id, FlagUrl = "https://flagcdn.com/w320/us.png" },
            new() { Name = "Paraguay", Code = "PAR", Confederation = Confederation.CONMEBOL, FifaRanking = 46, GroupId = groups[3].Id, FlagUrl = "https://flagcdn.com/w320/py.png" },
            new() { Name = "Australia", Code = "AUS", Confederation = Confederation.AFC, FifaRanking = 27, GroupId = groups[3].Id, FlagUrl = "https://flagcdn.com/w320/au.png" },
            new() { Name = "Türkiye", Code = "TUR", Confederation = Confederation.UEFA, FifaRanking = 29, GroupId = groups[3].Id, FlagUrl = "https://flagcdn.com/w320/tr.png" },

            // Group E
            new() { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA, FifaRanking = 11, GroupId = groups[4].Id, FlagUrl = "https://flagcdn.com/w320/de.png" },
            new() { Name = "Curaçao", Code = "CUW", Confederation = Confederation.CONCACAF, FifaRanking = 82, GroupId = groups[4].Id, FlagUrl = "https://flagcdn.com/w320/cw.png" },
            new() { Name = "Ivory Coast", Code = "CIV", Confederation = Confederation.CAF, FifaRanking = 52, GroupId = groups[4].Id, FlagUrl = "https://flagcdn.com/w320/ci.png" },
            new() { Name = "Ecuador", Code = "ECU", Confederation = Confederation.CONMEBOL, FifaRanking = 44, GroupId = groups[4].Id, FlagUrl = "https://flagcdn.com/w320/ec.png" },

            // Group F
            new() { Name = "Netherlands", Code = "NED", Confederation = Confederation.UEFA, FifaRanking = 6, GroupId = groups[5].Id, FlagUrl = "https://flagcdn.com/w320/nl.png" },
            new() { Name = "Japan", Code = "JPN", Confederation = Confederation.AFC, FifaRanking = 20, GroupId = groups[5].Id, FlagUrl = "https://flagcdn.com/w320/jp.png" },
            new() { Name = "Sweden", Code = "SWE", Confederation = Confederation.UEFA, FifaRanking = 16, GroupId = groups[5].Id, FlagUrl = "https://flagcdn.com/w320/se.png" },
            new() { Name = "Tunisia", Code = "TUN", Confederation = Confederation.CAF, FifaRanking = 30, GroupId = groups[5].Id, FlagUrl = "https://flagcdn.com/w320/tn.png" },

            // Group G
            new() { Name = "Belgium", Code = "BEL", Confederation = Confederation.UEFA, FifaRanking = 3, GroupId = groups[6].Id, FlagUrl = "https://flagcdn.com/w320/be.png" },
            new() { Name = "Egypt", Code = "EGY", Confederation = Confederation.CAF, FifaRanking = 36, GroupId = groups[6].Id, FlagUrl = "https://flagcdn.com/w320/eg.png" },
            new() { Name = "Iran", Code = "IRN", Confederation = Confederation.AFC, FifaRanking = 21, GroupId = groups[6].Id, FlagUrl = "https://flagcdn.com/w320/ir.png" },
            new() { Name = "New Zealand", Code = "NZL", Confederation = Confederation.OFC, FifaRanking = 101, GroupId = groups[6].Id, FlagUrl = "https://flagcdn.com/w320/nz.png" },

            // Group H
            new() { Name = "Spain", Code = "ESP", Confederation = Confederation.UEFA, FifaRanking = 7, GroupId = groups[7].Id, FlagUrl = "https://flagcdn.com/w320/es.png" },
            new() { Name = "Cape Verde", Code = "CPV", Confederation = Confederation.CAF, FifaRanking = 73, GroupId = groups[7].Id, FlagUrl = "https://flagcdn.com/w320/cv.png" },
            new() { Name = "Saudi Arabia", Code = "KSA", Confederation = Confederation.AFC, FifaRanking = 53, GroupId = groups[7].Id, FlagUrl = "https://flagcdn.com/w320/sa.png" },
            new() { Name = "Uruguay", Code = "URU", Confederation = Confederation.CONMEBOL, FifaRanking = 14, GroupId = groups[7].Id, FlagUrl = "https://flagcdn.com/w320/uy.png" },

            // Group I
            new() { Name = "France", Code = "FRA", Confederation = Confederation.UEFA, FifaRanking = 2, GroupId = groups[8].Id, FlagUrl = "https://flagcdn.com/w320/fr.png" },
            new() { Name = "Senegal", Code = "SEN", Confederation = Confederation.CAF, FifaRanking = 18, GroupId = groups[8].Id, FlagUrl = "https://flagcdn.com/w320/sn.png" },
            new() { Name = "Iraq", Code = "IRQ", Confederation = Confederation.AFC, FifaRanking = 70, GroupId = groups[8].Id, FlagUrl = "https://flagcdn.com/w320/iq.png" },
            new() { Name = "Norway", Code = "NOR", Confederation = Confederation.UEFA, FifaRanking = 38, GroupId = groups[8].Id, FlagUrl = "https://flagcdn.com/w320/no.png" },

            // Group J
            new() { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1, GroupId = groups[9].Id, FlagUrl = "https://flagcdn.com/w320/ar.png" },
            new() { Name = "Algeria", Code = "ALG", Confederation = Confederation.CAF, FifaRanking = 37, GroupId = groups[9].Id, FlagUrl = "https://flagcdn.com/w320/dz.png" },
            new() { Name = "Austria", Code = "AUT", Confederation = Confederation.UEFA, FifaRanking = 32, GroupId = groups[9].Id, FlagUrl = "https://flagcdn.com/w320/at.png" },
            new() { Name = "Jordan", Code = "JOR", Confederation = Confederation.AFC, FifaRanking = 87, GroupId = groups[9].Id, FlagUrl = "https://flagcdn.com/w320/jo.png" },

            // Group K
            new() { Name = "Portugal", Code = "POR", Confederation = Confederation.UEFA, FifaRanking = 8, GroupId = groups[10].Id, FlagUrl = "https://flagcdn.com/w320/pt.png" },
            new() { Name = "DR Congo", Code = "COD", Confederation = Confederation.CAF, FifaRanking = 63, GroupId = groups[10].Id, FlagUrl = "https://flagcdn.com/w320/cd.png" },
            new() { Name = "Uzbekistan", Code = "UZB", Confederation = Confederation.AFC, FifaRanking = 68, GroupId = groups[10].Id, FlagUrl = "https://flagcdn.com/w320/uz.png" },
            new() { Name = "Colombia", Code = "COL", Confederation = Confederation.CONMEBOL, FifaRanking = 17, GroupId = groups[10].Id, FlagUrl = "https://flagcdn.com/w320/co.png" },

            // Group L
            new() { Name = "England", Code = "ENG", Confederation = Confederation.UEFA, FifaRanking = 4, GroupId = groups[11].Id, FlagUrl = "https://flagcdn.com/w320/gb-eng.png" },
            new() { Name = "Croatia", Code = "CRO", Confederation = Confederation.UEFA, FifaRanking = 12, GroupId = groups[11].Id, FlagUrl = "https://flagcdn.com/w320/hr.png" },
            new() { Name = "Ghana", Code = "GHA", Confederation = Confederation.CAF, FifaRanking = 61, GroupId = groups[11].Id, FlagUrl = "https://flagcdn.com/w320/gh.png" },
            new() { Name = "Panama", Code = "PAN", Confederation = Confederation.CONCACAF, FifaRanking = 56, GroupId = groups[11].Id, FlagUrl = "https://flagcdn.com/w320/pa.png" }
        };

        await _context.Teams.AddRangeAsync(teams, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

// Made with Bob
