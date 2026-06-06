# FIFA World Cup 2026 - Database Check and Seed Script
Write-Host "==================================" -ForegroundColor Cyan
Write-Host "FIFA World Cup 2026 - DB Check" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

$apiUrl = "http://localhost:5004"
$apiRunning = $false
$hasData = $false

# Check if API is running
Write-Host "1. Checking if API is running..." -ForegroundColor Yellow
try {
    $null = Invoke-WebRequest -Uri "$apiUrl/swagger/index.html" -Method Get -TimeoutSec 5 -ErrorAction Stop
    Write-Host "   ✓ API is running on $apiUrl" -ForegroundColor Green
    $apiRunning = $true
} catch {
    Write-Host "   ✗ API is not running!" -ForegroundColor Red
    Write-Host "   Please start the API first:" -ForegroundColor Yellow
    Write-Host "   cd backend\src\WorldCup2026.API" -ForegroundColor Yellow
    Write-Host "   dotnet run" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "==================================" -ForegroundColor Cyan
    exit 1
}

Write-Host ""

# Check if data exists
if ($apiRunning) {
    Write-Host "2. Checking if database has data..." -ForegroundColor Yellow
    try {
        $teamsUrl = $apiUrl + '/api/Teams?pageNumber=1&pageSize=1'
        $teamsResponse = Invoke-RestMethod -Uri $teamsUrl -Method Get
        $teamCount = $teamsResponse.totalCount
        
        if ($teamCount -gt 0) {
            $hasData = $true
            Write-Host "   ✓ Database has data: $teamCount teams found" -ForegroundColor Green
            Write-Host ""
            Write-Host "Database Statistics:" -ForegroundColor Cyan
            
            # Get more statistics
            $groupsResponse = Invoke-RestMethod -Uri "$apiUrl/api/Groups" -Method Get -ErrorAction SilentlyContinue
            $stadiumsUrl = $apiUrl + '/api/Stadiums?pageNumber=1&pageSize=100'
            $stadiumsResponse = Invoke-RestMethod -Uri $stadiumsUrl -Method Get -ErrorAction SilentlyContinue
            
            Write-Host "   - Teams: $teamCount" -ForegroundColor White
            if ($groupsResponse) {
                Write-Host "   - Groups: $($groupsResponse.Count)" -ForegroundColor White
            }
            if ($stadiumsResponse) {
                Write-Host "   - Stadiums: $($stadiumsResponse.totalCount)" -ForegroundColor White
            }
            
            Write-Host ""
            Write-Host "✓ Database is ready! You can now use the frontend." -ForegroundColor Green
            Write-Host "  Frontend: http://localhost:5173" -ForegroundColor Cyan
            Write-Host "  Backend API: $apiUrl" -ForegroundColor Cyan
            Write-Host "  Swagger: $apiUrl/swagger" -ForegroundColor Cyan
        }
    } catch {
        Write-Host "   ! Could not check database" -ForegroundColor Yellow
        Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}


# Seed database if empty
if ($apiRunning -and -not $hasData) {
    Write-Host "   ! Database is empty" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "3. Seeding database..." -ForegroundColor Yellow
    
    try {
        $seedResponse = Invoke-RestMethod -Uri "$apiUrl/api/seed" -Method Post
        Write-Host "   ✓ $($seedResponse.message)" -ForegroundColor Green
        
        Write-Host ""
        Write-Host "4. Verifying seeded data..." -ForegroundColor Yellow
        $verifyUrl = $apiUrl + '/api/Teams?pageNumber=1&pageSize=1'
        $verifyResponse = Invoke-RestMethod -Uri $verifyUrl -Method Get
        Write-Host "   ✓ Database now has $($verifyResponse.totalCount) teams" -ForegroundColor Green
        
        Write-Host ""
        Write-Host "✓ Database seeded successfully!" -ForegroundColor Green
        Write-Host "  You can now use the frontend at http://localhost:5173" -ForegroundColor Cyan
    } catch {
        Write-Host "   ✗ Error seeding database" -ForegroundColor Red
        Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    }

Write-Host ""
Write-Host "==================================" -ForegroundColor Cyan

# Made with Bob
