using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Interfaces;
using WorldCup2026.Infrastructure.Data;

namespace WorldCup2026.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Stadium entity
/// </summary>
public class StadiumRepository : Repository<Stadium>, IStadiumRepository
{
    public StadiumRepository(WorldCupDbContext context) : base(context)
    {
    }

    public async Task<Stadium?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Stadium>> GetByCountryAsync(string country, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.Country == country)
            .OrderBy(s => s.City)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Stadium>> GetByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.City == city)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Stadium?> GetWithMatchesAsync(int stadiumId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Matches)
                .ThenInclude(m => m.HomeTeam)
            .Include(s => s.Matches)
                .ThenInclude(m => m.AwayTeam)
            .Include(s => s.Matches)
                .ThenInclude(m => m.Result)
            .FirstOrDefaultAsync(s => s.Id == stadiumId, cancellationToken);
    }

    public async Task<IEnumerable<Stadium>> GetOrderedByCapacityAsync(bool descending = true, CancellationToken cancellationToken = default)
    {
        if (descending)
        {
            return await _dbSet
                .OrderByDescending(s => s.Capacity)
                .ThenBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }
        else
        {
            return await _dbSet
                .OrderBy(s => s.Capacity)
                .ThenBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<Stadium>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => EF.Functions.Like(s.Name, $"%{searchTerm}%") ||
                       EF.Functions.Like(s.City, $"%{searchTerm}%") ||
                       EF.Functions.Like(s.Country, $"%{searchTerm}%"))
            .OrderBy(s => s.Country)
            .ThenBy(s => s.City)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Stadium>> GetWithCoordinatesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.Latitude != null && s.Longitude != null)
            .OrderBy(s => s.Country)
            .ThenBy(s => s.City)
            .ToListAsync(cancellationToken);
    }
}

// Made with Bob