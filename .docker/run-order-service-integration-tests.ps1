param(
  [string]$Configuration = "Release",
  [switch]$KeepEnvironment
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$serviceRunner = Join-Path $repoRoot "..\\services\\order-service\\.docker\\run-integration-tests.ps1"

if (-not (Test-Path $serviceRunner)) {
  throw "Order service integration runner not found at: $serviceRunner"
}

& $serviceRunner -Configuration $Configuration -KeepEnvironment:$KeepEnvironment

