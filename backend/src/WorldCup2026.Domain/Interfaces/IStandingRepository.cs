using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Domain.Interfaces;

/// <summary>
/// Repository interface for Standing entity with specific queries
/// </summary>
public interface IStandingRepository : IRepository<Standing>
{
    /// <summary>
    /// Get standing by team ID
    /// </summary>
    Task<Standing?> GetByTeamIdAsync(int teamId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get standings by group ID ordered by position
    /// </summary>
    Task<IEnumerable<Standing>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all standings ordered by group and position
    /// </summary>
    Task<IEnumerable<Standing>> GetAllOrderedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get top teams from each group (for knockout qualification)
    /// </summary>
    Task<IEnumerable<Standing>> GetQualifiedTeamsAsync(int teamsPerGroup = 2, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get standing with team and group details
    /// </summary>
    Task<Standing?> GetWithDetailsAsync(int standingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update standings for a specific group
    /// </summary>
    Task UpdateGroupStandingsAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Recalculate all standings
    /// </summary>
    Task RecalculateAllStandingsAsync(CancellationToken cancellationToken = default);
}

// Made with Bob