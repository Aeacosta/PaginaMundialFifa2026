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
                    │  PostgreSQL   │
                    └───────────────┘
```

## Backend Layers

### 1. Presentation Layer (WorldCup2026.API)
**Responsibility:** Handle HTTP requests and responses

**Components:**
- **Controllers:** API endpoints
- **DTOs:** Data Transfer Objects for request/response
- **Filters:** Exception filters, action filters
- **Middleware:** Error handling, logging, CORS
- **Program.cs:** Application configuration

**Key Principles:**
- Thin controllers (delegate to services)
- Input validation
- Response formatting
- HTTP status code management

### 2. Application Layer (WorldCup2026.Application)
**Responsibility:** Business logic and orchestration

**Components:**
- **Services:** Business logic implementation
- **Interfaces:** Service contracts
- **Validators:** FluentValidation rules
- **Mappings:** AutoMapper profiles
- **Common:** Shared utilities

**Key Principles:**
- Business rules enforcement
- Transaction management
- Service orchestration
- No direct database access

### 3. Domain Layer (WorldCup2026.Domain)
**Responsibility:** Core business entities and rules

**Components:**
- **Entities:** Domain models (Team, Match, Group, etc.)
- **Enums:** Match status, phases, etc.
- **Interfaces:** Repository contracts
- **Exceptions:** Domain-specific exceptions

**Key Principles:**
- Framework-independent
- Rich domain models
- Business invariants
- No external dependencies

### 4. Infrastructure Layer (WorldCup2026.Infrastructure)
**Responsibility:** External concerns and data access

**Components:**
- **Data/WorldCupDbContext:** EF Core context
- **Data/Configurations:** Entity configurations
- **Data/Migrations:** Database migrations
- **Repositories:** Data access implementation
- **Services:** External service integrations

**Key Principles:**
- Database access
- External API calls
- File system operations
- Infrastructure concerns

## Frontend Architecture

### Component Hierarchy

```
App
├── Layout
│   ├── Header
│   ├── Navigation
│   └── Footer
├── Routes
│   ├── Dashboard
│   ├── Teams
│   │   ├── TeamList
│   │   ├── TeamDetail
│   │   └── TeamForm
│   ├── Groups
│   │   ├── GroupList
│   │   ├── GroupDetail
│   │   └── Standings
│   ├── Matches
│   │   ├── MatchList
│   │   ├── MatchDetail
│   │   ├── Calendar
│   │   └── MatchForm
│   └── Knockout
│       └── Bracket
└── Common Components
    ├── Loading
    ├── ErrorBoundary
    ├── ConfirmDialog
    └── Notification
```

### State Management Strategy

- **React Query:** Server state (API data)
- **React Context:** Global UI state (theme, language)
- **Local State:** Component-specific state
- **URL State:** Filters, pagination

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
- **ORM:** Entity Framework Core
- **Migrations:** Code-first approach
- **Connection Pooling:** Built-in EF Core
- **Transactions:** Automatic for operations

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

### Production
```
Docker Compose
├── Frontend Container (Nginx)
├── Backend Container (.NET)
└── PostgreSQL Container
```

## Design Patterns Used

### Backend
- **Repository Pattern:** Data access abstraction
- **Service Pattern:** Business logic encapsulation
- **Dependency Injection:** Loose coupling
- **DTO Pattern:** Data transfer
- **Factory Pattern:** Object creation
- **Strategy Pattern:** Algorithm selection

### Frontend
- **Component Pattern:** UI composition
- **Container/Presenter:** Logic separation
- **Custom Hooks:** Logic reuse
- **Higher-Order Components:** Cross-cutting concerns
- **Render Props:** Component composition

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
- Serilog with structured logging
- Log levels: Debug, Info, Warning, Error
- Request/response logging
- Performance metrics

### Frontend Logging
- Console logging (development)
- Error tracking service (production)
- User action tracking
- Performance monitoring

## Testing Strategy

### Backend Tests
- **Unit Tests:** Services, validators
- **Integration Tests:** Controllers, repositories
- **Test Coverage:** Minimum 70%

### Frontend Tests
- **Unit Tests:** Utilities, hooks
- **Component Tests:** React Testing Library
- **E2E Tests:** Cypress (future)

## Documentation

- **API Documentation:** Swagger/OpenAPI
- **Code Documentation:** XML comments (C#), JSDoc (TypeScript)
- **Architecture Documentation:** This document
- **README:** Setup and deployment instructions