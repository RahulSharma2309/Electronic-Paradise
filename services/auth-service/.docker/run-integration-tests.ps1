param(
  [string]$Configuration = "Release",
  [switch]$KeepEnvironment
)

$ErrorActionPreference = "Stop"

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
Push-Location $here

try {
  $projectName = "auth-it"

  Write-Host "Publishing integration tests..."
  $testProject = Join-Path $here "..\\test\\integration-test\\AuthService.Integration.Test\\AuthService.Integration.Test.csproj"
  $publishDir = Join-Path $here "..\\test\\integration-test\\AuthService.Integration.Test\\obj\\docker\\publish"

  dotnet publish $testProject -c $Configuration -o $publishDir | Out-Host
  if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed" }

  Write-Host "Running docker compose integration tests..."
  $env:COMPOSE_PROJECT_NAME = $projectName

  Write-Host "Building docker images explicitly..."
  $serviceContext = (Resolve-Path (Join-Path $here "..")).Path
  $serviceDockerfile = Join-Path $serviceContext "src\\Dockerfile"
  $testContext = (Resolve-Path (Join-Path $here "..\\test\\integration-test\\AuthService.Integration.Test")).Path
  $mockContext = Join-Path $testContext "UserServiceMock"

  $ghToken = $env:GITHUB_TOKEN
  if ([string]::IsNullOrWhiteSpace($ghToken)) {
    docker build -t auth-service:it -f $serviceDockerfile $serviceContext | Out-Host
  } else {
    docker build -t auth-service:it --build-arg "GITHUB_TOKEN=$ghToken" -f $serviceDockerfile $serviceContext | Out-Host
  }
  if ($LASTEXITCODE -ne 0) { throw "docker build auth-service failed" }

  docker build -t auth-user-service-mock:it -f (Join-Path $mockContext "Dockerfile.mockserver") $mockContext | Out-Host
  if ($LASTEXITCODE -ne 0) { throw "docker build user-service-mock failed" }

  docker build -t auth-service.integration.test:it -f (Join-Path $testContext "Dockerfile.test") $testContext | Out-Host
  if ($LASTEXITCODE -ne 0) { throw "docker build auth-service integration tests failed" }

  docker compose -f docker-compose.yml -f docker-compose.test.yml up --no-build --abort-on-container-exit --exit-code-from auth-service.integration.test | Out-Host
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

