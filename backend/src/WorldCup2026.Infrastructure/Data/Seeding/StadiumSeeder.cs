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
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/04/Metlife_stadium_%28Aerial_view%29.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://static.clubs.nfl.com/image/private/t_new_photo_album_2x/f_auto/cowboys/ow1uhkj5h1hayelquqbb.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://static.clubs.nfl.com/image/upload/t_new_photo_album_2x/f_auto/chiefs/lx8ffqfgn35u7czmyzxu.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://static.clubs.nfl.com/image/private/t_new_photo_album_2x/f_auto/chargers/fke9kc44elz18q9aklzq.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://di-uploads-pod47.dealerinspire.com/mercedesbenzoflittleton/uploads/2021/11/mbstadium.png?w=800&h=600&fit=crop",
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
                ImageUrl = "https://www.stadiumscene.tv/img/stadium/nfl/nrgstadium.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://s3.eu-north-1.amazonaws.com/sportsgrounds.us.com/Pennsylvania/Lincoln-Financial-Field4-68ad3dccc8526.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://cdn.prod.website-files.com/609445b9eb43c95f4a0b0e7b/614377a3561cd7fb26a377ca_lumen-field-aerial.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://static.clubs.nfl.com/image/upload/t_new_photo_album_2x/f_auto/49ers/ieuo8vfkz6j1c1kmefsk.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://www.tour2sky.com/_next/image?url=https%3A%2F%2Fapi.tour2sky.com%2Fstorage%2Fimages%2Fservice%2Fhard-rock-stadium-helicopter-ride-h1-2sbg8731.jpg&w=1920&q=75?w=800&h=600&fit=crop",
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
                ImageUrl = "https://media.bizj.us/view/img/11938659/gillette-stadium-05.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://lirp.cdn-website.com/7dc051fe/dms3rep/multi/opt/arquitecto+estadio+azteca+5-1920w.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://cdn-3.expansion.mx/dims4/default/9613cd2/2147483647/strip/true/crop/659x452+0+0/resize/800x549!/format/webp/quality/80/?url=https%3A%2F%2Fcdn-3.expansion.mx%2F18%2F18e32fe88230020890ac375adb565768%2Festadiobbvabancomeruno20150803104850.jpg&h=600&fit=crop",
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
                ImageUrl = "https://colombiaone.com/wp-content/uploads/2026/04/Estadio-Akron-1536x1024.jpg.webp?w=800&h=600&fit=crop",
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
                ImageUrl = "https://images.mlssoccer.com/image/private/t_editorial_landscape_12_desktop/mls-tor/tfhcxeuqozdayhjeb4bw.jpg?w=800&h=600&fit=crop",
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
                ImageUrl = "https://images.pexels.com/photos/33378699/pexels-photo-33378699.jpeg?w=800&h=600&fit=crop",
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
