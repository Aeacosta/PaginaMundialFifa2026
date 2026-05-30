namespace WorldCup2026.Domain.Interfaces;

/// <summary>
/// Unit of Work pattern interface for managing transactions and repository access
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Team repository
    /// </summary>
    ITeamRepository Teams { get; }

    /// <summary>
    /// Group repository
    /// </summary>
    IGroupRepository Groups { get; }

    /// <summary>
    /// Match repository
    /// </summary>
    IMatchRepository Matches { get; }

    /// <summary>
    /// Standing repository
    /// </summary>
    IStandingRepository Standings { get; }

    /// <summary>
    /// Stadium repository
    /// </summary>
    IStadiumRepository Stadiums { get; }

    /// <summary>
    /// Save all changes to the database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begin a database transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commit the current transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rollback the current transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

// Made with Bob