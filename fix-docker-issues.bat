@echo off
echo ACC-CALC Docker Troubleshooting and Fix Script
echo ================================================
echo.

echo Step 1: Stopping all services...
docker-compose down --remove-orphans

echo.
echo Step 2: Updating pnpm lockfile...
cd VueFrontend
if exist "pnpm-lock.yaml" (
    echo Found pnpm-lock.yaml, updating...
    pnpm install
    echo ✓ Lockfile updated
) else (
    echo No pnpm-lock.yaml found, creating...
    pnpm install
    echo ✓ Lockfile created
)

cd ..

echo Step 3: Checking for empty Vue files...
cd VueFrontend
powershell -Command "Get-ChildItem -Path src -Filter '*.vue' -Recurse | Where-Object {$_.Length -eq 0} | ForEach-Object { Write-Host 'Empty file found:' $_.FullName; '<template><div>Geliştirme aşamasında</div></template><script setup></script>' | Out-File -FilePath $_.FullName -Encoding UTF8 }"
cd ..

echo.
echo Step 4: Cleaning Docker build cache...
docker system prune -f

echo.
echo Step 5: Rebuilding and starting services...
docker-compose up --build -d

echo.
echo Step 6: Waiting for services...
timeout /t 45 >nul

echo.
echo Step 7: Checking service health...
docker-compose ps

echo.
echo If services are still not running properly, check logs:
echo docker-compose logs -f
echo.
echo Troubleshooting completed!
pause
