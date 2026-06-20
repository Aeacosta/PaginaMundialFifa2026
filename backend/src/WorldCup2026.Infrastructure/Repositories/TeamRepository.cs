using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Domain.Interfaces;
using WorldCup2026.Infrastructure.Data;

namespace WorldCup2026.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Team entity
/// </summary>
public class TeamRepository : Repository<Team>, ITeamRepository
{
    public TeamRepository(WorldCupDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Group)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Team?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Group)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Team?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Group)
            .FirstOrDefaultAsync(t => t.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Team>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.GroupId == groupId)
            .Include(t => t.Group)
            .Include(t => t.Standing)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Team>> GetByConfederationAsync(Confederation confederation, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.Confederation == confederation)
            .Include(t => t.Group)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Team?> GetWithMatchesAsync(int teamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.HomeMatches)
                .ThenInclude(m => m.AwayTeam)
            .Include(t => t.HomeMatches)
                .ThenInclude(m => m.Stadium)
            .Include(t => t.HomeMatches)
                .ThenInclude(m => m.Result)
            .Include(t => t.AwayMatches)
                .ThenInclude(m => m.HomeTeam)
            .Include(t => t.AwayMatches)
                .ThenInclude(m => m.Stadium)
            .Include(t => t.AwayMatches)
                .ThenInclude(m => m.Result)
            .FirstOrDefaultAsync(t => t.Id == teamId, cancellationToken);
    }

    public async Task<Team?> GetWithStandingAsync(int teamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Standing)
                .ThenInclude(s => s!.Group)
            .Include(t => t.Group)
            .FirstOrDefaultAsync(t => t.Id == teamId, cancellationToken);
    }

    public async Task<IEnumerable<Team>> GetUnassignedTeamsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.GroupId == null)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Team>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => EF.Functions.Like(t.Name, $"%{searchTerm}%") || 
                       EF.Functions.Like(t.Code, $"%{searchTerm}%"))
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }
}

// Made with Bob