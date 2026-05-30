using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Domain.Interfaces;

/// <summary>
/// Repository interface for Match entity with specific queries
/// </summary>
public interface IMatchRepository : IRepository<Match>
{
    /// <summary>
    /// Get match with complete details (teams, stadium, result)
    /// </summary>
    Task<Match?> GetWithDetailsAsync(int matchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get matches by phase
    /// </summary>
    Task<IEnumerable<Match>> GetByPhaseAsync(MatchPhase phase, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get matches by status
    /// </summary>
    Task<IEnumerable<Match>> GetByStatusAsync(MatchStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get matches by group ID
    /// </summary>
    Task<IEnumerable<Match>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get matches by team ID (home or away)
    /// </summary>
    Task<IEnumerable<Match>> GetByTeamIdAsync(int teamId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get matches by stadium ID
    /// </summary>
    Task<IEnumerable<Match>> GetByStadiumIdAsync(int stadiumId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get matches by date range
    /// </summary>
    Task<IEnumerable<Match>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get upcoming matches (scheduled, ordered by date)
    /// </summary>
    Task<IEnumerable<Match>> GetUpcomingMatchesAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get recent results (finished matches, ordered by date descending)
    /// </summary>
    Task<IEnumerable<Match>> GetRecentResultsAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get matches for today
    /// </summary>
    Task<IEnumerable<Match>> GetTodayMatchesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get group stage matches by round
    /// </summary>
    Task<IEnumerable<Match>> GetGroupStageByRoundAsync(int round, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get knockout stage matches with results
    /// </summary>
    Task<IEnumerable<Match>> GetKnockoutMatchesAsync(CancellationToken cancellationToken = default);
}

// Made with Bob