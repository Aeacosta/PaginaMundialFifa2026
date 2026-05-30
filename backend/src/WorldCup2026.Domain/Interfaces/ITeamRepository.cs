using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Domain.Interfaces;

/// <summary>
/// Repository interface for Team entity with specific queries
/// </summary>
public interface ITeamRepository : IRepository<Team>
{
    /// <summary>
    /// Get team by country code
    /// </summary>
    Task<Team?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get teams by group ID with standings
    /// </summary>
    Task<IEnumerable<Team>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get teams by confederation
    /// </summary>
    Task<IEnumerable<Team>> GetByConfederationAsync(Confederation confederation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get team with all matches (home and away)
    /// </summary>
    Task<Team?> GetWithMatchesAsync(int teamId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get team with standing information
    /// </summary>
    Task<Team?> GetWithStandingAsync(int teamId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get teams without a group assignment
    /// </summary>
    Task<IEnumerable<Team>> GetUnassignedTeamsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Search teams by name
    /// </summary>
    Task<IEnumerable<Team>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
}

// Made with Bob