@echo off
echo Starting ACC-CALC Application with Docker...
echo.

echo Checking Docker...
docker --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: Docker is not installed or not running.
    echo Please install Docker Desktop and make sure it's running.
    pause
    exit /b 1
)

echo Building and starting services...
docker-compose down --remove-orphans
docker-compose up --build -d

echo.
echo Waiting for services to be ready...
timeout /t 30 >nul

echo.
echo ACC-CALC Application is starting up...
echo.
echo Frontend: http://localhost
echo Backend API: http://localhost:5000
echo SQL Server: localhost:1433 (sa/AccCalc123!)
echo.
echo Checking service status...
docker-compose ps

echo.
echo To view logs: docker-compose logs -f
echo To stop: docker-compose down
echo.
pause
