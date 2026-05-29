# Folder Structure - FIFA World Cup 2026 Application

## Complete Project Structure

```
WorldCup2026/
├── .workbench/                          # Planning documentation
│   ├── 01-technology-stack.md
│   ├── 02-architecture.md
│   ├── 03-folder-structure.md
│   ├── 04-database-model.md
│   ├── 05-api-endpoints.md
│   └── 06-implementation-plan.md
│
├── backend/                             # Backend .NET solution
│   ├── src/
│   │   ├── WorldCup2026.API/           # Presentation Layer
│   │   │   ├── Controllers/
│   │   │   │   ├── TeamsController.cs
│   │   │   │   ├── GroupsController.cs
│   │   │   │   ├── MatchesController.cs
│   │   │   │   ├── StadiumsController.cs
│   │   │   │   ├── DashboardController.cs
│   │   │   │   └── KnockoutController.cs
│   │   │   ├── DTOs/
│   │   │   │   ├── Team/
│   │   │   │   │   ├── TeamDto.cs
│   │   │   │   │   ├── CreateTeamDto.cs
│   │   │   │   │   └── UpdateTeamDto.cs
│   │   │   │   ├── Match/
│   │   │   │   │   ├── MatchDto.cs
│   │   │   │   │   ├── CreateMatchDto.cs
│   │   │   │   │   ├── UpdateMatchDto.cs
│   │   │   │   │   └── MatchResultDto.cs
│   │   │   │   ├── Group/
│   │   │   │   │   ├── GroupDto.cs
│   │   │   │   │   └── StandingDto.cs
│   │   │   │   ├── Stadium/
│   │   │   │   │   ├── StadiumDto.cs
│   │   │   │   │   └── CreateStadiumDto.cs
│   │   │   │   └── Dashboard/
│   │   │   │       ├── DashboardDto.cs
│   │   │   │       └── StatsDto.cs
│   │   │   ├── Filters/
│   │   │   │   ├── ExceptionFilter.cs
│   │   │   │   └── ValidationFilter.cs
│   │   │   ├── Middleware/
│   │   │   │   ├── ErrorHandlingMiddleware.cs
│   │   │   │   └── RequestLoggingMiddleware.cs
│   │   │   ├── Extensions/
│   │   │   │   ├── ServiceCollectionExtensions.cs
│   │   │   │   └── ApplicationBuilderExtensions.cs
│   │   │   ├── Program.cs
│   │   │   ├── appsettings.json
│   │   │   ├── appsettings.Development.json
│   │   │   └── WorldCup2026.API.csproj
│   │   │
│   │   ├── WorldCup2026.Application/   # Application Layer
│   │   │   ├── Services/
│   │   │   │   ├── TeamService.cs
│   │   │   │   ├── GroupService.cs
│   │   │   │   ├── MatchService.cs
│   │   │   │   ├── StandingService.cs
│   │   │   │   ├── StadiumService.cs
│   │   │   │   └── DashboardService.cs
│   │   │   ├── Interfaces/
│   │   │   │   ├── ITeamService.cs
│   │   │   │   ├── IGroupService.cs
│   │   │   │   ├── IMatchService.cs
│   │   │   │   ├── IStandingService.cs
│   │   │   │   ├── IStadiumService.cs
│   │   │   │   └── IDashboardService.cs
│   │   │   ├── Validators/
│   │   │   │   ├── CreateTeamDtoValidator.cs
│   │   │   │   ├── CreateMatchDtoValidator.cs
│   │   │   │   ├── MatchResultDtoValidator.cs
│   │   │   │   └── UpdateMatchDtoValidator.cs
│   │   │   ├── Mappings/
│   │   │   │   ├── TeamProfile.cs
│   │   │   │   ├── MatchProfile.cs
│   │   │   │   ├── GroupProfile.cs
│   │   │   │   └── StadiumProfile.cs
│   │   │   ├── Common/
│   │   │   │   ├── Result.cs
│   │   │   │   ├── PagedResult.cs
│   │   │   │   └── Constants.cs
│   │   │   └── WorldCup2026.Application.csproj
│   │   │
│   │   ├── WorldCup2026.Domain/        # Domain Layer
│   │   │   ├── Entities/
│   │   │   │   ├── Team.cs
│   │   │   │   ├── Group.cs
│   │   │   │   ├── Match.cs
│   │   │   │   ├── MatchResult.cs
│   │   │   │   ├── Standing.cs
│   │   │   │   ├── Stadium.cs
│   │   │   │   └── BaseEntity.cs
│   │   │   ├── Enums/
│   │   │   │   ├── MatchStatus.cs
│   │   │   │   ├── MatchPhase.cs
│   │   │   │   └── Confederation.cs
│   │   │   ├── Interfaces/
│   │   │   │   ├── IRepository.cs
│   │   │   │   ├── ITeamRepository.cs
│   │   │   │   ├── IGroupRepository.cs
│   │   │   │   ├── IMatchRepository.cs
│   │   │   │   ├── IStandingRepository.cs
│   │   │   │   ├── IStadiumRepository.cs
│   │   │   │   └── IUnitOfWork.cs
│   │   │   ├── Exceptions/
│   │   │   │   ├── DomainException.cs
│   │   │   │   ├── NotFoundException.cs
│   │   │   │   ├── ValidationException.cs
│   │   │   │   └── BusinessRuleException.cs
│   │   │   └── WorldCup2026.Domain.csproj
│   │   │
│   │   └── WorldCup2026.Infrastructure/ # Infrastructure Layer
│   │       ├── Data/
│   │       │   ├── WorldCupDbContext.cs
│   │       │   ├── Configurations/
│   │       │   │   ├── TeamConfiguration.cs
│   │       │   │   ├── GroupConfiguration.cs
│   │       │   │   ├── MatchConfiguration.cs
│   │       │   │   ├── MatchResultConfiguration.cs
│   │       │   │   ├── StandingConfiguration.cs
│   │       │   │   └── StadiumConfiguration.cs
│   │       │   ├── Migrations/
│   │       │   │   └── (auto-generated)
│   │       │   └── Seed/
│   │       │       ├── DataSeeder.cs
│   │       │       ├── TeamSeeder.cs
│   │       │       ├── GroupSeeder.cs
│   │       │       ├── StadiumSeeder.cs
│   │       │       └── MatchSeeder.cs
│   │       ├── Repositories/
│   │       │   ├── Repository.cs
│   │       │   ├── TeamRepository.cs
│   │       │   ├── GroupRepository.cs
│   │       │   ├── MatchRepository.cs
│   │       │   ├── StandingRepository.cs
│   │       │   ├── StadiumRepository.cs
│   │       │   └── UnitOfWork.cs
│   │       ├── Services/
│   │       │   └── (external service integrations)
│   │       └── WorldCup2026.Infrastructure.csproj
│   │
│   ├── tests/
│   │   ├── WorldCup2026.UnitTests/
│   │   │   ├── Services/
│   │   │   │   ├── TeamServiceTests.cs
│   │   │   │   ├── MatchServiceTests.cs
│   │   │   │   └── StandingServiceTests.cs
│   │   │   ├── Validators/
│   │   │   │   └── CreateTeamDtoValidatorTests.cs
│   │   │   └── WorldCup2026.UnitTests.csproj
│   │   │
│   │   └── WorldCup2026.IntegrationTests/
│   │       ├── Controllers/
│   │       │   ├── TeamsControllerTests.cs
│   │       │   └── MatchesControllerTests.cs
│   │       ├── Fixtures/
│   │       │   └── WebApplicationFactory.cs
│   │       └── WorldCup2026.IntegrationTests.csproj
│   │
│   ├── WorldCup2026.sln
│   ├── .gitignore
│   ├── Directory.Build.props
│   └── README.md
│
├── frontend/                            # Frontend React application
│   ├── public/
│   │   ├── assets/
│   │   │   ├── images/
│   │   │   │   ├── logo.svg
│   │   │   │   └── flags/
│   │   │   └── icons/
│   │   ├── favicon.ico
│   │   └── index.html
│   │
│   ├── src/
│   │   ├── components/                 # Reusable components
│   │   │   ├── common/
│   │   │   │   ├── Header.tsx
│   │   │   │   ├── Footer.tsx
│   │   │   │   ├── Navigation.tsx
│   │   │   │   ├── Loading.tsx
│   │   │   │   ├── ErrorBoundary.tsx
│   │   │   │   ├── ConfirmDialog.tsx
│   │   │   │   ├── Notification.tsx
│   │   │   │   └── PageHeader.tsx
│   │   │   ├── teams/
│   │   │   │   ├── TeamCard.tsx
│   │   │   │   ├── TeamList.tsx
│   │   │   │   ├── TeamForm.tsx
│   │   │   │   └── TeamStats.tsx
│   │   │   ├── matches/
│   │   │   │   ├── MatchCard.tsx
│   │   │   │   ├── MatchList.tsx
│   │   │   │   ├── MatchForm.tsx
│   │   │   │   ├── MatchResult.tsx
│   │   │   │   └── MatchStatus.tsx
│   │   │   ├── groups/
│   │   │   │   ├── GroupCard.tsx
│   │   │   │   ├── GroupList.tsx
│   │   │   │   ├── StandingsTable.tsx
│   │   │   │   └── GroupMatches.tsx
│   │   │   └── knockout/
│   │   │       ├── BracketView.tsx
│   │   │       ├── BracketMatch.tsx
│   │   │       └── RoundSelector.tsx
│   │   │
│   │   ├── pages/                      # Page components
│   │   │   ├── Dashboard.tsx
│   │   │   ├── Teams/
│   │   │   │   ├── TeamListPage.tsx
│   │   │   │   ├── TeamDetailPage.tsx
│   │   │   │   ├── CreateTeamPage.tsx
│   │   │   │   └── EditTeamPage.tsx
│   │   │   ├── Groups/
│   │   │   │   ├── GroupListPage.tsx
│   │   │   │   ├── GroupDetailPage.tsx
│   │   │   │   └── StandingsPage.tsx
│   │   │   ├── Matches/
│   │   │   │   ├── MatchListPage.tsx
│   │   │   │   ├── MatchDetailPage.tsx
│   │   │   │   ├── CreateMatchPage.tsx
│   │   │   │   ├── EditMatchPage.tsx
│   │   │   │   └── CalendarPage.tsx
│   │   │   ├── Knockout/
│   │   │   │   └── KnockoutPage.tsx
│   │   │   └── NotFound.tsx
│   │   │
│   │   ├── services/                   # API services
│   │   │   ├── api.ts
│   │   │   ├── teamService.ts
│   │   │   ├── matchService.ts
│   │   │   ├── groupService.ts
│   │   │   ├── stadiumService.ts
│   │   │   └── dashboardService.ts
│   │   │
│   │   ├── hooks/                      # Custom hooks
│   │   │   ├── useTeams.ts
│   │   │   ├── useTeam.ts
│   │   │   ├── useMatches.ts
│   │   │   ├── useMatch.ts
│   │   │   ├── useGroups.ts
│   │   │   ├── useStandings.ts
│   │   │   ├── useStadiums.ts
│   │   │   └── useDashboard.ts
│   │   │
│   │   ├── types/                      # TypeScript types
│   │   │   ├── team.ts
│   │   │   ├── match.ts
│   │   │   ├── group.ts
│   │   │   ├── standing.ts
│   │   │   ├── stadium.ts
│   │   │   ├── dashboard.ts
│   │   │   └── common.ts
│   │   │
│   │   ├── utils/                      # Utility functions
│   │   │   ├── dateUtils.ts
│   │   │   ├── formatters.ts
│   │   │   ├── validators.ts
│   │   │   └── constants.ts
│   │   │
│   │   ├── theme/                      # MUI theme
│   │   │   ├── theme.ts
│   │   │   ├── colors.ts
│   │   │   └── typography.ts
│   │   │
│   │   ├── routes/                     # Route configuration
│   │   │   └── AppRoutes.tsx
│   │   │
│   │   ├── contexts/                   # React contexts
│   │   │   └── NotificationContext.tsx
│   │   │
│   │   ├── App.tsx
│   │   ├── main.tsx
│   │   └── vite-env.d.ts
│   │
│   ├── tests/
│   │   ├── components/
│   │   ├── hooks/
│   │   └── utils/
│   │
│   ├── .env.example
│   ├── .env.development
│   ├── .gitignore
│   ├── package.json
│   ├── tsconfig.json
│   ├── tsconfig.node.json
│   ├── vite.config.ts
│   ├── vitest.config.ts
│   └── README.md
│
├── docker/                              # Docker configuration
│   ├── backend/
│   │   ├── Dockerfile
│   │   └── .dockerignore
│   ├── frontend/
│   │   ├── Dockerfile
│   │   ├── nginx.conf
│   │   └── .dockerignore
│   └── postgres/
│       └── init.sql
│
├── docker-compose.yml
├── docker-compose.dev.yml
├── .gitignore
└── README.md
```

## Key Directory Explanations

### Backend Structure

#### **src/WorldCup2026.API** (Presentation Layer)
- Entry point for HTTP requests
- Controllers handle routing and HTTP concerns
- DTOs define request/response contracts
- Filters and middleware for cross-cutting concerns

#### **src/WorldCup2026.Application** (Application Layer)
- Business logic implementation
- Service interfaces and implementations
- Input validation with FluentValidation
- AutoMapper profiles for object mapping

#### **src/WorldCup2026.Domain** (Domain Layer)
- Core business entities
- Domain interfaces (repository contracts)
- Business exceptions
- Enums for domain concepts

#### **src/WorldCup2026.Infrastructure** (Infrastructure Layer)
- Database context and configurations
- Repository implementations
- Data seeding
- External service integrations

### Frontend Structure

#### **src/components**
- Reusable UI components organized by feature
- Common components shared across features
- Each component has its own file

#### **src/pages**
- Top-level page components
- Organized by feature area
- Each page corresponds to a route

#### **src/services**
- API communication layer
- Axios configuration
- Service methods for each entity

#### **src/hooks**
- Custom React hooks
- React Query hooks for data fetching
- Reusable logic extraction

#### **src/types**
- TypeScript type definitions
- Interfaces for API responses
- Shared types across components

## Naming Conventions

### Backend (C#)
- **Files:** PascalCase (e.g., `TeamService.cs`)
- **Classes:** PascalCase (e.g., `TeamService`)
- **Interfaces:** IPascalCase (e.g., `ITeamService`)
- **Methods:** PascalCase (e.g., `GetTeamById`)
- **Properties:** PascalCase (e.g., `TeamName`)
- **Private fields:** _camelCase (e.g., `_repository`)

### Frontend (TypeScript/React)
- **Files:** PascalCase for components (e.g., `TeamCard.tsx`)
- **Files:** camelCase for utilities (e.g., `dateUtils.ts`)
- **Components:** PascalCase (e.g., `TeamCard`)
- **Functions:** camelCase (e.g., `formatDate`)
- **Variables:** camelCase (e.g., `teamList`)
- **Constants:** UPPER_SNAKE_CASE (e.g., `API_BASE_URL`)
- **Types/Interfaces:** PascalCase (e.g., `Team`, `MatchStatus`)

## File Organization Principles

1. **Separation of Concerns:** Each layer has a specific responsibility
2. **Feature-based Organization:** Group related files together
3. **Scalability:** Easy to add new features without restructuring
4. **Testability:** Clear boundaries make testing easier
5. **Maintainability:** Consistent structure across the project
6. **Discoverability:** Intuitive file locations

## Configuration Files

### Backend
- `appsettings.json` - Application configuration
- `appsettings.Development.json` - Development overrides
- `.csproj` files - Project dependencies
- `Directory.Build.props` - Shared build properties

### Frontend
- `package.json` - Dependencies and scripts
- `tsconfig.json` - TypeScript configuration
- `vite.config.ts` - Vite build configuration
- `vitest.config.ts` - Test configuration
- `.env` files - Environment variables

### Docker
- `Dockerfile` - Container image definition
- `docker-compose.yml` - Multi-container orchestration
- `.dockerignore` - Files to exclude from image