using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.Interfaces;

/// <summary>
/// Service interface for team management operations
/// </summary>
public interface ITeamService
{
    /// <summary>
    /// Get all teams with optional filtering and pagination
    /// </summary>
    Task<PagedResult<TeamDto>> GetAllTeamsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        Confederation? confederation = null,
        int? groupId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a team by ID
    /// </summary>
    Task<TeamDto?> GetTeamByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a team by FIFA code
    /// </summary>
    Task<TeamDto?> GetTeamByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all teams in a specific group
    /// </summary>
    Task<IEnumerable<TeamDto>> GetTeamsByGroupAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all teams from a specific confederation
    /// </summary>
    Task<IEnumerable<TeamDto>> GetTeamsByConfederationAsync(Confederation confederation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new team
    /// </summary>
    Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an existing team
    /// </summary>
    Task<TeamDto> UpdateTeamAsync(int id, UpdateTeamDto updateTeamDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a team
    /// </summary>
    Task<bool> DeleteTeamAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a team exists by ID
    /// </summary>
    Task<bool> TeamExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a FIFA code is already in use
    /// </summary>
    Task<bool> CodeExistsAsync(string code, int? excludeTeamId = null, CancellationToken cancellationToken = default);
}

// Made with Bob
