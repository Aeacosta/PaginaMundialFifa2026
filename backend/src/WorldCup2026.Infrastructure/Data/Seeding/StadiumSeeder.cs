using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Infrastructure.Data.Seeding;

/// <summary>
/// Seeds stadium data for FIFA World Cup 2026
/// 16 stadiums across USA, Mexico, and Canada
/// </summary>
public class StadiumSeeder : IDataSeeder
{
    private readonly WorldCupDbContext _context;

    public StadiumSeeder(WorldCupDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if stadiums already exist
        if (await _context.Stadiums.AnyAsync(cancellationToken))
        {
            return;
        }

        var stadiums = new List<Stadium>
        {
            // USA Stadiums (11)
            new()
            {
                Name = "MetLife Stadium",
                City = "East Rutherford",
                Country = "USA",
                Capacity = 82500,
                Latitude = 40.8128m,
                Longitude = -74.0742m
            },
            new()
            {
                Name = "AT&T Stadium",
                City = "Arlington",
                Country = "USA",
                Capacity = 80000,
                Latitude = 32.7473m,
                Longitude = -97.0945m
            },
            new()
            {
                Name = "Arrowhead Stadium",
                City = "Kansas City",
                Country = "USA",
                Capacity = 76416,
                Latitude = 39.0489m,
                Longitude = -94.4839m
            },
            new()
            {
                Name = "SoFi Stadium",
                City = "Inglewood",
                Country = "USA",
                Capacity = 70240,
                Latitude = 33.9535m,
                Longitude = -118.3392m
            },
            new()
            {
                Name = "Mercedes-Benz Stadium",
                City = "Atlanta",
                Country = "USA",
                Capacity = 71000,
                Latitude = 33.7553m,
                Longitude = -84.4006m
            },
            new()
            {
                Name = "NRG Stadium",
                City = "Houston",
                Country = "USA",
                Capacity = 72220,
                Latitude = 29.6847m,
                Longitude = -95.4107m
            },
            new()
            {
                Name = "Lincoln Financial Field",
                City = "Philadelphia",
                Country = "USA",
                Capacity = 69796,
                Latitude = 39.9008m,
                Longitude = -75.1675m
            },
            new()
            {
                Name = "Lumen Field",
                City = "Seattle",
                Country = "USA",
                Capacity = 69000,
                Latitude = 47.5952m,
                Longitude = -122.3316m
            },
            new()
            {
                Name = "Levi's Stadium",
                City = "Santa Clara",
                Country = "USA",
                Capacity = 68500,
                Latitude = 37.4032m,
                Longitude = -121.9698m
            },
            new()
            {
                Name = "Hard Rock Stadium",
                City = "Miami Gardens",
                Country = "USA",
                Capacity = 64767,
                Latitude = 25.9580m,
                Longitude = -80.2389m
            },
            new()
            {
                Name = "Gillette Stadium",
                City = "Foxborough",
                Country = "USA",
                Capacity = 65878,
                Latitude = 42.0909m,
                Longitude = -71.2643m
            },
            // Mexico Stadiums (3)
            new()
            {
                Name = "Estadio Azteca",
                City = "Mexico City",
                Country = "Mexico",
                Capacity = 87523,
                Latitude = 19.3030m,
                Longitude = -99.1506m
            },
            new()
            {
                Name = "Estadio BBVA",
                City = "Monterrey",
                Country = "Mexico",
                Capacity = 53500,
                Latitude = 25.7207m,
                Longitude = -100.2441m
            },
            new()
            {
                Name = "Estadio Akron",
                City = "Guadalajara",
                Country = "Mexico",
                Capacity = 46232,
                Latitude = 20.6917m,
                Longitude = -103.4641m
            },
            // Canada Stadiums (2)
            new()
            {
                Name = "BMO Field",
                City = "Toronto",
                Country = "Canada",
                Capacity = 45500,
                Latitude = 43.6332m,
                Longitude = -79.4185m
            },
            new()
            {
                Name = "BC Place",
                City = "Vancouver",
                Country = "Canada",
                Capacity = 54500,
                Latitude = 49.2768m,
                Longitude = -123.1119m
            }
        };

        await _context.Stadiums.AddRangeAsync(stadiums, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

// Made with Bob
