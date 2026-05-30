# Phase 2: Repository and Data Access Layer - Progress Log

## Overview
Phase 2 implemented the Repository Pattern with Unit of Work, providing a clean abstraction layer over Entity Framework Core for data access operations.

---

## Phase 2.1: Create Repository Interfaces ✅ COMPLETED

### Generic Repository Interface:
**File:** `backend/src/WorldCup2026.Domain/Interfaces/IRepository.cs`

**Methods:**
- `GetByIdAsync(int id)` - Get single entity by ID
- `GetAllAsync()` - Get all entities
- `FindAsync(Expression<Func<T, bool>> predicate)` - Find entities by condition
- `AddAsync(T entity)` - Add new entity
- `Update(T entity)` - Update existing entity
- `Delete(T entity)` - Delete entity
- `CountAsync()` - Count all entities
- `AnyAsync(Expression<Func<T, bool>> predicate)` - Check if any entity matches condition

**Features:**
- Generic type constraint: `where T : BaseEntity`
- All methods async except Update/Delete (EF Core pattern)
- Expression-based queries for flexibility
- CancellationToken support

### Specific Repository Interfaces:

#### 1. ITeamRepository
**File:** `backend/src/WorldCup2026.Domain/Interfaces/ITeamRepository.cs`

**Additional Methods:**
- `GetByCodeAsync(string code)` - Get team by FIFA code
- `GetByGroupAsync(int groupId)` - Get all teams in a group
- `GetByConfederationAsync(Confederation confederation)` - Get teams by confederation
- `SearchTeamsAsync(string searchTerm)` - Search teams by name or code

#### 2. IGroupRepository
**File:** `backend/src/WorldCup2026.Domain/Interfaces/IGroupRepository.cs`

**Additional Methods:**
- `GetByNameAsync(string name)` - Get group by name (A, B, C, etc.)
- `GetWithTeamsAsync(int id)` - Get group with teams included
- `GetWithStandingsAsync(int id)` - Get group with standings included

#### 3. IMatchRepository
**File:** `backend/src/WorldCup2026.Domain/Interfaces/IMatchRepository.cs`

**Additional Methods:**
- `GetByTeamAsync(int teamId)` - Get all matches for a team
- `GetByGroupAsync(int groupId)` - Get all matches in a group
- `GetByStadiumAsync(int stadiumId)` - Get all matches at a stadium
- `GetByPhaseAsync(MatchPhase phase)` - Get matches by tournament phase
- `GetByStatusAsync(MatchStatus status)` - Get matches by status
- `GetByDateRangeAsync(DateTime from, DateTime to)` - Get matches in date range
- `GetUpcomingMatchesAsync(int count)` - Get next scheduled matches
- `GetRecentMatchesAsync(int count)` - Get recently completed matches

#### 4. IStandingRepository
**File:** `backend/src/WorldCup2026.Domain/Interfaces/IStandingRepository.cs`

**Additional Methods:**
- `GetByGroupAsync(int groupId)` - Get standings for a group (ordered)
- `GetByTeamAsync(int teamId)` - Get standing for a specific team
- `RecalculateStandingsAsync(int groupId)` - Recalculate standings based on match results

**Special Feature:**
- Automatic standings calculation from match results
- Proper ordering by Points → GoalDifference → GoalsFor

#### 5. IStadiumRepository
**File:** `backend/src/WorldCup2026.Domain/Interfaces/IStadiumRepository.cs`

**Additional Methods:**
- `GetByCityAsync(string city)` - Get stadiums in a city
- `GetByCountryAsync(string country)` - Get stadiums in a country
- `SearchStadiumsAsync(string searchTerm)` - Search stadiums by name, city, or country

### Unit of Work Interface:
**File:** `backend/src/WorldCup2026.Domain/Interfaces/IUnitOfWork.cs`

**Properties:**
- `Teams` - ITeamRepository
- `Groups` - IGroupRepository
- `Matches` - IMatchRepository
- `Standings` - IStandingRepository
- `Stadiums` - IStadiumRepository

**Methods:**
- `SaveChangesAsync()` - Commit all changes
- `BeginTransactionAsync()` - Start database transaction
- `CommitTransactionAsync()` - Commit transaction
- `RollbackTransactionAsync()` - Rollback transaction

**Features:**
- Implements IDisposable
- Transaction support for complex operations
- Single point of commit for all repositories

**Statistics:** 6 interfaces, 40+ methods total

---

## Phase 2.2: Implement Repositories ✅ COMPLETED

### Generic Repository Implementation:
**File:** `backend/src/WorldCup2026.Infrastructure/Repositories/Repository.cs`

**Implementation Details:**
- Protected DbContext and DbSet fields
- All CRUD operations implemented
- Proper async/await patterns
- AsNoTracking for read operations
- Expression-based queries

**Key Methods:**
```csharp
public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    => await _dbSet.FindAsync(new object[] { id }, cancellationToken);

public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    => await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
```

### Specific Repository Implementations:

#### 1. TeamRepository
**File:** `backend/src/WorldCup2026.Infrastructure/Repositories/TeamRepository.cs`

**Implementation Highlights:**
- Includes Group navigation in queries
- Case-insensitive search
- Efficient filtering by confederation
- FIFA code lookup

**Example:**
```csharp
public async Task<Team?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
{
    return await _context.Teams
        .Include(t => t.Group)
        .FirstOrDefaultAsync(t => t.Code == code.ToUpper(), cancellationToken);
}
```

#### 2. GroupRepository
**File:** `backend/src/WorldCup2026.Infrastructure/Repositories/GroupRepository.cs`

**Implementation Highlights:**
- Eager loading of Teams and Standings
- Ordered standings by position
- Case-insensitive name lookup

**Example:**
```csharp
public async Task<Group?> GetWithStandingsAsync(int id, CancellationToken cancellationToken = default)
{
    return await _context.Groups
        .Include(g => g.Standings.OrderBy(s => s.Position))
        .ThenInclude(s => s.Team)
        .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
}
```

#### 3. MatchRepository
**File:** `backend/src/WorldCup2026.Infrastructure/Repositories/MatchRepository.cs`

**Implementation Highlights:**
- Comprehensive navigation property loading
- Complex filtering by multiple criteria
- Date range queries
- Ordered results (upcoming by date ASC, recent by date DESC)

**Example:**
```csharp
public async Task<IEnumerable<Match>> GetUpcomingMatchesAsync(int count = 10, CancellationToken cancellationToken = default)
{
    return await _context.Matches
        .Include(m => m.HomeTeam)
        .Include(m => m.AwayTeam)
        .Include(m => m.Stadium)
        .Include(m => m.Group)
        .Where(m => m.Status == MatchStatus.Scheduled && m.MatchDate > DateTime.UtcNow)
        .OrderBy(m => m.MatchDate)
        .Take(count)
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}
```

#### 4. StandingRepository
**File:** `backend/src/WorldCup2026.Infrastructure/Repositories/StandingRepository.cs`

**Implementation Highlights:**
- **Automatic standings calculation** from match results
- Proper FIFA ranking rules (Points → GD → GF)
- Efficient bulk updates
- Transaction handling for recalculation

**RecalculateStandingsAsync Logic:**
1. Get all completed matches in the group
2. Calculate statistics for each team:
   - Matches played
   - Wins, draws, losses
   - Goals for/against
   - Goal difference
   - Points (3 for win, 1 for draw)
3. Order teams by: Points DESC → GoalDifference DESC → GoalsFor DESC
4. Assign positions
5. Update or create standing records

**Example:**
```csharp
public async Task RecalculateStandingsAsync(int groupId, CancellationToken cancellationToken = default)
{
    var matches = await _context.Matches
        .Include(m => m.Result)
        .Where(m => m.GroupId == groupId && m.Status == MatchStatus.Completed && m.Result != null)
        .ToListAsync(cancellationToken);

    // Calculate statistics for each team
    var teamStats = new Dictionary<int, TeamStats>();
    foreach (var match in matches)
    {
        // Update home team stats
        // Update away team stats
        // Calculate wins/draws/losses
        // Calculate goals and points
    }

    // Order and assign positions
    var orderedTeams = teamStats
        .OrderByDescending(t => t.Value.Points)
        .ThenByDescending(t => t.Value.GoalDifference)
        .ThenByDescending(t => t.Value.GoalsFor)
        .ToList();

    // Update standings in database
}
```

#### 5. StadiumRepository
**File:** `backend/src/WorldCup2026.Infrastructure/Repositories/StadiumRepository.cs`

**Implementation Highlights:**
- Geographic filtering (city, country)
- Multi-field search
- Case-insensitive queries
- Configurable sorting

**Example:**
```csharp
public async Task<IEnumerable<Stadium>> SearchStadiumsAsync(string searchTerm, CancellationToken cancellationToken = default)
{
    var search = searchTerm.ToLower();
    return await _context.Stadiums
        .Where(s => s.Name.ToLower().Contains(search) ||
                    s.City.ToLower().Contains(search) ||
                    s.Country.ToLower().Contains(search))
        .OrderBy(s => s.Name)
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}
```

### Unit of Work Implementation:
**File:** `backend/src/WorldCup2026.Infrastructure/Repositories/UnitOfWork.cs`

**Implementation Details:**
- Lazy initialization of repositories
- Single DbContext instance shared across repositories
- Transaction management via DbContext
- Proper disposal pattern

**Key Features:**
```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly WorldCupDbContext _context;
    private ITeamRepository? _teams;
    private IGroupRepository? _groups;
    // ... other repositories

    public ITeamRepository Teams => _teams ??= new TeamRepository(_context);
    public IGroupRepository Groups => _groups ??= new GroupRepository(_context);
    // ... other repository properties

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.CommitTransactionAsync(cancellationToken);

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.RollbackTransactionAsync(cancellationToken);
}
```

### Dependency Injection Registration:
**File:** `backend/src/WorldCup2026.API/Program.cs`

**Registered Services:**
```csharp
// DbContext
builder.Services.AddDbContext<WorldCupDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repositories
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IStandingRepository, StandingRepository>();
builder.Services.AddScoped<IStadiumRepository, StadiumRepository>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```

**Statistics:** 6 repository implementations, 1 UnitOfWork, 50+ methods

---

## Phase 2 Summary

### Achievements:
✅ Complete Repository Pattern implementation
✅ Unit of Work pattern for transaction management
✅ Generic repository with common operations
✅ 5 specialized repositories with domain-specific queries
✅ Automatic standings calculation logic
✅ Comprehensive navigation property loading
✅ Dependency injection configured

### Statistics:
- **Interfaces:** 6 (1 generic + 5 specific)
- **Implementations:** 6 (1 generic + 5 specific)
- **Total Methods:** 50+
- **Lines of Code:** ~1,200

### Key Features:
- **Repository Pattern** - Clean abstraction over EF Core
- **Unit of Work** - Transaction management and single commit point
- **Lazy Loading** - Repositories initialized on first access
- **Async/Await** - All operations asynchronous
- **AsNoTracking** - Optimized read operations
- **Expression Trees** - Flexible querying
- **Navigation Properties** - Eager loading where needed
- **Business Logic** - Standings calculation in repository

### Design Patterns Applied:
1. **Repository Pattern** - Data access abstraction
2. **Unit of Work Pattern** - Transaction coordination
3. **Dependency Injection** - Loose coupling
4. **Generic Repository** - Code reuse
5. **Lazy Initialization** - Performance optimization

### Special Implementations:

#### StandingRepository Calculation Logic:
- Reads completed matches from database
- Calculates team statistics (W/D/L, GF/GA, GD, Points)
- Applies FIFA ranking rules
- Updates standings with proper positions
- Handles edge cases (no matches, ties)

#### Match Repository Queries:
- Upcoming matches (future, ordered by date)
- Recent matches (past, ordered by date DESC)
- Complex filtering (phase, status, group, stadium, date range)
- Team-specific match history

### Build Status:
✅ All repositories compile successfully
✅ Zero warnings
✅ Zero errors
✅ Dependency injection configured
✅ Ready for service layer

### Problem Solved:
**Issue:** StadiumRepository had compilation error with OrderBy/ThenBy type inference
**Solution:** Restructured query into separate if/else blocks for each sort option

---

## Next Phase:
Phase 3: Application Layer - Services, DTOs, and Validators