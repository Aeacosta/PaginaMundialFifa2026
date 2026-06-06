using Microsoft.EntityFrameworkCore;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Domain.Interfaces;
using WorldCup2026.Infrastructure.Data;

namespace WorldCup2026.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Match entity
/// </summary>
public class MatchRepository : Repository<Match>, IMatchRepository
{
    public MatchRepository(WorldCupDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Match>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Group)
            .Include(m => m.Result)
            .OrderBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Match?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Group)
            .Include(m => m.Result)
                .ThenInclude(r => r!.WinnerTeam)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Match?> GetWithDetailsAsync(int matchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Group)
            .Include(m => m.Result)
                .ThenInclude(r => r!.WinnerTeam)
            .FirstOrDefaultAsync(m => m.Id == matchId, cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetByPhaseAsync(MatchPhase phase, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Result)
            .Where(m => m.Phase == phase)
            .OrderBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetByStatusAsync(MatchStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Result)
            .Where(m => m.Status == status)
            .OrderBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Result)
            .Where(m => m.GroupId == groupId)
            .OrderBy(m => m.Round)
            .ThenBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetByTeamIdAsync(int teamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Result)
            .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
            .OrderBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetByStadiumIdAsync(int stadiumId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Result)
            .Where(m => m.StadiumId == stadiumId)
            .OrderBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Result)
            .Where(m => m.MatchDate >= startDate && m.MatchDate <= endDate)
            .OrderBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetUpcomingMatchesAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Where(m => m.Status == MatchStatus.Scheduled && m.MatchDate >= now)
            .OrderBy(m => m.MatchDate)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetRecentResultsAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Result)
            .Where(m => m.Status == MatchStatus.Finished)
            .OrderByDescending(m => m.MatchDate)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetTodayMatchesAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Result)
            .Where(m => m.MatchDate >= today && m.MatchDate < tomorrow)
            .OrderBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetGroupStageByRoundAsync(int round, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Group)
            .Include(m => m.Result)
            .Where(m => m.Phase == MatchPhase.GroupStage && m.Round == round)
            .OrderBy(m => m.GroupId)
            .ThenBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetKnockoutMatchesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Result)
                .ThenInclude(r => r!.WinnerTeam)
            .Where(m => m.Phase != MatchPhase.GroupStage)
            .OrderBy(m => m.Phase)
            .ThenBy(m => m.MatchDate)
            .ToListAsync(cancellationToken);
    }
}

// Made with Bob