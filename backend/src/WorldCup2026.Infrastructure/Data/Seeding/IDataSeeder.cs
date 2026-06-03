namespace WorldCup2026.Infrastructure.Data.Seeding;

/// <summary>
/// Interface for data seeding operations
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    /// Seeds data into the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task SeedAsync(CancellationToken cancellationToken = default);
}

// Made with Bob
