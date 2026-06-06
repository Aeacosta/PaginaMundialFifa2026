# FIFA World Cup 2026 - Database Check Script
Write-Host "==================================" -ForegroundColor Cyan
Write-Host "FIFA World Cup 2026 - DB Check" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

$apiUrl = "http://localhost:5004"

# Check API
Write-Host "Checking API status..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$apiUrl/api/Teams?pageNumber=1&pageSize=1" -Method Get -ErrorAction Stop
    $count = $response.totalCount
    Write-Host "✓ API is running" -ForegroundColor Green
    Write-Host "✓ Database has $count teams" -ForegroundColor Green
    
    if ($count -eq 0) {
        Write-Host ""
        Write-Host "Database is empty. Seeding..." -ForegroundColor Yellow
        $seed = Invoke-RestMethod -Uri "$apiUrl/api/seed" -Method Post
        Write-Host "✓ $($seed.message)" -ForegroundColor Green
    }
} catch {
    Write-Host "✗ Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Make sure the API is running:" -ForegroundColor Yellow
    Write-Host "  cd backend\src\WorldCup2026.API" -ForegroundColor White
    Write-Host "  dotnet run" -ForegroundColor White
}

Write-Host ""
Write-Host "==================================" -ForegroundColor Cyan

# Made with Bob
