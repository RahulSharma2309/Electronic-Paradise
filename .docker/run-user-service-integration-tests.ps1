param(
  [string]$Configuration = "Release",
  [switch]$KeepEnvironment
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$serviceRunner = Join-Path $repoRoot "..\\services\\user-service\\.docker\\run-integration-tests.ps1"

if (-not (Test-Path $serviceRunner)) {
  throw "User service integration runner not found"
}

& $serviceRunner -Configuration $Configuration -KeepEnvironment:$KeepEnvironment

