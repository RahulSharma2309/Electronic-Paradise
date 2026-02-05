# ============================================================================
# Create all FreshHarvest Market databases in LocalDB
# ============================================================================
# Run this script in PowerShell to create all 10 databases
# ============================================================================

Write-Host "Creating FreshHarvest Market databases in LocalDB..." -ForegroundColor Cyan
Write-Host ""

# Local Development databases
$localDatabases = @(
    "EP_Local_AuthDb",
    "EP_Local_UserDb",
    "EP_Local_ProductDb",
    "EP_Local_OrderDb",
    "EP_Local_PaymentDb"
)

# Staging databases
$stagingDatabases = @(
    "EP_Staging_AuthDb",
    "EP_Staging_UserDb",
    "EP_Staging_ProductDb",
    "EP_Staging_OrderDb",
    "EP_Staging_PaymentDb"
)

Write-Host "=== Creating Local Development Databases ===" -ForegroundColor Yellow
foreach ($db in $localDatabases) {
    Write-Host "Creating $db..." -NoNewline
    try {
        sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$db') CREATE DATABASE [$db]" 2>$null
        Write-Host " Done" -ForegroundColor Green
    } catch {
        Write-Host " Failed: $_" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=== Creating Staging Databases ===" -ForegroundColor Yellow
foreach ($db in $stagingDatabases) {
    Write-Host "Creating $db..." -NoNewline
    try {
        sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$db') CREATE DATABASE [$db]" 2>$null
        Write-Host " Done" -ForegroundColor Green
    } catch {
        Write-Host " Failed: $_" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=== Database Creation Complete ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "To verify, connect to (localdb)\MSSQLLocalDB in SSMS"
Write-Host ""
Write-Host "Usage:"
Write-Host "  Development (EP_Local_*):  dotnet run"
Write-Host "  Staging (EP_Staging_*):    dotnet run --environment Staging"
Write-Host ""
