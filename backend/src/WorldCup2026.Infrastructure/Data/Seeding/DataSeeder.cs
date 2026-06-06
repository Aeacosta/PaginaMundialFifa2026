using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace WorldCup2026.Infrastructure.Data.Seeding;

/// <summary>
/// Main data seeder that orchestrates all individual seeders
/// </summary>
public class DataSeeder
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataSeeder> _logger;
    private readonly IConfiguration _configuration;

    public DataSeeder(
        IServiceProvider serviceProvider,
        ILogger<DataSeeder> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Seeds all data in the correct order
    /// </summary>
    /// <param name="useJsonForMatches">Override to force JSON seeding for matches</param>
    public async Task SeedAllAsync(bool? useJsonForMatches = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting database seeding...");

            using var scope = _serviceProvider.CreateScope();

            // Seed in order of dependencies
            await SeedGroupsAsync(scope, cancellationToken);
            await SeedStadiumsAsync(scope, cancellationToken);
            await SeedTeamsAsync(scope, cancellationToken);
            await SeedStandingsAsync(scope, cancellationToken);
            await SeedMatchesAsync(scope, useJsonForMatches, cancellationToken);

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

    private async Task SeedStandingsAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding standings...");
        var seeder = scope.ServiceProvider.GetRequiredService<StandingSeeder>();
        await seeder.SeedAsync(cancellationToken);
        _logger.LogInformation("Standings seeded successfully");
    }

    private async Task SeedMatchesAsync(IServiceScope scope, bool? useJsonForMatches, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding matches...");
        
        // Check configuration or parameter to determine seeding method
        var useJson = useJsonForMatches ?? bool.TryParse(_configuration["Seeding:UseJsonForMatches"], out var configValue) && configValue;
        
        if (useJson)
        {
            _logger.LogInformation("Using JSON-based match seeding");
            var jsonSeeder = scope.ServiceProvider.GetRequiredService<JsonMatchSeeder>();
            await jsonSeeder.SeedAsync(cancellationToken);
        }
        else
        {
            _logger.LogInformation("Using code-based match seeding");
            var seeder = scope.ServiceProvider.GetRequiredService<MatchSeeder>();
            await seeder.SeedAsync(cancellationToken);
        }
        
        _logger.LogInformation("Matches seeded successfully");
    }
}

// Made with Bob
