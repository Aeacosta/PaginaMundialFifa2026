# Docker Database Connection Fix

## Problem
The Docker container was failing because:
1. Database migrations were not being applied automatically in Production environment
2. Database was not being seeded on startup
3. The application tried to query empty database tables, causing SQL errors

## Solution Applied

### 1. Updated Program.cs
Added automatic database initialization on application startup:
- Applies EF Core migrations automatically
- Seeds database if it's empty
- Logs all operations for debugging
- Works in all environments (Development, Production)

### 2. Updated Dockerfile
- Added `curl` installation for health checks
- Ensured proper data directory creation

### 3. Added Health Check Endpoint
- Created `/health` endpoint for Docker health checks
- Returns JSON with status and timestamp

## How to Test

### Step 1: Clean Up Existing Containers and Volumes
```powershell
# Stop and remove containers
docker-compose down

# Remove volumes to start fresh
docker volume rm proyecto_backend-data

# Or remove all unused volumes
docker volume prune -f
```

### Step 2: Rebuild and Start
```powershell
# Rebuild images (important after code changes)
docker-compose build --no-cache

# Start services
docker-compose up -d

# Or do both in one command
docker-compose up -d --build
```

### Step 3: Monitor Logs
```powershell
# Watch backend logs
docker-compose logs -f backend

# Look for these success messages:
# - "Applying database migrations..."
# - "Database migrations applied successfully"
# - "Database is empty. Starting seeding process..."
# - "Database seeded successfully"
```

### Step 4: Verify Health
```powershell
# Check container status
docker-compose ps

# Test health endpoint
curl http://localhost:5004/health

# Should return: {"status":"healthy","timestamp":"2026-06-07T..."}
```

### Step 5: Test API Endpoints
```powershell
# Get all teams
curl http://localhost:5004/api/teams

# Get dashboard data
curl http://localhost:5004/api/dashboard

# Get upcoming matches
curl http://localhost:5004/api/matches/upcoming
```

## Troubleshooting

### If containers fail to start:
```powershell
# Check logs for errors
docker-compose logs backend

# Check if port is already in use
netstat -ano | findstr :5004

# Restart with fresh build
docker-compose down
docker-compose up -d --build
```

### If database is still empty:
```powershell
# Enter the container
docker exec -it worldcup2026-backend bash

# Check if database file exists
ls -la /app/data/

# Check database content (if sqlite3 is available)
sqlite3 /app/data/WorldCup2026.db ".tables"
```

### If health check fails:
```powershell
# Test from inside container
docker exec -it worldcup2026-backend curl http://localhost:5004/health

# Check if app is listening on correct port
docker exec -it worldcup2026-backend netstat -tlnp
```

## Key Changes Summary

### Program.cs Changes:
- Added database migration on startup (line 75-107)
- Added health check endpoint (line 129-131)
- Automatic seeding when database is empty

### Dockerfile Changes:
- Installed curl for health checks (line 27)
- Maintained data directory creation

### docker-compose.yml:
- No changes needed (already configured correctly)
- Uses persistent volume for database
- Health check configured properly

## Expected Behavior

1. **First Run**: 
   - Migrations applied
   - Database seeded with all data
   - ~30-60 seconds startup time

2. **Subsequent Runs**:
   - Migrations checked (no changes)
   - Seeding skipped (data exists)
   - ~10-20 seconds startup time

3. **After Volume Reset**:
   - Behaves like first run
   - Fresh database created and seeded

## Production Considerations

- Database is persisted in Docker volume `proyecto_backend-data`
- Volume survives container restarts
- To reset database: remove volume and restart
- Logs show all database operations
- Health check ensures container is ready before frontend starts

## Made with Bob