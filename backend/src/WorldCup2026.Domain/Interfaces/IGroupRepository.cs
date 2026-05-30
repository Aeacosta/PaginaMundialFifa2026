using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Domain.Interfaces;

/// <summary>
/// Repository interface for Group entity with specific queries
/// </summary>
public interface IGroupRepository : IRepository<Group>
{
    /// <summary>
    /// Get group by name (e.g., "Group A")
    /// </summary>
    Task<Group?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get group with all teams
    /// </summary>
    Task<Group?> GetWithTeamsAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get group with all matches
    /// </summary>
    Task<Group?> GetWithMatchesAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get group with standings ordered by position
    /// </summary>
    Task<Group?> GetWithStandingsAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get group with complete information (teams, matches, standings)
    /// </summary>
    Task<Group?> GetCompleteAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all groups with their teams
    /// </summary>
    Task<IEnumerable<Group>> GetAllWithTeamsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all groups with standings
    /// </summary>
    Task<IEnumerable<Group>> GetAllWithStandingsAsync(CancellationToken cancellationToken = default);
}

// Made with Bob