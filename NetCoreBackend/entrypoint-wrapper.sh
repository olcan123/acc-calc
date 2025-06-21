#!/bin/bash
# entrypoint-wrapper.sh - Custom entrypoint for SQL Server with database initialization

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to be ready
echo "Waiting for SQL Server to start..."
sleep 30

# Run the initialization script
echo "Running database initialization script..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i /docker-entrypoint-initdb.d/init-database.sql -C

echo "Database initialization completed"

# Keep the container running by waiting for the SQL Server process
wait
