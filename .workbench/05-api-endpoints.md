# REST API Endpoints - FIFA World Cup 2026 Application

## Base URL
- **Development:** `http://localhost:5000/api`
- **Production:** `https://api.worldcup2026.com/api`

## API Versioning
- Version 1: `/api/v1/...` (optional, can be added later)
- Current: `/api/...` (default to v1)

## Common Response Formats

### Success Response
```json
{
  "data": { ... },
  "message": "Success message",
  "timestamp": "2026-06-15T10:30:00Z"
}
```

### Error Response
```json
{
  "error": {
    "code": "ERROR_CODE",
    "message": "Human-readable error message",
    "details": ["Additional error details"],
    "timestamp": "2026-06-15T10:30:00Z"
  }
}
```

### Paginated Response
```json
{
  "data": [...],
  "pagination": {
    "page": 1,
    "pageSize": 20,
    "totalPages": 5,
    "totalItems": 100
  }
}
```

---

## 1. Teams Endpoints

### GET /api/teams
Get all teams with optional filtering.

**Query Parameters:**
- `groupId` (int, optional) - Filter by group
- `confederation` (string, optional) - Filter by confederation
- `page` (int, optional, default: 1) - Page number
- `pageSize` (int, optional, default: 20) - Items per page

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "name": "Argentina",
      "code": "ARG",
      "flagUrl": "https://example.com/flags/arg.png",
      "groupId": 1,
      "groupName": "Group A",
      "confederation": "CONMEBOL",
      "fifaRanking": 1,
      "createdAt": "2026-01-01T00:00:00Z",
      "updatedAt": "2026-01-01T00:00:00Z"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 20,
    "totalPages": 3,
    "totalItems": 48
  }
}
```

---

### GET /api/teams/{id}
Get a specific team by ID.

**Path Parameters:**
- `id` (int, required) - Team ID

**Response:** `200 OK`
```json
{
  "data": {
    "id": 1,
    "name": "Argentina",
    "code": "ARG",
    "flagUrl": "https://example.com/flags/arg.png",
    "groupId": 1,
    "groupName": "Group A",
    "confederation": "CONMEBOL",
    "fifaRanking": 1,
    "standing": {
      "matchesPlayed": 3,
      "wins": 2,
      "draws": 1,
      "losses": 0,
      "goalsFor": 7,
      "goalsAgainst": 2,
      "goalDifference": 5,
      "points": 7
    },
    "createdAt": "2026-01-01T00:00:00Z",
    "updatedAt": "2026-01-01T00:00:00Z"
  }
}
```

**Error Responses:**
- `404 Not Found` - Team not found

---

### GET /api/teams/group/{groupId}
Get all teams in a specific group.

**Path Parameters:**
- `groupId` (int, required) - Group ID

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "name": "Argentina",
      "code": "ARG",
      "flagUrl": "https://example.com/flags/arg.png",
      "groupId": 1,
      "confederation": "CONMEBOL",
      "fifaRanking": 1
    }
  ]
}
```

---

### POST /api/teams
Create a new team.

**Request Body:**
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

**Response:** `201 Created`
```json
{
  "data": {
    "id": 1,
    "name": "Argentina",
    "code": "ARG",
    "flagUrl": "https://example.com/flags/arg.png",
    "groupId": 1,
    "confederation": "CONMEBOL",
    "fifaRanking": 1,
    "createdAt": "2026-01-01T00:00:00Z",
    "updatedAt": "2026-01-01T00:00:00Z"
  }
}
```

**Error Responses:**
- `400 Bad Request` - Invalid input
- `422 Unprocessable Entity` - Validation errors

---

### PUT /api/teams/{id}
Update an existing team.

**Path Parameters:**
- `id` (int, required) - Team ID

**Request Body:**
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

**Response:** `200 OK`
```json
{
  "data": {
    "id": 1,
    "name": "Argentina",
    "code": "ARG",
    "flagUrl": "https://example.com/flags/arg.png",
    "groupId": 1,
    "confederation": "CONMEBOL",
    "fifaRanking": 1,
    "updatedAt": "2026-01-02T00:00:00Z"
  }
}
```

**Error Responses:**
- `404 Not Found` - Team not found
- `422 Unprocessable Entity` - Validation errors

---

### DELETE /api/teams/{id}
Delete a team.

**Path Parameters:**
- `id` (int, required) - Team ID

**Response:** `204 No Content`

**Error Responses:**
- `404 Not Found` - Team not found
- `409 Conflict` - Team has associated matches

---

## 2. Groups Endpoints

### GET /api/groups
Get all groups.

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "name": "Group A",
      "description": "Group A of the 2026 World Cup",
      "teamCount": 4,
      "createdAt": "2026-01-01T00:00:00Z",
      "updatedAt": "2026-01-01T00:00:00Z"
    }
  ]
}
```

---

### GET /api/groups/{id}
Get a specific group by ID.

**Path Parameters:**
- `id` (int, required) - Group ID

**Response:** `200 OK`
```json
{
  "data": {
    "id": 1,
    "name": "Group A",
    "description": "Group A of the 2026 World Cup",
    "teams": [
      {
        "id": 1,
        "name": "Argentina",
        "code": "ARG",
        "flagUrl": "https://example.com/flags/arg.png"
      }
    ],
    "createdAt": "2026-01-01T00:00:00Z",
    "updatedAt": "2026-01-01T00:00:00Z"
  }
}
```

---

### GET /api/groups/{id}/teams
Get all teams in a group.

**Path Parameters:**
- `id` (int, required) - Group ID

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "name": "Argentina",
      "code": "ARG",
      "flagUrl": "https://example.com/flags/arg.png",
      "confederation": "CONMEBOL",
      "fifaRanking": 1
    }
  ]
}
```

---

### GET /api/groups/{id}/standings
Get standings for a group.

**Path Parameters:**
- `id` (int, required) - Group ID

**Response:** `200 OK`
```json
{
  "data": [
    {
      "position": 1,
      "team": {
        "id": 1,
        "name": "Argentina",
        "code": "ARG",
        "flagUrl": "https://example.com/flags/arg.png"
      },
      "matchesPlayed": 3,
      "wins": 2,
      "draws": 1,
      "losses": 0,
      "goalsFor": 7,
      "goalsAgainst": 2,
      "goalDifference": 5,
      "points": 7
    }
  ]
}
```

---

### GET /api/groups/{id}/matches
Get all matches in a group.

**Path Parameters:**
- `id` (int, required) - Group ID

**Query Parameters:**
- `round` (int, optional) - Filter by round (1, 2, or 3)

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "homeTeam": {
        "id": 1,
        "name": "Argentina",
        "code": "ARG",
        "flagUrl": "https://example.com/flags/arg.png"
      },
      "awayTeam": {
        "id": 2,
        "name": "Brazil",
        "code": "BRA",
        "flagUrl": "https://example.com/flags/bra.png"
      },
      "stadium": {
        "id": 1,
        "name": "MetLife Stadium",
        "city": "East Rutherford"
      },
      "matchDate": "2026-06-15T20:00:00Z",
      "phase": "GroupStage",
      "round": 1,
      "status": "Scheduled"
    }
  ]
}
```

---

### POST /api/groups
Create a new group.

**Request Body:**
```json
{
  "name": "Group A",
  "description": "Group A of the 2026 World Cup"
}
```

**Response:** `201 Created`

---

## 3. Matches Endpoints

### GET /api/matches
Get all matches with filtering and pagination.

**Query Parameters:**
- `teamId` (int, optional) - Filter by team
- `groupId` (int, optional) - Filter by group
- `stadiumId` (int, optional) - Filter by stadium
- `phase` (string, optional) - Filter by phase
- `status` (string, optional) - Filter by status
- `dateFrom` (date, optional) - Filter from date
- `dateTo` (date, optional) - Filter to date
- `page` (int, optional, default: 1)
- `pageSize` (int, optional, default: 20)

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "homeTeam": {
        "id": 1,
        "name": "Argentina",
        "code": "ARG",
        "flagUrl": "https://example.com/flags/arg.png"
      },
      "awayTeam": {
        "id": 2,
        "name": "Brazil",
        "code": "BRA",
        "flagUrl": "https://example.com/flags/bra.png"
      },
      "stadium": {
        "id": 1,
        "name": "MetLife Stadium",
        "city": "East Rutherford",
        "country": "United States"
      },
      "matchDate": "2026-06-15T20:00:00Z",
      "phase": "GroupStage",
      "round": 1,
      "groupId": 1,
      "status": "Finished",
      "result": {
        "homeTeamScore": 2,
        "awayTeamScore": 1,
        "winnerTeamId": 1
      }
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 20,
    "totalPages": 6,
    "totalItems": 104
  }
}
```

---

### GET /api/matches/{id}
Get a specific match by ID.

**Path Parameters:**
- `id` (int, required) - Match ID

**Response:** `200 OK`
```json
{
  "data": {
    "id": 1,
    "homeTeam": {
      "id": 1,
      "name": "Argentina",
      "code": "ARG",
      "flagUrl": "https://example.com/flags/arg.png"
    },
    "awayTeam": {
      "id": 2,
      "name": "Brazil",
      "code": "BRA",
      "flagUrl": "https://example.com/flags/bra.png"
    },
    "stadium": {
      "id": 1,
      "name": "MetLife Stadium",
      "city": "East Rutherford",
      "country": "United States",
      "capacity": 82500
    },
    "matchDate": "2026-06-15T20:00:00Z",
    "phase": "GroupStage",
    "round": 1,
    "groupId": 1,
    "groupName": "Group A",
    "status": "Finished",
    "result": {
      "homeTeamScore": 2,
      "awayTeamScore": 1,
      "winnerTeamId": 1,
      "winnerTeamName": "Argentina"
    },
    "createdAt": "2026-01-01T00:00:00Z",
    "updatedAt": "2026-06-15T22:00:00Z"
  }
}
```

---

### GET /api/matches/upcoming
Get upcoming matches.

**Query Parameters:**
- `limit` (int, optional, default: 10) - Number of matches to return
- `days` (int, optional, default: 7) - Look ahead days

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "homeTeam": { "id": 1, "name": "Argentina", "code": "ARG" },
      "awayTeam": { "id": 2, "name": "Brazil", "code": "BRA" },
      "stadium": { "id": 1, "name": "MetLife Stadium", "city": "East Rutherford" },
      "matchDate": "2026-06-15T20:00:00Z",
      "phase": "GroupStage",
      "status": "Scheduled"
    }
  ]
}
```

---

### GET /api/matches/recent
Get recent matches.

**Query Parameters:**
- `limit` (int, optional, default: 10) - Number of matches to return
- `days` (int, optional, default: 7) - Look back days

**Response:** `200 OK`

---

### GET /api/matches/date/{date}
Get matches on a specific date.

**Path Parameters:**
- `date` (date, required) - Date in format YYYY-MM-DD

**Response:** `200 OK`

---

### GET /api/matches/team/{teamId}
Get all matches for a specific team.

**Path Parameters:**
- `teamId` (int, required) - Team ID

**Query Parameters:**
- `phase` (string, optional) - Filter by phase
- `status` (string, optional) - Filter by status

**Response:** `200 OK`

---

### GET /api/matches/phase/{phase}
Get matches by phase.

**Path Parameters:**
- `phase` (string, required) - Phase name (GroupStage, RoundOf32, etc.)

**Response:** `200 OK`

---

### POST /api/matches
Create a new match.

**Request Body:**
```json
{
  "homeTeamId": 1,
  "awayTeamId": 2,
  "stadiumId": 1,
  "matchDate": "2026-06-15T20:00:00Z",
  "phase": "GroupStage",
  "round": 1,
  "groupId": 1
}
```

**Response:** `201 Created`

**Validation Rules:**
- homeTeamId ≠ awayTeamId
- matchDate must be in the future (for creation)
- groupId required for GroupStage
- groupId must be null for knockout phases

---

### PUT /api/matches/{id}
Update a match.

**Path Parameters:**
- `id` (int, required) - Match ID

**Request Body:**
```json
{
  "homeTeamId": 1,
  "awayTeamId": 2,
  "stadiumId": 1,
  "matchDate": "2026-06-15T20:00:00Z",
  "phase": "GroupStage",
  "round": 1,
  "groupId": 1,
  "status": "Scheduled"
}
```

**Response:** `200 OK`

---

### PATCH /api/matches/{id}/result
Update match result.

**Path Parameters:**
- `id` (int, required) - Match ID

**Request Body:**
```json
{
  "homeTeamScore": 2,
  "awayTeamScore": 1,
  "homeTeamPenalties": null,
  "awayTeamPenalties": null
}
```

**For knockout with penalties:**
```json
{
  "homeTeamScore": 1,
  "awayTeamScore": 1,
  "homeTeamPenalties": 4,
  "awayTeamPenalties": 5
}
```

**Response:** `200 OK`
```json
{
  "data": {
    "id": 1,
    "homeTeamScore": 2,
    "awayTeamScore": 1,
    "winnerTeamId": 1,
    "match": {
      "id": 1,
      "status": "Finished"
    }
  }
}
```

**Business Logic:**
- Automatically updates match status to "Finished"
- Calculates winner based on scores
- Updates group standings if group stage match
- Validates that knockout matches have a winner

---

### DELETE /api/matches/{id}
Delete a match.

**Path Parameters:**
- `id` (int, required) - Match ID

**Response:** `204 No Content`

---

## 4. Stadiums Endpoints

### GET /api/stadiums
Get all stadiums.

**Query Parameters:**
- `country` (string, optional) - Filter by country
- `city` (string, optional) - Filter by city

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "name": "MetLife Stadium",
      "city": "East Rutherford",
      "country": "United States",
      "capacity": 82500,
      "imageUrl": "https://example.com/stadiums/metlife.jpg",
      "latitude": 40.8128,
      "longitude": -74.0742,
      "matchCount": 8
    }
  ]
}
```

---

### GET /api/stadiums/{id}
Get a specific stadium by ID.

**Path Parameters:**
- `id` (int, required) - Stadium ID

**Response:** `200 OK`

---

### GET /api/stadiums/{id}/matches
Get all matches at a stadium.

**Path Parameters:**
- `id` (int, required) - Stadium ID

**Query Parameters:**
- `phase` (string, optional) - Filter by phase
- `status` (string, optional) - Filter by status

**Response:** `200 OK`

---

### POST /api/stadiums
Create a new stadium.

**Request Body:**
```json
{
  "name": "MetLife Stadium",
  "city": "East Rutherford",
  "country": "United States",
  "capacity": 82500,
  "imageUrl": "https://example.com/stadiums/metlife.jpg",
  "latitude": 40.8128,
  "longitude": -74.0742
}
```

**Response:** `201 Created`

---

## 5. Dashboard Endpoints

### GET /api/dashboard
Get dashboard summary data.

**Response:** `200 OK`
```json
{
  "data": {
    "upcomingMatches": [
      {
        "id": 1,
        "homeTeam": { "name": "Argentina", "code": "ARG" },
        "awayTeam": { "name": "Brazil", "code": "BRA" },
        "matchDate": "2026-06-15T20:00:00Z",
        "stadium": { "name": "MetLife Stadium", "city": "East Rutherford" }
      }
    ],
    "recentResults": [
      {
        "id": 2,
        "homeTeam": { "name": "Germany", "code": "GER" },
        "awayTeam": { "name": "France", "code": "FRA" },
        "result": { "homeTeamScore": 2, "awayTeamScore": 1 },
        "matchDate": "2026-06-14T20:00:00Z"
      }
    ],
    "groupLeaders": [
      {
        "groupName": "Group A",
        "team": { "name": "Argentina", "code": "ARG" },
        "points": 7,
        "goalDifference": 5
      }
    ],
    "qualifiedTeams": [
      { "id": 1, "name": "Argentina", "code": "ARG" }
    ],
    "tournamentStats": {
      "totalMatches": 104,
      "matchesPlayed": 48,
      "matchesRemaining": 56,
      "totalGoals": 142,
      "averageGoalsPerMatch": 2.96
    }
  }
}
```

---

### GET /api/dashboard/stats
Get tournament statistics.

**Response:** `200 OK`
```json
{
  "data": {
    "totalTeams": 48,
    "totalGroups": 12,
    "totalStadiums": 16,
    "totalMatches": 104,
    "matchesPlayed": 48,
    "matchesScheduled": 56,
    "totalGoals": 142,
    "averageGoalsPerMatch": 2.96,
    "currentPhase": "GroupStage",
    "tournamentStartDate": "2026-06-11T00:00:00Z",
    "tournamentEndDate": "2026-07-19T00:00:00Z"
  }
}
```

---

## 6. Knockout Phase Endpoints

### GET /api/knockout/bracket
Get the complete knockout bracket.

**Response:** `200 OK`
```json
{
  "data": {
    "roundOf32": [
      {
        "id": 49,
        "homeTeam": { "id": 1, "name": "Argentina", "code": "ARG" },
        "awayTeam": { "id": 2, "name": "Brazil", "code": "BRA" },
        "matchDate": "2026-06-27T20:00:00Z",
        "status": "Finished",
        "result": { "homeTeamScore": 2, "awayTeamScore": 1, "winnerTeamId": 1 }
      }
    ],
    "roundOf16": [...],
    "quarterFinals": [...],
    "semiFinals": [...],
    "thirdPlace": {...},
    "final": {...}
  }
}
```

---

### GET /api/knockout/round/{round}
Get matches for a specific knockout round.

**Path Parameters:**
- `round` (string, required) - Round name (RoundOf32, RoundOf16, QuarterFinals, SemiFinals, ThirdPlace, Final)

**Response:** `200 OK`

---

## HTTP Status Codes

### Success Codes
- `200 OK` - Request successful
- `201 Created` - Resource created successfully
- `204 No Content` - Request successful, no content to return

### Client Error Codes
- `400 Bad Request` - Invalid request format
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource conflict (e.g., duplicate)
- `422 Unprocessable Entity` - Validation errors

### Server Error Codes
- `500 Internal Server Error` - Server error
- `503 Service Unavailable` - Service temporarily unavailable

---

## Rate Limiting

- **Rate Limit:** 100 requests per minute per IP
- **Headers:**
  - `X-RateLimit-Limit: 100`
  - `X-RateLimit-Remaining: 95`
  - `X-RateLimit-Reset: 1623456789`

---

## CORS Configuration

**Allowed Origins:**
- Development: `http://localhost:3000`, `http://localhost:5173`
- Production: `https://worldcup2026.com`

**Allowed Methods:**
- GET, POST, PUT, PATCH, DELETE, OPTIONS

**Allowed Headers:**
- Content-Type, Authorization, X-Requested-With

---

## Authentication (Future)

When authentication is implemented:

**Headers:**
```
Authorization: Bearer <jwt_token>
```

**Protected Endpoints:**
- POST, PUT, PATCH, DELETE operations
- Admin-only endpoints

**Public Endpoints:**
- All GET operations
- Dashboard endpoints

---

## API Documentation

- **Swagger UI:** Available at `/swagger`
- **OpenAPI Spec:** Available at `/swagger/v1/swagger.json`
- **ReDoc:** Available at `/api-docs`

---

## Versioning Strategy

### Current Approach
- No versioning (implicit v1)
- Breaking changes will introduce v2

### Future Versioning
- URL versioning: `/api/v2/...`
- Header versioning: `Accept: application/vnd.worldcup.v2+json`

---

## Best Practices

1. **Use appropriate HTTP methods**
2. **Return appropriate status codes**
3. **Include timestamps in responses**
4. **Provide clear error messages**
5. **Support filtering and pagination**
6. **Use consistent naming conventions**
7. **Document all endpoints**
8. **Version the API**
9. **Implement rate limiting**
10. **Enable CORS properly**