# FIFA World Cup 2026 - Tracking Application

⚽ A comprehensive web application for tracking and managing the FIFA World Cup 2026 tournament.

## 🎯 Project Overview

This is a full-stack web application built to track the 2026 FIFA World Cup, featuring team management, match scheduling, live results, group standings, and knockout bracket visualization. The project demonstrates modern software development practices with clean architecture, RESTful API design, and responsive UI.

## 🏗️ Architecture

- **Backend:** .NET 10.0 Web API with Clean Architecture
- **Frontend:** React 18 with TypeScript and Material-UI
- **Database:** SQLite (Development) / PostgreSQL (Production)
- **Containerization:** Docker & Docker Compose

## 📚 Documentation

Comprehensive planning documentation is available in the [`.workbench/`](.workbench/) directory:

- **[Technology Stack](.workbench/01-technology-stack.md)** - Selected technologies and justifications
- **[Architecture](.workbench/02-architecture.md)** - System architecture and design patterns
- **[Folder Structure](.workbench/03-folder-structure.md)** - Complete project organization
- **[Database Model](.workbench/04-database-model.md)** - Entity relationships and schema
- **[API Endpoints](.workbench/05-api-endpoints.md)** - Complete REST API specification
- **[Implementation Plan](.workbench/06-implementation-plan.md)** - Step-by-step development roadmap
- **[Business Rules](.workbench/07-business-rules.md)** - Validation and business logic rules

## ✨ Key Features

### Core Functionality
- ✅ **Team Management** - CRUD operations for national teams
- ✅ **Group Stage** - 12 groups with 4 teams each
- ✅ **Match Scheduling** - Complete tournament calendar
- ✅ **Live Results** - Real-time score updates
- ✅ **Standings** - Automatic calculation and ranking
- ✅ **Knockout Bracket** - Interactive tournament bracket
- ✅ **Dashboard** - Tournament statistics and highlights
- ✅ **Stadium Information** - Venue details and locations

### Business Logic
- Automatic points calculation (Win: 3, Draw: 1, Loss: 0)
- Goal difference and standings sorting
- Penalty shootout support for knockout matches
- Winner advancement through bracket
- Data validation and integrity checks

## 🚀 Quick Start

### Prerequisites
- **.NET 10.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/10.0)
- **Node.js 18+** - [Download here](https://nodejs.org/)
- **Git** - [Download here](https://git-scm.com/)

### Step-by-Step Setup

#### 1️⃣ Clone the Repository
```bash
git clone <repository-url>
cd WorldCup2026
```

#### 2️⃣ Backend Setup

**Navigate to API directory:**
```bash
cd backend/src/WorldCup2026.API
```

**Restore dependencies:**
```bash
dotnet restore
```

**Create database schema:**
```bash
dotnet ef database update
```
This creates `WorldCup2026.db` (SQLite) with all tables.

**Start the backend API:**
```bash
dotnet run
```
✅ Backend running at: **http://localhost:5004**
✅ Swagger UI at: **http://localhost:5004/swagger**

#### 3️⃣ Seed the Database

**Open a NEW terminal** (keep backend running) and run:

**Option A - Using PowerShell:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5004/api/seed" -Method Post
```

**Option B - Using the batch file:**
```cmd
.\seed-database.bat
```

**Option C - Using Swagger UI:**
1. Open http://localhost:5004/swagger
2. Find `POST /api/seed` endpoint
3. Click "Try it out" → "Execute"

**What gets seeded:**
- ✅ 12 Groups (A-L)
- ✅ 16 Stadiums (USA, Mexico, Canada)
- ✅ 48 Teams (FIFA World Cup 2026 qualified teams)
- ✅ 48 Standings (initial records with zeros)

#### 4️⃣ Frontend Setup

**Open a NEW terminal** and navigate to frontend:
```bash
cd frontend
```

**Install dependencies:**
```bash
npm install
```

**Start the development server:**
```bash
npm run dev
```
✅ Frontend running at: **http://localhost:5173**

#### 5️⃣ Access the Application

Open your browser and navigate to:
- **Frontend:** http://localhost:5173
- **Backend API:** http://localhost:5004
- **Swagger UI:** http://localhost:5004/swagger

### 🎉 You're Ready!

The application should now display:
- Dashboard with statistics
- Teams list (48 teams)
- Groups list (12 groups)
- Stadiums list (16 stadiums)
- Standings table

---

## 🔧 Troubleshooting

### Backend Issues

**Problem:** `dotnet ef` command not found
```bash
# Install EF Core tools globally
dotnet tool install --global dotnet-ef
```

**Problem:** Database tables don't exist
```bash
# Re-run migrations
cd backend/src/WorldCup2026.API
dotnet ef database update
```

**Problem:** Port 5004 already in use
```bash
# Change port in launchSettings.json or kill the process
```

### Frontend Issues

**Problem:** `npm install` fails
```bash
# Clear cache and retry
npm cache clean --force
npm install
```

**Problem:** Port 5173 already in use
```bash
# Vite will automatically use next available port
# Or specify port: npm run dev -- --port 3000
```

### Database Issues

**Problem:** No data displaying in frontend
```bash
# 1. Verify backend is running (http://localhost:5004)
# 2. Seed the database using one of the methods above
# 3. Check browser console for errors (F12)
# 4. Verify API endpoints in Swagger
```

**Problem:** Need to reset database
```bash
# Delete database file and re-create
cd backend/src/WorldCup2026.API
Remove-Item WorldCup2026.db
dotnet ef database update
# Then seed again
```

---

## 🎮 Simulating Match Results

After seeding the database, you can simulate actual matches by updating match results through the API. This will automatically update the standings.

### Method 1: Using PowerShell

```powershell
# Update a match result (Match ID 1: Team 1 vs Team 2)
$body = @{
    homeTeamScore = 2
    awayTeamScore = 1
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5004/api/matches/1/result" `
    -Method PUT `
    -Body $body `
    -ContentType "application/json"
```

### Method 2: Using Swagger UI

1. Open **http://localhost:5004/swagger**
2. Find `PUT /api/matches/{id}/result` endpoint
3. Click **"Try it out"**
4. Enter:
   - **id:** Match ID (e.g., `1`)
   - **Request body:**
     ```json
     {
       "homeTeamScore": 2,
       "awayTeamScore": 1
     }
     ```
5. Click **"Execute"**

### Method 3: Using cURL

```bash
curl -X PUT "http://localhost:5004/api/matches/1/result" \
  -H "Content-Type: application/json" \
  -d '{
    "homeTeamScore": 2,
    "awayTeamScore": 1
  }'
```

### Method 4: Using Postman

1. Create a new **PUT** request
2. URL: `http://localhost:5004/api/matches/1/result`
3. Headers: `Content-Type: application/json`
4. Body (raw JSON):
   ```json
   {
     "homeTeamScore": 2,
     "awayTeamScore": 1
   }
   ```
5. Click **Send**

### Example: Simulating Multiple Matches

```powershell
# Match 1: USA 3-1 Mexico
Invoke-RestMethod -Uri "http://localhost:5004/api/matches/1/result" `
    -Method PUT `
    -Body '{"homeTeamScore":3,"awayTeamScore":1}' `
    -ContentType "application/json"

# Match 2: Canada 2-2 Costa Rica
Invoke-RestMethod -Uri "http://localhost:5004/api/matches/2/result" `
    -Method PUT `
    -Body '{"homeTeamScore":2,"awayTeamScore":2}' `
    -ContentType "application/json"

# Match 3: Brazil 4-0 Argentina
Invoke-RestMethod -Uri "http://localhost:5004/api/matches/3/result" `
    -Method PUT `
    -Body '{"homeTeamScore":4,"awayTeamScore":0}' `
    -ContentType "application/json"
```

### For Knockout Matches (with penalties)

```json
{
  "homeTeamScore": 1,
  "awayTeamScore": 1,
  "homeTeamPenalties": 4,
  "awayTeamPenalties": 3
}
```

### What Happens After Updating Results:

1. ✅ Match status changes to "Completed"
2. ✅ Standings are automatically recalculated
3. ✅ Points are awarded (Win: 3, Draw: 1, Loss: 0)
4. ✅ Goal difference is updated
5. ✅ Teams are re-ranked in their groups
6. ✅ Frontend displays updated standings immediately

### Verify Results:

- **View Match:** http://localhost:5004/api/matches/1
- **View Standings:** http://localhost:5004/api/standings/all
- **View Group Standings:** http://localhost:5004/api/standings/group/1
- **Frontend Standings:** http://localhost:5173/standings
- **Frontend Matches:** http://localhost:5173/matches

---

## 🐳 Docker Setup (Alternative)

**Coming soon** - Docker Compose configuration for easy deployment

## 📊 Tournament Structure

- **48 Teams** divided into **12 Groups** (A-L)
- **Group Stage:** 72 matches (3 per team)
- **Knockout Stage:** 32 matches (Round of 32 → Final)
- **Total Matches:** 104
- **Duration:** June 11 - July 19, 2026
- **Host Countries:** USA 🇺🇸, Mexico 🇲🇽, Canada 🇨🇦

## 🛠️ Technology Stack

### Backend
- **.NET 10.0** - Modern, high-performance framework
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core 9.0** - ORM for database access
- **SQLite** - Lightweight database (development)
- **FluentValidation 12.1.1** - Input validation
- **AutoMapper 12.0.1** - Object mapping
- **Swagger/OpenAPI** - API documentation

### Frontend
- **React 18** - UI library
- **TypeScript 5.6** - Type-safe JavaScript
- **Vite 6.0** - Fast build tool
- **Material-UI v5** - Component library
- **React Router v6** - Client-side routing
- **React Query (TanStack Query)** - Data fetching and caching
- **Axios** - HTTP client

### Testing
- **MSTest** - Backend unit testing
- **Moq 4.20.72** - Mocking framework
- **FluentAssertions 8.10.0** - Assertion library
- **Vitest** - Frontend testing (planned)
- **React Testing Library** - Component testing (planned)

## 📁 Project Structure

```
WorldCup2026/
├── .workbench/              # Planning documentation
├── backend/                 # .NET 8 Web API
│   ├── src/
│   │   ├── WorldCup2026.API/
│   │   ├── WorldCup2026.Application/
│   │   ├── WorldCup2026.Domain/
│   │   └── WorldCup2026.Infrastructure/
│   └── tests/
├── frontend/                # React application
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── services/
│   │   └── hooks/
│   └── public/
├── docker/                  # Docker configuration
├── docker-compose.yml
└── README.md
```

## 🧪 Testing

### Backend Unit Tests ✅

The backend has **178 comprehensive unit tests** covering all services and controllers with **100% pass rate**.

#### Quick Test Run
```bash
# From project root
cd backend/src/WorldCup2026.Tests
dotnet test
```

#### Detailed Test Run with Verbosity
```bash
cd backend/src/WorldCup2026.Tests
dotnet test --verbosity normal
```

#### Run Specific Test Class
```bash
# Run only TeamServiceTests
dotnet test --filter "FullyQualifiedName~TeamServiceTests"

# Run only GroupsControllerTests
dotnet test --filter "FullyQualifiedName~GroupsControllerTests"
```

#### Test Coverage Summary

**Service Tests (88 tests)**:
- `TeamServiceTests` - 10 tests (CRUD, validation, filters)
- `GroupServiceTests` - 15 tests (group management, team assignments)
- `StadiumServiceTests` - 16 tests (stadium CRUD, filters, scheduled matches)
- `MatchServiceTests` - 23 tests (match management, result updates, status transitions)
- `StandingServiceTests` - 13 tests (standings calculation, recalculation)
- `DashboardServiceTests` - 11 tests (statistics, top teams, scorers)

**Controller Tests (90 tests)**:
- `TeamsControllerTests` - 10 tests (HTTP endpoints, status codes)
- `GroupsControllerTests` - 17 tests (group endpoints with standings)
- `StadiumsControllerTests` - 17 tests (stadium endpoints with filters)
- `MatchesControllerTests` - 20 tests (match endpoints, result updates)
- `StandingsControllerTests` - 12 tests (standings endpoints, recalculation)
- `DashboardControllerTests` - 13 tests (dashboard statistics endpoints)

#### Test Results
```
Total tests: 178
     Passed: 178 (100%)
     Failed: 0
   Duration: ~811ms
```

#### Test Technologies
- **MSTest** - Testing framework
- **Moq 4.20.72** - Mocking framework for dependencies
- **FluentAssertions 8.10.0** - Readable assertion library
- **AAA Pattern** - Arrange-Act-Assert test structure

#### Example Test Output
```
Test run for WorldCup2026.Tests.dll (.NETCoreApp,Version=v10.0)
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:   178, Skipped:     0, Total:   178, Duration: 811 ms
```

### Frontend Tests ⏳

Frontend testing with Vitest and React Testing Library is planned for future implementation.

```bash
cd frontend
npm test  # Coming soon
```

## 📊 Current Implementation Status

### ✅ Completed (77% - 69.5/90 hours)
- ✅ **Backend API** - 100% complete
  - 6 controllers with 60 endpoints
  - Clean Architecture implementation
  - Repository and Service patterns
  - FluentValidation for all DTOs
  - Swagger documentation
  
- ✅ **Frontend** - 100% complete
  - 11 pages (Dashboard, Teams, Groups, Matches, Stadiums, Standings)
  - Responsive design with Material-UI
  - React Query for data fetching
  - TypeScript type safety
  - Search and filter functionality
  
- ✅ **Database** - 100% complete
  - EF Core migrations
  - 6 entity types with relationships
  - Seeding with real FIFA World Cup 2026 data

- ✅ **Backend Unit Tests** - 100% complete
  - 178 comprehensive tests (88 service + 90 controller)
  - 100% pass rate
  - MSTest with Moq and FluentAssertions
  - Full coverage of business logic and API endpoints

### ⏳ Pending (23% - 20.5 hours)
- ⏳ Frontend unit tests (8 hours)
- ⏳ Docker & Deployment (6 hours)
- ⏳ Documentation (4 hours)
- ⏳ Final polish (2.5 hours)

## 📖 API Documentation

Once the backend is running, access the interactive API documentation:
- **Swagger UI:** http://localhost:5004/swagger
- **OpenAPI Spec:** http://localhost:5004/swagger/v1/swagger.json

## 🎓 Learning Outcomes

This project demonstrates:
- Clean Architecture principles
- Repository and Service patterns
- RESTful API design
- Entity Framework Core
- React with TypeScript
- Material-UI components
- Docker containerization
- Automated testing
- Comprehensive documentation

## 🔮 Future Enhancements

- User authentication and authorization
- Match predictions and leaderboards
- Player statistics and analytics
- Real-time updates with SignalR
- Mobile application
- Multi-language support
- Social features (comments, sharing)
- News and articles integration

## 📝 Development Status

- ✅ **Planning Phase:** Complete (100%)
- ✅ **Backend Implementation:** Complete (100%)
- ✅ **Frontend Implementation:** Complete (100%)
- ✅ **Database Setup:** Complete (100%)
- ✅ **Backend Testing:** Complete (100% - 178 tests passing)
- ⏳ **Frontend Testing:** Pending (0%)
- ⏳ **Deployment Phase:** Pending (0%)

## 🤝 Contributing

This is a learning project. Contributions, issues, and feature requests are welcome!

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is open source and available for educational purposes.

## 🙏 Acknowledgments

- FIFA for World Cup inspiration
- Open source community for amazing tools
- All contributors and learners

## 📞 Contact

For questions or feedback, please open an issue in the repository.

---

## 📞 Support

For issues or questions:
1. Check the [Progress Report](.workbench/08-progress-report.md)
2. Review [API Documentation](.workbench/05-api-endpoints.md)
3. Open an issue in the repository

---

**Built with ❤️ for the love of football and software development** ⚽🏆

**Ready to track the World Cup 2026!** 🎉

**Last Updated:** June 6, 2026