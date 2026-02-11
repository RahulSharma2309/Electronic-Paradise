# Run all services locally (connects to LocalDB)
# Usage: .\run-local.ps1
#
# This script starts all microservices on your local machine.
# They will connect to LocalDB databases (EP_Local_*Db).
#
# Ports:
#   - Gateway:         http://localhost:5000
#   - Auth Service:    http://localhost:5001
#   - Product Service: http://localhost:5002
#   - Payment Service: http://localhost:5003
#   - Order Service:   http://localhost:5004
#   - User Service:    http://localhost:5005
#   - Frontend:        http://localhost:3000

param(
    [switch]$SkipFrontend,
    [switch]$SkipBuild
)

$ErrorActionPreference = "Stop"
$RepoRoot = Split-Path -Parent $PSScriptRoot

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  FreshHarvest Market - Local Runner   " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Function to start a service in a new terminal
function Start-Service {
    param(
        [string]$Name,
        [string]$Path,
        [int]$Port
    )
    
    Write-Host "Starting $Name on port $Port..." -ForegroundColor Yellow
    
    $projectPath = Join-Path $RepoRoot $Path
    
    Start-Process powershell -ArgumentList @(
        "-NoExit",
        "-Command",
        "Set-Location '$projectPath'; `$env:ASPNETCORE_URLS='http://localhost:$Port'; Write-Host 'Starting $Name on port $Port...' -ForegroundColor Green; dotnet run"
    )
}

# Function to start frontend
function Start-Frontend {
    Write-Host "Starting Frontend on port 3000..." -ForegroundColor Yellow
    
    $frontendPath = Join-Path $RepoRoot "frontend"
    
    Start-Process powershell -ArgumentList @(
        "-NoExit",
        "-Command",
        "Set-Location '$frontendPath'; Write-Host 'Starting Frontend...' -ForegroundColor Green; npm start"
    )
}

# Optionally build all services first
if (-not $SkipBuild) {
    Write-Host "Building all services..." -ForegroundColor Magenta
    
    # Build platform first (shared dependency)
    Write-Host "  Building: platform/Ep.Platform" -ForegroundColor Gray
    $platformPath = Join-Path $RepoRoot "platform/Ep.Platform"
    Push-Location $platformPath
    $buildResult = dotnet build --verbosity quiet 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "    Platform build failed" -ForegroundColor Red
        Write-Host $buildResult
    }
    Pop-Location
    
    $services = @(
        "services/user-service/src/UserService.API",
        "services/auth-service/src/AuthService.API",
        "services/product-service/src/ProductService.API",
        "services/payment-service/src/PaymentService.API",
        "services/order-service/src/OrderService.API",
        "gateway/src"
    )
    
    foreach ($service in $services) {
        $projectPath = Join-Path $RepoRoot $service
        Write-Host "  Building: $service" -ForegroundColor Gray
        Push-Location $projectPath
        $buildResult = dotnet build --verbosity quiet 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Host "    Build failed for $service" -ForegroundColor Red
        }
        Pop-Location
    }
    
    Write-Host "Build complete!" -ForegroundColor Green
    Write-Host ""
}

# Start services in dependency order
Write-Host "Starting services..." -ForegroundColor Magenta
Write-Host ""

# 1. User Service (no dependencies)
Start-Service -Name "User Service" -Path "services/user-service/src/UserService.API" -Port 5005
Start-Sleep -Seconds 2

# 2. Auth Service (depends on User Service)
Start-Service -Name "Auth Service" -Path "services/auth-service/src/AuthService.API" -Port 5001
Start-Sleep -Seconds 2

# 3. Product Service (no dependencies)
Start-Service -Name "Product Service" -Path "services/product-service/src/ProductService.API" -Port 5002
Start-Sleep -Seconds 2

# 4. Payment Service (depends on User Service)
Start-Service -Name "Payment Service" -Path "services/payment-service/src/PaymentService.API" -Port 5003
Start-Sleep -Seconds 2

# 5. Order Service (depends on Product, Payment, User)
Start-Service -Name "Order Service" -Path "services/order-service/src/OrderService.API" -Port 5004
Start-Sleep -Seconds 2

# 6. Gateway (depends on all services)
Start-Service -Name "Gateway" -Path "gateway/src" -Port 5000
Start-Sleep -Seconds 2

# 7. Frontend (optional)
if (-not $SkipFrontend) {
    Start-Frontend
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  All services started!                " -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Service URLs:" -ForegroundColor Cyan
Write-Host "  Gateway:         http://localhost:5000" -ForegroundColor White
Write-Host "  Auth Service:    http://localhost:5001" -ForegroundColor White
Write-Host "  Product Service: http://localhost:5002" -ForegroundColor White
Write-Host "  Payment Service: http://localhost:5003" -ForegroundColor White
Write-Host "  Order Service:   http://localhost:5004" -ForegroundColor White
Write-Host "  User Service:    http://localhost:5005" -ForegroundColor White
if (-not $SkipFrontend) {
    Write-Host "  Frontend:        http://localhost:3000" -ForegroundColor White
}
Write-Host ""
Write-Host "Press Ctrl+C in each terminal to stop the services." -ForegroundColor Yellow
