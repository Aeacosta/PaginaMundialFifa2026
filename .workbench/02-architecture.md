# System Architecture - FIFA World Cup 2026 Application

## Architecture Pattern: Clean Architecture (Layered)

The application follows Clean Architecture principles with clear separation of concerns across multiple layers.

## High-Level Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                         Frontend (React)                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Pages      │  │  Components  │  │   Services   │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                            │ HTTP/REST
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    Backend (.NET 8 API)                      │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Presentation Layer                       │   │
│  │  Controllers, DTOs, Filters, Middleware              │   │
│  └──────────────────────────────────────────────────────┘   │
│                            │                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Application Layer                        │   │
│  │  Services, Business Logic, Validators                │   │
│  └──────────────────────────────────────────────────────┘   │
│                            │                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Domain Layer                             │   │
│  │  Entities, Interfaces, Domain Logic                  │   │
│  └──────────────────────────────────────────────────────┘   │
│                            │                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Infrastructure Layer                     │   │
│  │  DbContext, Repositories, External Services          │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
                    ┌───────────────┐
                    │ SQLite/       │
                    │ PostgreSQL    │
                    └───────────────┘
```

## Database Schema

### Core Tables
- **Teams** (48 teams)
 - Id, Name, Code, Confederation, FifaRanking, FlagUrl, GroupId
 - Indexes: Code, Name, Confederation
 
- **Groups** (12 groups: A-L)
 - Id, Name
 - Unique constraint on Name
 
- **Stadiums** (16 venues)
 - Id, Name, City, Country, Capacity, ImageUrl, FlagUrl, Latitude, Longitude
 - Indexes: Name, City, Country
 
- **Matches** (104 total)
 - Id, HomeTeamId, AwayTeamId, StadiumId, GroupId, Phase, Round, MatchDate, Status
 - Relationships to Teams, Stadium, Group
 
- **MatchResults**
 - Id, MatchId, HomeTeamScore, AwayTeamScore, HomeTeamPenalties, AwayTeamPenalties, WinnerTeamId, Highlights
 - One-to-one with Match
 
- **Standings** (48 records, 4 per group)
 - Id, TeamId, GroupId, Position, Played, Won, Drawn, Lost, GoalsFor, GoalsAgainst, GoalDifference, Points
 - Composite unique index: (TeamId, GroupId)
 - Auto-calculated from match results

### Relationships
- Team → Group (Many-to-One)
- Match → HomeTeam, AwayTeam (Many-to-One each)
- Match → Stadium (Many-to-One)
- Match → Group (Many-to-One, optional for knockout)
- Match → MatchResult (One-to-One)
- Standing → Team, Group (Many-to-One each)

## Backend Layers

### 1. Presentation Layer (WorldCup2026.API)
**Responsibility:** Handle HTTP requests and responses

**Components:**
- **Controllers:** RESTful API endpoints
  - TeamsController: Team CRUD operations with pagination
  - MatchesController: Match management, result updates, filtering
  - GroupsController: Group operations with standings
  - StadiumsController: Stadium management with location queries
  - StandingsController: Group standings and rankings
  - DashboardController: Statistics, analytics, and overview data
- **Filters:** Exception filters, action filters
- **Middleware:** Error handling, logging, CORS
- **Program.cs:** Application configuration with DI, Swagger, AutoMapper, FluentValidation

**Key Endpoints:**
- `/api/teams` - Team management with search and pagination
- `/api/matches` - Match operations with phase/status/date filtering
- `/api/groups` - Group management with standings
- `/api/stadiums` - Stadium operations with city/country filtering
- `/api/standings` - Group standings and team rankings
- `/api/dashboard` - Tournament statistics, top teams, upcoming/recent matches

**Key Principles:**
- Thin controllers (delegate to services)
- FluentValidation for input validation
- Standardized response formatting
- HTTP status code management (200, 201, 400, 404, 422, 500)
- Comprehensive API documentation via Swagger

### 2. Application Layer (WorldCup2026.Application)
**Responsibility:** Business logic and orchestration

**Components:**
- **Services:** Business logic implementation (TeamService, MatchService, GroupService, StadiumService, StandingService, DashboardService)
- **Interfaces:** Service contracts (ITeamService, IMatchService, IGroupService, IStadiumService, IStandingService, IDashboardService)
- **DTOs:** Data Transfer Objects for all entities (Team, Match, Group, Stadium, Standing, Dashboard)
  - Create/Update DTOs for mutations
  - Read DTOs for queries
  - PagedResult<T> for pagination
- **Validators:** FluentValidation rules for all DTOs
- **Mappings:** AutoMapper profiles (MappingProfile)

**Key Services:**
- **DashboardService:** Tournament statistics, top teams, upcoming/recent matches
- **MatchService:** Match CRUD, result updates, filtering by phase/status/date
- **TeamService:** Team management with pagination and search
- **StadiumService:** Stadium operations with location data
- **StandingService:** Group standings with automatic recalculation

**Key Principles:**
- Business rules enforcement
- Transaction management via UnitOfWork
- Service orchestration
- No direct database access
- Comprehensive validation

### 3. Domain Layer (WorldCup2026.Domain)
**Responsibility:** Core business entities and rules

**Components:**
- **Entities:** Domain models (Team, Match, Group, Stadium, Standing, MatchResult, BaseEntity)
- **Enums:** MatchStatus, MatchPhase, Confederation
- **Interfaces:** Repository contracts (IRepository, ITeamRepository, IMatchRepository, IGroupRepository, IStadiumRepository, IStandingRepository, IUnitOfWork)
- **Exceptions:** Domain-specific exceptions

**Key Entities:**
- **BaseEntity:** Base class with Id, CreatedAt, UpdatedAt
- **Team:** Represents national teams with confederation, FIFA ranking
- **Match:** Match details with home/away teams, stadium, phase, status
- **MatchResult:** Match outcomes including scores, penalties, highlights
- **Group:** Tournament groups (A-L)
- **Stadium:** Venue information with capacity, location coordinates, images
- **Standing:** Group stage standings with points, goals, position

**Key Principles:**
- Framework-independent
- Rich domain models
- Business invariants
- No external dependencies

### 4. Infrastructure Layer (WorldCup2026.Infrastructure)
**Responsibility:** External concerns and data access

**Components:**
- **Data/WorldCupDbContext:** EF Core context with all entity configurations
- **Data/Configurations:** Fluent API entity configurations
  - TeamConfiguration: Team entity with indexes on Code, Name, Confederation
  - MatchConfiguration: Match entity with complex relationships
  - GroupConfiguration: Group entity with unique name constraint
  - StadiumConfiguration: Stadium with geographic coordinates (Latitude/Longitude)
  - StandingConfiguration: Standing with composite unique index
- **Data/Migrations:** EF Core migrations (InitialCreate, AddFlagUrlToStadium, AddHighlightsToMatchResult)
- **Data/Seeding:** Comprehensive data seeding system
  - DataSeeder: Orchestrates all seeders
  - TeamSeeder: Seeds 48 teams with confederations
  - GroupSeeder: Seeds 12 groups (A-L)
  - StadiumSeeder: Seeds 16 stadiums across USA, Mexico, Canada
  - StandingSeeder: Initializes group standings
  - MatchSeeder: Code-based match generation
  - JsonMatchSeeder: JSON file-based match seeding with result support
- **Repositories:** Generic and specialized repositories
  - Repository<T>: Base generic repository
  - TeamRepository, MatchRepository, GroupRepository, StadiumRepository, StandingRepository
  - UnitOfWork: Transaction management

**Key Features:**
- SQLite database (development) / PostgreSQL (production ready)
- Entity configurations with proper indexes and constraints
- Flexible seeding system (code-based or JSON-based)
- Automatic standing recalculation after match results
- Geographic data support for stadiums
- Comprehensive relationship mapping

**Key Principles:**
- Database access abstraction
- Repository pattern implementation
- Unit of Work for transactions
- Configurable seeding strategies
- Migration-based schema management

## Frontend Architecture

### Component Hierarchy

```
App (React + TypeScript + Vite)
├── MainLayout
│   ├── Header with Navigation
│   └── Content Area
├── Routes (React Router)
│   ├── DashboardPage
│   │   ├── StatCards (Teams, Matches, Stadiums, Groups)
│   │   ├── Upcoming Matches Section
│   │   └── Tournament Information Cards
│   ├── TeamsPage
│   │   ├── TeamList with Pagination
│   │   └── TeamDetailsPage
│   ├── MatchesPage
│   │   ├── MatchList with Filters
│   │   └── MatchDetailsPage
│   └── (Future: Groups, Standings, Knockout)
└── Shared Components
    ├── PageHeader
    ├── StatCard
    ├── Loading
    └── ErrorMessage
```

### Implemented Features

**Pages:**
- **DashboardPage:** Tournament overview with statistics, upcoming matches, and tournament format information
- **TeamsPage:** Team listing with search and pagination
- **TeamDetailsPage:** Individual team information
- **MatchesPage:** Match listing with phase/status filtering
- **MatchDetailsPage:** Detailed match information

**Services (API Integration):**
- `dashboard.service.ts` - Dashboard statistics
- `teams.service.ts` - Team operations
- `matches.service.ts` - Match operations
- `groups.service.ts` - Group operations
- `stadiums.service.ts` - Stadium operations
- `standings.service.ts` - Standing operations

**Shared Components:**
- `PageHeader` - Consistent page titles and subtitles
- `StatCard` - Animated statistics cards with icons
- `Loading` - Loading spinner with messages
- `ErrorMessage` - User-friendly error display

### State Management Strategy

- **React Query (@tanstack/react-query):** Server state management with caching
- **React Router:** URL-based routing and navigation
- **Local State:** Component-specific state with useState
- **Material-UI (MUI):** Component library and theming

### Data Flow

```
User Action → Component → Service → API → Backend
                ↓                           ↓
            Local State              Database
                ↓                           ↓
            Re-render ← React Query ← Response
```

## Communication Patterns

### Frontend to Backend
- **Protocol:** HTTP/HTTPS
- **Format:** JSON
- **Authentication:** JWT (future implementation)
- **Error Handling:** Standardized error responses

### Backend to Database
- **ORM:** Entity Framework Core 8.0
- **Database:** SQLite (development), PostgreSQL-ready (production)
- **Migrations:** Code-first approach with 3 migrations
  - InitialCreate: Base schema
  - AddFlagUrlToStadium: Stadium flag images
  - AddHighlightsToMatchResult: Match highlights field
- **Connection Pooling:** Built-in EF Core
- **Transactions:** UnitOfWork pattern with automatic transaction management
- **Seeding:** Configurable (code-based or JSON-based)

## Scalability Considerations

### Horizontal Scaling
- Stateless API design
- Database connection pooling
- Load balancer ready

### Vertical Scaling
- Efficient queries with EF Core
- Caching strategies
- Async/await patterns

### Future Enhancements
- Redis for caching
- Message queue for async operations
- CDN for static assets
- Microservices separation

## Security Architecture

### API Security
- Input validation (FluentValidation)
- SQL injection prevention (EF Core)
- CORS configuration
- Rate limiting
- HTTPS enforcement

### Data Security
- Parameterized queries
- Environment variables for secrets
- Connection string encryption
- Audit logging

### Frontend Security
- XSS prevention (React default)
- CSRF protection
- Secure HTTP headers
- Input sanitization

## Deployment Architecture

### Development
```
Developer Machine
├── Frontend (Vite dev server)
├── Backend (.NET CLI)
└── PostgreSQL (Docker)
```

### Production (Docker Compose)
```
Docker Compose Setup
├── Frontend Container
│   ├── Nginx web server
│   ├── Optimized production build
│   └── Port 80
├── Backend Container
│   ├── .NET 8 Runtime
│   ├── ASP.NET Core API
│   └── Port 5000
└── Database
    ├── SQLite (current)
    └── PostgreSQL (production-ready)
```

**Docker Configuration:**
- Multi-stage builds for optimization
- Health checks for all services
- Volume mounts for data persistence
- Environment-based configuration
- Network isolation between services

## Design Patterns Used

### Backend
- **Repository Pattern:** Data access abstraction with generic and specialized repositories
- **Unit of Work Pattern:** Transaction management across repositories
- **Service Pattern:** Business logic encapsulation in application services
- **Dependency Injection:** Built-in .NET DI container for loose coupling
- **DTO Pattern:** Data transfer between layers with AutoMapper
- **Builder Pattern:** Entity configuration with Fluent API
- **Strategy Pattern:** Configurable seeding (code-based vs JSON-based)
- **Template Method:** Base repository with specialized implementations

### Frontend
- **Component Pattern:** Reusable UI components with Material-UI
- **Service Layer Pattern:** API abstraction with axios
- **Custom Hooks:** React Query for data fetching and caching
- **Composition Pattern:** Layout and page composition
- **Provider Pattern:** Theme and routing context

## Error Handling Strategy

### Backend
1. Domain exceptions → 400 Bad Request
2. Not found → 404 Not Found
3. Validation errors → 422 Unprocessable Entity
4. Server errors → 500 Internal Server Error
5. All errors logged with Serilog

### Frontend
1. Network errors → Retry with React Query
2. Validation errors → Form field errors
3. Server errors → Error boundary
4. User-friendly messages
5. Error logging to console (dev) / service (prod)

## Performance Optimization

### Backend
- Async/await for I/O operations
- Database indexing
- Query optimization
- Response caching
- Pagination for large datasets

### Frontend
- Code splitting (React.lazy)
- Image optimization
- Memoization (useMemo, useCallback)
- Virtual scrolling for large lists
- Debouncing for search inputs

## Monitoring and Logging

### Backend Logging
- Built-in .NET logging with ILogger
- Log levels: Debug, Info, Warning, Error
- Structured logging for all operations
- Request/response logging in controllers
- Database operation logging
- Seeding operation detailed logging

### Frontend Logging
- Console logging (development)
- Error boundaries for React error handling
- Network error logging via React Query
- User-friendly error messages

## Testing Strategy

### Backend Tests (MSTest + Moq)
- **Unit Tests:**
  - Services: TeamService, MatchService, GroupService, StadiumService, StandingService, DashboardService
  - Validators: CreateTeamDtoValidator, UpdateMatchResultDtoValidator, etc.
  - Domain: Team, Standing business logic
  - Mappings: AutoMapper profile validation
- **Integration Tests:**
  - Controllers: All 6 controllers with mock services
  - Repositories: TeamRepository, UnitOfWork with in-memory database
- **Test Coverage:** Comprehensive coverage across all layers
- **Test Projects:** WorldCup2026.Tests with organized test structure

### Frontend Tests (Vitest + React Testing Library)
- **Component Tests:**
  - ErrorMessage, Loading, StatCard components
- **Service Tests:**
  - teams.service, matches.service, stadiums.service
- **Test Utilities:**
  - Custom render with providers
  - Mock data generators
- **Configuration:** Vitest with jsdom environment

## Documentation

- **API Documentation:** Swagger/OpenAPI at `/swagger` endpoint
- **Code Documentation:**
  - XML comments on all public APIs (C#)
  - JSDoc comments for TypeScript services
  - Inline comments for complex logic
- **Architecture Documentation:**
  - This document (02-architecture.md)
  - Technology stack (01-technology-stack.md)
- **Project Documentation:**
  - README.md: Project overview and setup
  - DOCKER.md: Docker deployment guide
  - GIT_COMMANDS.md: Git workflow
  - Progreso.md: Development progress tracking
  - Informe.md: Project reports
- **Test Documentation:**
  - Test coverage reports
  - Test README files

## Key Features Implemented

### Backend Features
✅ Complete CRUD operations for all entities
✅ Advanced filtering and pagination
✅ Match result management with automatic standing updates
✅ Dashboard with comprehensive statistics
✅ Flexible data seeding (code-based and JSON-based)
✅ Geographic data support for stadiums
✅ Comprehensive validation with FluentValidation
✅ Full test coverage across all layers
✅ Swagger API documentation
✅ Docker support

### Frontend Features
✅ Dashboard with tournament overview
✅ Team listing and details
✅ Match listing with filters
✅ Responsive Material-UI design
✅ React Query for efficient data fetching
✅ Loading states and error handling
✅ Animated UI components
✅ Type-safe TypeScript implementation
✅ Component testing with Vitest

### Data Management
✅ 48 teams across 6 confederations
✅ 12 groups (A-L)
✅ 16 stadiums across USA, Mexico, Canada
✅ 104 matches (group stage + knockout)
✅ Automatic standings calculation
✅ Match results with highlights
✅ Geographic coordinates for stadiums

## Future Enhancements

### Planned Features
- [ ] Player management and statistics
- [ ] Individual player goal tracking
- [ ] Live match updates
- [ ] Knockout bracket visualization
- [ ] Advanced analytics and predictions
- [ ] User authentication and authorization
- [ ] Match commentary and timeline
- [ ] Social features (predictions, discussions)
- [ ] Mobile application
- [ ] Real-time notifications
- [ ] Multi-language support
- [ ] Advanced search and filtering
- [ ] Export functionality (PDF, Excel)
- [ ] Admin dashboard for data management