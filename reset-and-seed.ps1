# Stop all running processes
Write-Host "Stopping running processes..." -ForegroundColor Yellow
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
Get-Process -Name "node" -ErrorAction SilentlyContinue | Stop-Process -Force
Start-Sleep -Seconds 2

# Delete the database
Write-Host "Deleting database..." -ForegroundColor Yellow
Remove-Item -Path "backend\src\WorldCup2026.API\worldcup2026.db" -Force -ErrorAction SilentlyContinue
Remove-Item -Path "backend\src\WorldCup2026.API\worldcup2026.db-shm" -Force -ErrorAction SilentlyContinue
Remove-Item -Path "backend\src\WorldCup2026.API\worldcup2026.db-wal" -Force -ErrorAction SilentlyContinue

Write-Host "Database deleted successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Now run: .\start-app.bat" -ForegroundColor Cyan
Write-Host "Then run: .\seed-database.bat" -ForegroundColor Cyan

# Made with Bob
