-- Run this in SSMS connected to (localdb)\MSSQLLocalDB

-- Local Development Databases
IF DB_ID('EP_Local_AuthDb') IS NULL CREATE DATABASE [EP_Local_AuthDb];
IF DB_ID('EP_Local_UserDb') IS NULL CREATE DATABASE [EP_Local_UserDb];
IF DB_ID('EP_Local_ProductDb') IS NULL CREATE DATABASE [EP_Local_ProductDb];
IF DB_ID('EP_Local_OrderDb') IS NULL CREATE DATABASE [EP_Local_OrderDb];
IF DB_ID('EP_Local_PaymentDb') IS NULL CREATE DATABASE [EP_Local_PaymentDb];

-- Staging Databases
IF DB_ID('EP_Staging_AuthDb') IS NULL CREATE DATABASE [EP_Staging_AuthDb];
IF DB_ID('EP_Staging_UserDb') IS NULL CREATE DATABASE [EP_Staging_UserDb];
IF DB_ID('EP_Staging_ProductDb') IS NULL CREATE DATABASE [EP_Staging_ProductDb];
IF DB_ID('EP_Staging_OrderDb') IS NULL CREATE DATABASE [EP_Staging_OrderDb];
IF DB_ID('EP_Staging_PaymentDb') IS NULL CREATE DATABASE [EP_Staging_PaymentDb];

PRINT 'All databases created successfully!';
