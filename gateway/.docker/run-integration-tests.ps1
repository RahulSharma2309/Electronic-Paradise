param(
  [string]$Configuration = "Release",
  [switch]$KeepEnvironment
)

$ErrorActionPreference = "Stop"

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
Push-Location $here

try {
  $projectName = "gateway-it"

  Write-Host "Publishing integration tests..."
  $testProject = Join-Path $here "..\\test\\integration-test\\Gateway.Integration.Test\\Gateway.Integration.Test.csproj"
  $publishDir = Join-Path $here "..\\test\\integration-test\\Gateway.Integration.Test\\obj\\docker\\publish"

  dotnet publish $testProject -c $Configuration -o $publishDir | Out-Host
  if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed" }

  Write-Host "Running docker compose integration tests..."
  $env:COMPOSE_PROJECT_NAME = $projectName

  Write-Host "Building docker images explicitly..."
  $gatewayContext = (Resolve-Path (Join-Path $here "..")).Path
  $gatewayDockerfile = Join-Path $gatewayContext "src\\Dockerfile"
  $testContext = (Resolve-Path (Join-Path $here "..\\test\\integration-test\\Gateway.Integration.Test")).Path
  $mockContext = Join-Path $testContext "Mocks"

  docker build -t gateway:it -f $gatewayDockerfile $gatewayContext | Out-Host
  if ($LASTEXITCODE -ne 0) { throw "docker build gateway failed" }

  docker build -t gateway-dependency-mock:it -f (Join-Path $mockContext "Dockerfile.mockserver") $mockContext | Out-Host
  if ($LASTEXITCODE -ne 0) { throw "docker build dependency-mock failed" }

  docker build -t gateway.integration.test:it -f (Join-Path $testContext "Dockerfile.test") $gatewayContext | Out-Host
  if ($LASTEXITCODE -ne 0) { throw "docker build gateway integration tests failed" }

  docker compose -f docker-compose.yml -f docker-compose.test.yml up --no-build --abort-on-container-exit --exit-code-from gateway.integration.test | Out-Host
  $exit = $LASTEXITCODE
  if ($exit -ne 0) { exit $exit }
}
finally {
  if (-not $KeepEnvironment) {
    Write-Host "Cleaning up docker compose environment..."
    docker compose -f docker-compose.yml -f docker-compose.test.yml down -v | Out-Host
  }
  Pop-Location
}

