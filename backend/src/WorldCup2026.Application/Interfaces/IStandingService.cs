using WorldCup2026.Application.DTOs.Standing;

namespace WorldCup2026.Application.Interfaces;

/// <summary>
/// Service interface for standings management operations
/// </summary>
public interface IStandingService
{
    /// <summary>
    /// Get all standings for a specific group, ordered by position
    /// </summary>
    Task<IEnumerable<StandingDto>> GetStandingsByGroupAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get standing for a specific team
    /// </summary>
    Task<StandingDto?> GetStandingByTeamAsync(int teamId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Recalculate standings for a specific group based on match results
    /// This will update all team standings in the group
    /// </summary>
    Task RecalculateGroupStandingsAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Recalculate standings for all groups
    /// </summary>
    Task RecalculateAllStandingsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get top teams across all groups (for knockout phase qualification)
    /// </summary>
    Task<IEnumerable<StandingDto>> GetTopTeamsAsync(int count = 16, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get qualified teams from a specific group (typically top 2)
    /// </summary>
    Task<IEnumerable<StandingDto>> GetQualifiedTeamsFromGroupAsync(int groupId, int count = 2, CancellationToken cancellationToken = default);
}

// Made with Bob
