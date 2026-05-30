using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Interfaces;
using WorldCup2026.Infrastructure.Data;

namespace WorldCup2026.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Group entity
/// </summary>
public class GroupRepository : Repository<Group>, IGroupRepository
{
    public GroupRepository(WorldCupDbContext context) : base(context)
    {
    }

    public async Task<Group?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
    }

    public async Task<Group?> GetWithTeamsAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Teams)
            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }

    public async Task<Group?> GetWithMatchesAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Matches)
                .ThenInclude(m => m.HomeTeam)
            .Include(g => g.Matches)
                .ThenInclude(m => m.AwayTeam)
            .Include(g => g.Matches)
                .ThenInclude(m => m.Stadium)
            .Include(g => g.Matches)
                .ThenInclude(m => m.Result)
            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }

    public async Task<Group?> GetWithStandingsAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Standings.OrderBy(s => s.Position))
                .ThenInclude(s => s.Team)
            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }

    public async Task<Group?> GetCompleteAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Teams)
            .Include(g => g.Matches)
                .ThenInclude(m => m.HomeTeam)
            .Include(g => g.Matches)
                .ThenInclude(m => m.AwayTeam)
            .Include(g => g.Matches)
                .ThenInclude(m => m.Stadium)
            .Include(g => g.Matches)
                .ThenInclude(m => m.Result)
            .Include(g => g.Standings.OrderBy(s => s.Position))
                .ThenInclude(s => s.Team)
            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }

    public async Task<IEnumerable<Group>> GetAllWithTeamsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Teams)
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Group>> GetAllWithStandingsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Standings.OrderBy(s => s.Position))
                .ThenInclude(s => s.Team)
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);
    }
}

// Made with Bob