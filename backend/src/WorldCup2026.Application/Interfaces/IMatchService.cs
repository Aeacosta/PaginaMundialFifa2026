using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.Interfaces;

/// <summary>
/// Service interface for match management operations
/// </summary>
public interface IMatchService
{
    /// <summary>
    /// Get all matches with optional filtering and pagination
    /// </summary>
    Task<PagedResult<MatchDto>> GetAllMatchesAsync(
        int pageNumber = 1,
        int pageSize = 10,
        MatchPhase? phase = null,
        MatchStatus? status = null,
        int? groupId = null,
        int? stadiumId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a match by ID
    /// </summary>
    Task<MatchDto?> GetMatchByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all matches for a specific team
    /// </summary>
    Task<IEnumerable<MatchDto>> GetMatchesByTeamAsync(int teamId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all matches in a specific group
    /// </summary>
    Task<IEnumerable<MatchDto>> GetMatchesByGroupAsync(int groupId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all matches at a specific stadium
    /// </summary>
    Task<IEnumerable<MatchDto>> GetMatchesByStadiumAsync(int stadiumId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all matches by phase
    /// </summary>
    Task<IEnumerable<MatchDto>> GetMatchesByPhaseAsync(MatchPhase phase, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all matches by status
    /// </summary>
    Task<IEnumerable<MatchDto>> GetMatchesByStatusAsync(MatchStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get upcoming matches (scheduled but not started)
    /// </summary>
    Task<IEnumerable<MatchDto>> GetUpcomingMatchesAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get recent matches (completed)
    /// </summary>
    Task<IEnumerable<MatchDto>> GetRecentMatchesAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get live matches (in progress)
    /// </summary>
    Task<IEnumerable<MatchDto>> GetLiveMatchesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new match
    /// </summary>
    Task<MatchDto> CreateMatchAsync(CreateMatchDto createMatchDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update match result
    /// </summary>
    Task<MatchDto> UpdateMatchResultAsync(int id, UpdateMatchResultDto updateResultDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update match status
    /// </summary>
    Task<MatchDto> UpdateMatchStatusAsync(int id, MatchStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a match
    /// </summary>
    Task<bool> DeleteMatchAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a match exists by ID
    /// </summary>
    Task<bool> MatchExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validate if teams can play (not already playing at the same time)
    /// </summary>
    Task<bool> ValidateTeamAvailabilityAsync(int homeTeamId, int awayTeamId, DateTime matchDate, int? excludeMatchId = null, CancellationToken cancellationToken = default);
}

// Made with Bob
