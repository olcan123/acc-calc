-- Initialize NetCoreBackend database
-- This script creates the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'NetCoreBackend')
BEGIN
    CREATE DATABASE [NetCoreBackend];
    PRINT 'NetCoreBackend database created successfully';
END
ELSE
BEGIN
    PRINT 'NetCoreBackend database already exists';
END
GO

-- Use the NetCoreBackend database
USE [NetCoreBackend];
GO

-- You can add any additional table creation scripts here if needed
-- For now, we'll let Entity Framework handle the table creation
PRINT 'Database initialization completed';
GO
