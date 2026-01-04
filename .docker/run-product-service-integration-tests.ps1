param(
  [string]$Configuration = "Release",
  [switch]$KeepEnvironment
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$serviceRunner = Join-Path $repoRoot "..\\services\\product-service\\.docker\\run-integration-tests.ps1"

if (-not (Test-Path $serviceRunner)) {
  throw "Product service integration runner not found at: $serviceRunner"
}

& $serviceRunner -Configuration $Configuration -KeepEnvironment:$KeepEnvironment

