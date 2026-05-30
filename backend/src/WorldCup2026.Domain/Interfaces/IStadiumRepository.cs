using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Domain.Interfaces;

/// <summary>
/// Repository interface for Stadium entity with specific queries
/// </summary>
public interface IStadiumRepository : IRepository<Stadium>
{
    /// <summary>
    /// Get stadium by name
    /// </summary>
    Task<Stadium?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get stadiums by country
    /// </summary>
    Task<IEnumerable<Stadium>> GetByCountryAsync(string country, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get stadiums by city
    /// </summary>
    Task<IEnumerable<Stadium>> GetByCityAsync(string city, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get stadium with all matches
    /// </summary>
    Task<Stadium?> GetWithMatchesAsync(int stadiumId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get stadiums ordered by capacity
    /// </summary>
    Task<IEnumerable<Stadium>> GetOrderedByCapacityAsync(bool descending = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search stadiums by name or city
    /// </summary>
    Task<IEnumerable<Stadium>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get stadiums with location coordinates
    /// </summary>
    Task<IEnumerable<Stadium>> GetWithCoordinatesAsync(CancellationToken cancellationToken = default);
}

// Made with Bob