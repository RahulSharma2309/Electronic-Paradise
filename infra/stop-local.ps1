# Stop all locally running FreshHarvest Market services
# Usage: .\stop-local.ps1

$ErrorActionPreference = "SilentlyContinue"

Write-Host "Stopping FreshHarvest Market services..." -ForegroundColor Yellow
Write-Host ""

# Find and kill dotnet processes running our services
$servicePorts = @(5000, 5001, 5002, 5003, 5004, 5005)

foreach ($port in $servicePorts) {
    $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue | Where-Object { $_.State -eq 'Listen' }
    if ($connection) {
        $process = Get-Process -Id $connection.OwningProcess -ErrorAction SilentlyContinue
        if ($process) {
            Write-Host "  Stopping process on port $port (PID: $($process.Id), Name: $($process.ProcessName))" -ForegroundColor Gray
            Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
        }
    }
}

# Stop node process on port 3000 (frontend)
$frontendConnection = Get-NetTCPConnection -LocalPort 3000 -ErrorAction SilentlyContinue | Where-Object { $_.State -eq 'Listen' }
if ($frontendConnection) {
    $process = Get-Process -Id $frontendConnection.OwningProcess -ErrorAction SilentlyContinue
    if ($process) {
        Write-Host "  Stopping frontend on port 3000 (PID: $($process.Id))" -ForegroundColor Gray
        Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
    }
}

Write-Host ""
Write-Host "All services stopped." -ForegroundColor Green
