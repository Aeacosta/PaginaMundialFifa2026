using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Stadium;

namespace WorldCup2026.Application.Interfaces;

/// <summary>
/// Service interface for stadium management operations
/// </summary>
public interface IStadiumService
{
    /// <summary>
    /// Get all stadiums with optional filtering and pagination
    /// </summary>
    Task<PagedResult<StadiumDto>> GetAllStadiumsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? city = null,
        string? country = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a stadium by ID
    /// </summary>
    Task<StadiumDto?> GetStadiumByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all stadiums in a specific city
    /// </summary>
    Task<IEnumerable<StadiumDto>> GetStadiumsByCityAsync(string city, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all stadiums in a specific country
    /// </summary>
    Task<IEnumerable<StadiumDto>> GetStadiumsByCountryAsync(string country, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new stadium
    /// </summary>
    Task<StadiumDto> CreateStadiumAsync(CreateStadiumDto createStadiumDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an existing stadium
    /// </summary>
    Task<StadiumDto> UpdateStadiumAsync(int id, CreateStadiumDto updateStadiumDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a stadium
    /// </summary>
    Task<bool> DeleteStadiumAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a stadium exists by ID
    /// </summary>
    Task<bool> StadiumExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a stadium has scheduled matches
    /// </summary>
    Task<bool> HasScheduledMatchesAsync(int id, CancellationToken cancellationToken = default);
}

// Made with Bob
