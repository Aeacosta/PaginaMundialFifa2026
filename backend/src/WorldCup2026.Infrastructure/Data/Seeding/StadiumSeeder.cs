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
                ImageUrl = "https://images.unsplash.com/photo-1508098682722-e99c43a406b2?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 40.8128m,
                Longitude = -74.0742m
            },
            new()
            {
                Name = "AT&T Stadium",
                City = "Arlington",
                Country = "USA",
                Capacity = 80000,
                ImageUrl = "https://images.unsplash.com/photo-1577223625816-7546f13df25d?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 32.7473m,
                Longitude = -97.0945m
            },
            new()
            {
                Name = "Arrowhead Stadium",
                City = "Kansas City",
                Country = "USA",
                Capacity = 76416,
                ImageUrl = "https://images.unsplash.com/photo-1459865264687-595d652de67e?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 39.0489m,
                Longitude = -94.4839m
            },
            new()
            {
                Name = "SoFi Stadium",
                City = "Inglewood",
                Country = "USA",
                Capacity = 70240,
                ImageUrl = "https://images.unsplash.com/photo-1522778119026-d647f0596c20?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 33.9535m,
                Longitude = -118.3392m
            },
            new()
            {
                Name = "Mercedes-Benz Stadium",
                City = "Atlanta",
                Country = "USA",
                Capacity = 71000,
                ImageUrl = "https://images.unsplash.com/photo-1431324155629-1a6deb1dec8d?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 33.7553m,
                Longitude = -84.4006m
            },
            new()
            {
                Name = "NRG Stadium",
                City = "Houston",
                Country = "USA",
                Capacity = 72220,
                ImageUrl = "https://images.unsplash.com/photo-1574629810360-7efbbe195018?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 29.6847m,
                Longitude = -95.4107m
            },
            new()
            {
                Name = "Lincoln Financial Field",
                City = "Philadelphia",
                Country = "USA",
                Capacity = 69796,
                ImageUrl = "https://images.unsplash.com/photo-1487466365202-1afdb86c764e?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 39.9008m,
                Longitude = -75.1675m
            },
            new()
            {
                Name = "Lumen Field",
                City = "Seattle",
                Country = "USA",
                Capacity = 69000,
                ImageUrl = "https://images.unsplash.com/photo-1551958219-acbc608c6377?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 47.5952m,
                Longitude = -122.3316m
            },
            new()
            {
                Name = "Levi's Stadium",
                City = "Santa Clara",
                Country = "USA",
                Capacity = 68500,
                ImageUrl = "https://images.unsplash.com/photo-1529900748604-07564a03e7a6?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 37.4032m,
                Longitude = -121.9698m
            },
            new()
            {
                Name = "Hard Rock Stadium",
                City = "Miami Gardens",
                Country = "USA",
                Capacity = 64767,
                ImageUrl = "https://images.unsplash.com/photo-1540747913346-19e32dc3e97e?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
                Latitude = 25.9580m,
                Longitude = -80.2389m
            },
            new()
            {
                Name = "Gillette Stadium",
                City = "Foxborough",
                Country = "USA",
                Capacity = 65878,
                ImageUrl = "https://images.unsplash.com/photo-1566577739112-5180d4bf9390?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/us.png",
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
                ImageUrl = "https://images.unsplash.com/photo-1624880357913-a8539238245b?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/mx.png",
                Latitude = 19.3030m,
                Longitude = -99.1506m
            },
            new()
            {
                Name = "Estadio BBVA",
                City = "Monterrey",
                Country = "Mexico",
                Capacity = 53500,
                ImageUrl = "https://images.unsplash.com/photo-1589487391730-58f20eb2c308?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/mx.png",
                Latitude = 25.7207m,
                Longitude = -100.2441m
            },
            new()
            {
                Name = "Estadio Akron",
                City = "Guadalajara",
                Country = "Mexico",
                Capacity = 46232,
                ImageUrl = "https://images.unsplash.com/photo-1577223625816-7546f13df25d?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/mx.png",
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
                ImageUrl = "https://images.unsplash.com/photo-1522778526097-ce0a22ceb253?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/ca.png",
                Latitude = 43.6332m,
                Longitude = -79.4185m
            },
            new()
            {
                Name = "BC Place",
                City = "Vancouver",
                Country = "Canada",
                Capacity = 54500,
                ImageUrl = "https://images.unsplash.com/photo-1512719994953-eabf50895df7?w=800&h=600&fit=crop",
                FlagUrl = "https://flagcdn.com/w320/ca.png",
                Latitude = 49.2768m,
                Longitude = -123.1119m
            }
        };

        await _context.Stadiums.AddRangeAsync(stadiums, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

// Made with Bob
