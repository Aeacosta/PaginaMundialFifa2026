using Microsoft.EntityFrameworkCore.Storage;
using WorldCup2026.Domain.Interfaces;
using WorldCup2026.Infrastructure.Data;

namespace WorldCup2026.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing transactions and repository access
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly WorldCupDbContext _context;
    private IDbContextTransaction? _transaction;

    // Lazy initialization of repositories
    private ITeamRepository? _teams;
    private IGroupRepository? _groups;
    private IMatchRepository? _matches;
    private IStandingRepository? _standings;
    private IStadiumRepository? _stadiums;

    public UnitOfWork(WorldCupDbContext context)
    {
        _context = context;
    }

    public ITeamRepository Teams
    {
        get
        {
            _teams ??= new TeamRepository(_context);
            return _teams;
        }
    }

    public IGroupRepository Groups
    {
        get
        {
            _groups ??= new GroupRepository(_context);
            return _groups;
        }
    }

    public IMatchRepository Matches
    {
        get
        {
            _matches ??= new MatchRepository(_context);
            return _matches;
        }
    }

    public IStandingRepository Standings
    {
        get
        {
            _standings ??= new StandingRepository(_context);
            return _standings;
        }
    }

    public IStadiumRepository Stadiums
    {
        get
        {
            _stadiums ??= new StadiumRepository(_context);
            return _stadiums;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

// Made with Bob