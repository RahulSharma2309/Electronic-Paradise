# Build and Start Electronic Paradise - Docker Setup
# This script disables BuildKit and builds/starts all services

Write-Host "=== Electronic Paradise - Docker Build & Start ===" -ForegroundColor Cyan
Write-Host ""

# Navigate to infra directory
Set-Location $PSScriptRoot\..\infra

# Disable BuildKit to avoid ordering issues (set for this session)
$env:DOCKER_BUILDKIT=0
Write-Host "BuildKit disabled for this session" -ForegroundColor Green

# Clean up existing containers
Write-Host ""
Write-Host "Stopping existing containers..." -ForegroundColor Yellow
docker-compose down -v

# Build all services
Write-Host ""
Write-Host "Building all services - this may take 5-10 minutes..." -ForegroundColor Yellow
docker-compose build

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "All services built successfully!" -ForegroundColor Green
    
    # Start all services
    Write-Host ""
    Write-Host "Starting all services..." -ForegroundColor Yellow
    docker-compose up -d
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "All services started!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Waiting 30 seconds for services to be healthy..." -ForegroundColor Yellow
        Start-Sleep -Seconds 30
        
        # Show service status
        Write-Host ""
        Write-Host "=== Service Status ===" -ForegroundColor Cyan
        docker ps --format "table {{.Names}}`t{{.Status}}`t{{.Ports}}"
        
        Write-Host ""
        Write-Host "=== Access URLs ===" -ForegroundColor Cyan
        Write-Host "Frontend:        http://localhost:3000" -ForegroundColor White
        Write-Host "API Gateway:     http://localhost:5000" -ForegroundColor White
        Write-Host "Auth Service:    http://localhost:5001/swagger" -ForegroundColor White
        Write-Host "Product Service: http://localhost:5002/swagger" -ForegroundColor White
        Write-Host "Payment Service: http://localhost:5003/swagger" -ForegroundColor White
        Write-Host "Order Service:   http://localhost:5004/swagger" -ForegroundColor White
        Write-Host "User Service:    http://localhost:5005/swagger" -ForegroundColor White
        Write-Host ""
        Write-Host "Setup complete! All services should be running." -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "Failed to start services. Check logs with: docker-compose logs" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host ""
    Write-Host "Build failed. Check the error messages above." -ForegroundColor Red
    exit 1
}
