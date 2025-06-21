@echo off
echo Stopping ACC-CALC Docker services...

docker-compose down --remove-orphans

echo.
echo All services stopped.
echo To remove all data: docker-compose down -v
echo.
pause
