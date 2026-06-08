using Microsoft.EntityFrameworkCore;
using WorldCup2026.Infrastructure.Data;

namespace WorldCup2026.Tests.Infrastructure;

/// <summary>
/// Base class for repository tests with in-memory database setup
/// </summary>
public abstract class RepositoryTestBase : IDisposable
{
    protected readonly WorldCupDbContext _context;
    private bool _disposed = false;

    protected RepositoryTestBase()
    {
        var options = new DbContextOptionsBuilder<WorldCupDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new WorldCupDbContext(options);
        _context.Database.EnsureCreated();
    }

    protected async Task SeedDataAsync()
    {
        // Override in derived classes to seed specific test data
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}

// Made with Bob