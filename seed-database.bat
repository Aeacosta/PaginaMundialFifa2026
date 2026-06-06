@echo off
echo ========================================
echo FIFA World Cup 2026 - Database Seeding
echo ========================================
echo.

echo Seeding database...
powershell -Command "Invoke-RestMethod -Uri 'http://localhost:5004/api/seed' -Method Post"

echo.
echo ========================================
echo Done!
echo ========================================
pause

@REM Made with Bob
