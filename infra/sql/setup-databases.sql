-- ============================================================================
-- Electronic Paradise - Database Setup Script
-- ============================================================================
-- This script creates the databases for Local Development and Staging environments
--
-- FOR LOCALDB:
--   Connect to: (localdb)\MSSQLLocalDB in SSMS
--   Or run: sqlcmd -S "(localdb)\MSSQLLocalDB" -i setup-databases.sql
--
-- FOR SQL SERVER EXPRESS:
--   Connect to: localhost,1433 or .\SQLEXPRESS in SSMS
-- ============================================================================

-- ============================================================================
-- PART 1: LOCAL DEVELOPMENT DATABASES
-- Used when running services via 'dotnet run' or Visual Studio
-- ============================================================================

PRINT '=== Creating Local Development Databases ==='

-- Auth Service Database (Local)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Local_AuthDb')
BEGIN
    CREATE DATABASE [EP_Local_AuthDb]
    PRINT 'Created database: EP_Local_AuthDb'
END
ELSE
    PRINT 'Database EP_Local_AuthDb already exists'
GO

-- User Service Database (Local)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Local_UserDb')
BEGIN
    CREATE DATABASE [EP_Local_UserDb]
    PRINT 'Created database: EP_Local_UserDb'
END
ELSE
    PRINT 'Database EP_Local_UserDb already exists'
GO

-- Product Service Database (Local)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Local_ProductDb')
BEGIN
    CREATE DATABASE [EP_Local_ProductDb]
    PRINT 'Created database: EP_Local_ProductDb'
END
ELSE
    PRINT 'Database EP_Local_ProductDb already exists'
GO

-- Order Service Database (Local)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Local_OrderDb')
BEGIN
    CREATE DATABASE [EP_Local_OrderDb]
    PRINT 'Created database: EP_Local_OrderDb'
END
ELSE
    PRINT 'Database EP_Local_OrderDb already exists'
GO

-- Payment Service Database (Local)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Local_PaymentDb')
BEGIN
    CREATE DATABASE [EP_Local_PaymentDb]
    PRINT 'Created database: EP_Local_PaymentDb'
END
ELSE
    PRINT 'Database EP_Local_PaymentDb already exists'
GO

-- ============================================================================
-- PART 2: STAGING DATABASES
-- Used by Kubernetes cluster (Docker Desktop K8s)
-- ============================================================================

PRINT ''
PRINT '=== Creating Staging Databases ==='

-- Auth Service Database (Staging)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Staging_AuthDb')
BEGIN
    CREATE DATABASE [EP_Staging_AuthDb]
    PRINT 'Created database: EP_Staging_AuthDb'
END
ELSE
    PRINT 'Database EP_Staging_AuthDb already exists'
GO

-- User Service Database (Staging)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Staging_UserDb')
BEGIN
    CREATE DATABASE [EP_Staging_UserDb]
    PRINT 'Created database: EP_Staging_UserDb'
END
ELSE
    PRINT 'Database EP_Staging_UserDb already exists'
GO

-- Product Service Database (Staging)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Staging_ProductDb')
BEGIN
    CREATE DATABASE [EP_Staging_ProductDb]
    PRINT 'Created database: EP_Staging_ProductDb'
END
ELSE
    PRINT 'Database EP_Staging_ProductDb already exists'
GO

-- Order Service Database (Staging)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Staging_OrderDb')
BEGIN
    CREATE DATABASE [EP_Staging_OrderDb]
    PRINT 'Created database: EP_Staging_OrderDb'
END
ELSE
    PRINT 'Database EP_Staging_OrderDb already exists'
GO

-- Payment Service Database (Staging)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EP_Staging_PaymentDb')
BEGIN
    CREATE DATABASE [EP_Staging_PaymentDb]
    PRINT 'Created database: EP_Staging_PaymentDb'
END
ELSE
    PRINT 'Database EP_Staging_PaymentDb already exists'
GO

-- ============================================================================
-- PART 3: OPTIONAL - Create a dedicated login for the application
-- Uncomment and customize if you don't want to use 'sa'
-- ============================================================================

/*
-- Create a login for Electronic Paradise
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'ep_app_user')
BEGIN
    CREATE LOGIN [ep_app_user] WITH PASSWORD = 'YourSecurePassword123!'
    PRINT 'Created login: ep_app_user'
END
GO

-- Grant access to Local databases
USE [EP_Local_AuthDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO

USE [EP_Local_UserDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO

USE [EP_Local_ProductDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO

USE [EP_Local_OrderDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO

USE [EP_Local_PaymentDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO

-- Grant access to Staging databases
USE [EP_Staging_AuthDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO

USE [EP_Staging_UserDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO

USE [EP_Staging_ProductDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO

USE [EP_Staging_OrderDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO

USE [EP_Staging_PaymentDb]
GO
CREATE USER [ep_app_user] FOR LOGIN [ep_app_user]
ALTER ROLE db_owner ADD MEMBER [ep_app_user]
GO
*/

PRINT ''
PRINT '=== Database Setup Complete ==='
PRINT ''
PRINT 'Local Development Databases:'
PRINT '  - EP_Local_AuthDb'
PRINT '  - EP_Local_UserDb'
PRINT '  - EP_Local_ProductDb'
PRINT '  - EP_Local_OrderDb'
PRINT '  - EP_Local_PaymentDb'
PRINT ''
PRINT 'Staging Databases (for K8s):'
PRINT '  - EP_Staging_AuthDb'
PRINT '  - EP_Staging_UserDb'
PRINT '  - EP_Staging_ProductDb'
PRINT '  - EP_Staging_OrderDb'
PRINT '  - EP_Staging_PaymentDb'
PRINT ''
PRINT 'Next Steps:'
PRINT '1. Tables will be created automatically by EF Core migrations when services start'
PRINT '2. Update your SQL Server to allow TCP/IP connections (SQL Server Configuration Manager)'
PRINT '3. Ensure SQL Server is listening on port 1433'
GO
