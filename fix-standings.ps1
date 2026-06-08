# PowerShell script to recalculate all standings
# This script calls the API endpoint to recalculate all group standings

$apiUrl = "http://localhost:5000/api/standings/recalculate-all"

Write-Host "Recalculating all group standings..." -ForegroundColor Cyan

try {
    $response = Invoke-RestMethod -Uri $apiUrl -Method Post -ContentType "application/json"
    Write-Host "Success: $($response.message)" -ForegroundColor Green
    
    # Now fetch and display standings for verification
    Write-Host "`nFetching updated standings..." -ForegroundColor Cyan
    $standingsUrl = "http://localhost:5000/api/standings/all"
    $standings = Invoke-RestMethod -Uri $standingsUrl -Method Get
    
    Write-Host "`nStandings Summary:" -ForegroundColor Yellow
    $standings | Format-Table -Property TeamName, GroupName, Position, Played, Won, Drawn, Lost, GoalsFor, GoalsAgainst, GoalDifference, Points -AutoSize
    
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Make sure the API is running on http://localhost:5000" -ForegroundColor Yellow
}

# Made with Bob
