@echo off
echo Testing ACC-CALC API Health...
echo.

echo Testing Backend Health Check...
curl -f http://localhost:5000/health
if %errorlevel% equ 0 (
    echo ✓ Backend is healthy
) else (
    echo ✗ Backend is not responding
)

echo.
echo Testing Frontend...
curl -f http://localhost/
if %errorlevel% equ 0 (
    echo ✓ Frontend is healthy
) else (
    echo ✗ Frontend is not responding
)

echo.
echo Testing SQL Server connection...
docker exec -it acc-calc-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P AccCalc123! -Q "SELECT 1 as HealthCheck" 2>nul
if %errorlevel% equ 0 (
    echo ✓ SQL Server is healthy
) else (
    echo ✗ SQL Server is not responding
)

echo.
echo Health check completed.
pause
