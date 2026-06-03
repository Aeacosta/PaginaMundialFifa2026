using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WorldCup2026.Infrastructure.Data.Seeding;

/// <summary>
/// Main data seeder that orchestrates all individual seeders
/// </summary>
public class DataSeeder
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(IServiceProvider serviceProvider, ILogger<DataSeeder> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Seeds all data in the correct order
    /// </summary>
    public async Task SeedAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting database seeding...");

            using var scope = _serviceProvider.CreateScope();

            // Seed in order of dependencies
            await SeedGroupsAsync(scope, cancellationToken);
            await SeedStadiumsAsync(scope, cancellationToken);
            await SeedTeamsAsync(scope, cancellationToken);

            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task SeedGroupsAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding groups...");
        var seeder = scope.ServiceProvider.GetRequiredService<GroupSeeder>();
        await seeder.SeedAsync(cancellationToken);
        _logger.LogInformation("Groups seeded successfully");
    }

    private async Task SeedStadiumsAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding stadiums...");
        var seeder = scope.ServiceProvider.GetRequiredService<StadiumSeeder>();
        await seeder.SeedAsync(cancellationToken);
        _logger.LogInformation("Stadiums seeded successfully");
    }

    private async Task SeedTeamsAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding teams...");
        var seeder = scope.ServiceProvider.GetRequiredService<TeamSeeder>();
        await seeder.SeedAsync(cancellationToken);
        _logger.LogInformation("Teams seeded successfully");
    }
}

// Made with Bob
