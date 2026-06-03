# API Testing Guide - FIFA World Cup 2026

## Overview
This document provides a comprehensive guide for testing all 60 API endpoints using Swagger UI.

## API Information
- **Base URL**: http://localhost:5004 or https://localhost:7004
- **Swagger UI**: Available at root URL (/)
- **API Version**: 1.0
- **OpenAPI**: 3.0

## Test Status
✅ **API is running and accessible**
✅ **Swagger UI is functional**
✅ **All 60 endpoints are documented**

## Controllers and Endpoints

### 1. Dashboard Controller (12 endpoints)
**Purpose**: Provides aggregated data and statistics for the dashboard

| Method | Endpoint | Description | Test Status |
|--------|----------|-------------|-------------|
| GET | `/api/Dashboard` | Get complete dashboard data | ⏳ Pending |
| GET | `/api/Dashboard/statistics` | Get tournament statistics | ⏳ Pending |
| GET | `/api/Dashboard/upcoming-matches` | Get upcoming matches (next 7 days) | ⏳ Pending |
| GET | `/api/Dashboard/recent-results` | Get recent match results | ⏳ Pending |
| GET | `/api/Dashboard/today-matches` | Get today's matches | ⏳ Pending |
| GET | `/api/Dashboard/top-scorers` | Get top scorers | ⏳ Pending |
| GET | `/api/Dashboard/top-teams/wins` | Get top teams by wins | ⏳ Pending |
| GET | `/api/Dashboard/top-teams/goal-difference` | Get top teams by goal difference | ⏳ Pending |
| GET | `/api/Dashboard/top-teams/points` | Get top teams by points | ⏳ Pending |
| GET | `/api/Dashboard/phase/{phase}/matches` | Get matches by phase | ⏳ Pending |
| GET | `/api/Dashboard/team/{teamId}/performance` | Get team performance | ⏳ Pending |
| GET | `/api/Dashboard/knockout-bracket` | Get knockout bracket | ⏳ Pending |

### 2. Groups Controller (9 endpoints)
**Purpose**: Manage tournament groups

| Method | Endpoint | Description | Test Status |
|--------|----------|-------------|-------------|
| GET | `/api/Groups` | Get all groups (paginated) | ⏳ Pending |
| GET | `/api/Groups/{id}` | Get group by ID | ⏳ Pending |
| GET | `/api/Groups/{id}/with-standings` | Get group with standings | ⏳ Pending |
| GET | `/api/Groups/name/{name}` | Get group by name | ⏳ Pending |
| GET | `/api/Groups/{id}/teams` | Get teams in group | ⏳ Pending |
| GET | `/api/Groups/{id}/matches` | Get matches in group | ⏳ Pending |
| POST | `/api/Groups` | Create new group | ⏳ Pending |
| PUT | `/api/Groups/{id}` | Update group | ⏳ Pending |
| DELETE | `/api/Groups/{id}` | Delete group | ⏳ Pending |

### 3. Matches Controller (13 endpoints)
**Purpose**: Manage matches and results

| Method | Endpoint | Description | Test Status |
|--------|----------|-------------|-------------|
| GET | `/api/Matches` | Get all matches (paginated) | ⏳ Pending |
| GET | `/api/Matches/{id}` | Get match by ID | ⏳ Pending |
| GET | `/api/Matches/team/{teamId}` | Get matches by team | ⏳ Pending |
| GET | `/api/Matches/stadium/{stadiumId}` | Get matches by stadium | ⏳ Pending |
| GET | `/api/Matches/phase/{phase}` | Get matches by phase | ⏳ Pending |
| GET | `/api/Matches/status/{status}` | Get matches by status | ⏳ Pending |
| GET | `/api/Matches/date-range` | Get matches by date range | ⏳ Pending |
| GET | `/api/Matches/upcoming` | Get upcoming matches | ⏳ Pending |
| GET | `/api/Matches/recent` | Get recent matches | ⏳ Pending |
| POST | `/api/Matches` | Create new match | ⏳ Pending |
| PUT | `/api/Matches/{id}` | Update match | ⏳ Pending |
| PUT | `/api/Matches/{id}/result` | Update match result | ⏳ Pending |
| DELETE | `/api/Matches/{id}` | Delete match | ⏳ Pending |

### 4. Stadiums Controller (9 endpoints)
**Purpose**: Manage stadiums

| Method | Endpoint | Description | Test Status |
|--------|----------|-------------|-------------|
| GET | `/api/Stadiums` | Get all stadiums (paginated) | ⏳ Pending |
| GET | `/api/Stadiums/{id}` | Get stadium by ID | ⏳ Pending |
| GET | `/api/Stadiums/city/{city}` | Get stadiums by city | ⏳ Pending |
| GET | `/api/Stadiums/country/{country}` | Get stadiums by country | ⏳ Pending |
| GET | `/api/Stadiums/search` | Search stadiums by name | ⏳ Pending |
| GET | `/api/Stadiums/{id}/matches` | Get matches at stadium | ⏳ Pending |
| POST | `/api/Stadiums` | Create new stadium | ⏳ Pending |
| PUT | `/api/Stadiums/{id}` | Update stadium | ⏳ Pending |
| DELETE | `/api/Stadiums/{id}` | Delete stadium | ⏳ Pending |

### 5. Standings Controller (8 endpoints)
**Purpose**: Manage group standings

| Method | Endpoint | Description | Test Status |
|--------|----------|-------------|-------------|
| GET | `/api/Standings` | Get all standings | ⏳ Pending |
| GET | `/api/Standings/{id}` | Get standing by ID | ⏳ Pending |
| GET | `/api/Standings/group/{groupId}` | Get standings by group | ⏳ Pending |
| GET | `/api/Standings/team/{teamId}` | Get standing by team | ⏳ Pending |
| POST | `/api/Standings` | Create new standing | ⏳ Pending |
| PUT | `/api/Standings/{id}` | Update standing | ⏳ Pending |
| POST | `/api/Standings/recalculate/{groupId}` | Recalculate group standings | ⏳ Pending |
| DELETE | `/api/Standings/{id}` | Delete standing | ⏳ Pending |

### 6. Teams Controller (9 endpoints)
**Purpose**: Manage teams

| Method | Endpoint | Description | Test Status |
|--------|----------|-------------|-------------|
| GET | `/api/Teams` | Get all teams (paginated) | ⏳ Pending |
| GET | `/api/Teams/{id}` | Get team by ID | ⏳ Pending |
| GET | `/api/Teams/code/{code}` | Get team by code | ⏳ Pending |
| GET | `/api/Teams/group/{groupId}` | Get teams by group | ⏳ Pending |
| GET | `/api/Teams/confederation/{confederation}` | Get teams by confederation | ⏳ Pending |
| GET | `/api/Teams/search` | Search teams by name | ⏳ Pending |
| POST | `/api/Teams` | Create new team | ⏳ Pending |
| PUT | `/api/Teams/{id}` | Update team | ⏳ Pending |
| DELETE | `/api/Teams/{id}` | Delete team | ⏳ Pending |

## Testing Workflow

### Phase 1: Basic CRUD Operations (Empty Database)
Since the database is empty, we need to create data first.

#### Step 1: Create Groups (POST /api/Groups)
Create 12 groups (A through L):
```json
{
  "name": "Group A",
  "description": "Group A - FIFA World Cup 2026"
}
```
Repeat for Groups B through L.

#### Step 2: Create Stadiums (POST /api/Stadiums)
Create stadiums in USA, Canada, and Mexico:
```json
{
  "name": "MetLife Stadium",
  "city": "East Rutherford",
  "country": "USA",
  "capacity": 82500,
  "imageUrl": "https://example.com/metlife.jpg",
  "latitude": "40.8128",
  "longitude": "-74.0742"
}
```

#### Step 3: Create Teams (POST /api/Teams)
Create 48 teams:
```json
{
  "name": "Argentina",
  "code": "ARG",
  "flagUrl": "https://example.com/flags/arg.png",
  "groupId": 1,
  "confederation": "CONMEBOL",
  "fifaRanking": 1
}
```

#### Step 4: Create Matches (POST /api/Matches)
Create matches for group stage:
```json
{
  "homeTeamId": 1,
  "awayTeamId": 2,
  "stadiumId": 1,
  "matchDate": "2026-06-11T20:00:00Z",
  "phase": "GroupStage",
  "round": 1,
  "groupId": 1,
  "status": "Scheduled"
}
```

### Phase 2: Read Operations
Test all GET endpoints:
1. Get all resources with pagination
2. Get by ID
3. Get by filters (group, team, stadium, etc.)
4. Search operations

### Phase 3: Update Operations
1. Update team information
2. Update match details
3. Update match results
4. Recalculate standings

### Phase 4: Dashboard and Statistics
Test all dashboard endpoints to verify aggregated data.

### Phase 5: Delete Operations
Test delete operations (be careful with foreign key constraints).

## Validation Testing

### Test Cases for Validation
1. **Required Fields**: Try creating entities without required fields
2. **String Length**: Test min/max length constraints
3. **Unique Constraints**: Try creating duplicate codes/names
4. **Foreign Keys**: Try invalid team/stadium/group IDs
5. **Date Validation**: Test past dates, invalid formats
6. **Enum Values**: Test invalid phase/status/confederation values
7. **Business Rules**: Test match result validation (penalties, winner)

## Expected Responses

### Success Responses
- **200 OK**: Successful GET, PUT
- **201 Created**: Successful POST
- **204 No Content**: Successful DELETE

### Error Responses
- **400 Bad Request**: Validation errors
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server errors

## Sample Test Data

### Confederations
- UEFA (Europe)
- CONMEBOL (South America)
- CONCACAF (North/Central America)
- CAF (Africa)
- AFC (Asia)
- OFC (Oceania)

### Match Phases
- GroupStage
- RoundOf32
- RoundOf16
- QuarterFinals
- SemiFinals
- ThirdPlace
- Final

### Match Status
- Scheduled
- InProgress
- Completed
- Postponed
- Cancelled

## Testing Notes

### Current Status
- ✅ API is running on port 5004
- ✅ Swagger UI is accessible
- ✅ Database is created with all tables
- ⚠️ Database is empty (no seed data)
- ⚠️ AutoMapper 12.0.1 has known vulnerability (needs upgrade)

### Recommendations
1. **Create seed data** before extensive testing
2. **Test in order**: Create → Read → Update → Delete
3. **Document issues** found during testing
4. **Test error scenarios** to verify validation
5. **Upgrade AutoMapper** to fix security vulnerability

## Next Steps
1. ✅ Phase 4.3 - API Testing (Current)
2. ⏳ Phase 5 - Database Seeding
3. ⏳ Phase 6-12 - Frontend Development

## Conclusion
The API is fully functional and ready for testing. All 60 endpoints are documented in Swagger UI. The next phase should focus on creating seed data to facilitate comprehensive testing.