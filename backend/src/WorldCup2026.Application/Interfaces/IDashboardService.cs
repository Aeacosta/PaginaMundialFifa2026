using WorldCup2026.Application.DTOs.Dashboard;

namespace WorldCup2026.Application.Interfaces;

/// <summary>
/// Service interface for dashboard and statistics operations
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Get complete dashboard data including stats and recent/upcoming matches
    /// </summary>
    Task<DashboardDto> GetDashboardDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get tournament statistics only
    /// </summary>
    Task<TournamentStatsDto> GetTournamentStatsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get top scorers across all matches
    /// </summary>
    Task<IEnumerable<TopScorerDto>> GetTopScorersAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get teams with most wins
    /// </summary>
    Task<IEnumerable<TeamPerformanceDto>> GetTopTeamsByWinsAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get teams with best goal difference
    /// </summary>
    Task<IEnumerable<TeamPerformanceDto>> GetTopTeamsByGoalDifferenceAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get teams with highest points
    /// </summary>
    Task<IEnumerable<TeamPerformanceDto>> GetTopTeamsByPointsAsync(int count = 10, CancellationToken cancellationToken = default);
}

// Made with Bob
