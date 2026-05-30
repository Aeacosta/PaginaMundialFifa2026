using WorldCup2026.Application.DTOs.Group;

namespace WorldCup2026.Application.Interfaces;

/// <summary>
/// Service interface for group management operations
/// </summary>
public interface IGroupService
{
    /// <summary>
    /// Get all groups
    /// </summary>
    Task<IEnumerable<GroupDto>> GetAllGroupsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a group by ID
    /// </summary>
    Task<GroupDto?> GetGroupByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a group by name (e.g., "A", "B", etc.)
    /// </summary>
    Task<GroupDto?> GetGroupByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a group with its current standings
    /// </summary>
    Task<GroupWithStandingsDto?> GetGroupWithStandingsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all groups with their standings
    /// </summary>
    Task<IEnumerable<GroupWithStandingsDto>> GetAllGroupsWithStandingsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new group
    /// </summary>
    Task<GroupDto> CreateGroupAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a group
    /// </summary>
    Task<GroupDto> UpdateGroupAsync(int id, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a group
    /// </summary>
    Task<bool> DeleteGroupAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a group exists by ID
    /// </summary>
    Task<bool> GroupExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a group name is already in use
    /// </summary>
    Task<bool> GroupNameExistsAsync(string name, int? excludeGroupId = null, CancellationToken cancellationToken = default);
}

// Made with Bob
