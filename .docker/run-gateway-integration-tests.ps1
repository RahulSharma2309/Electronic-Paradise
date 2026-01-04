param(
  [string]$Configuration = "Release",
  [switch]$KeepEnvironment
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$serviceRunner = Join-Path $repoRoot "..\\gateway\\.docker\\run-integration-tests.ps1"

if (-not (Test-Path $serviceRunner)) {
  throw "Gateway integration runner not found"
}

& $serviceRunner -Configuration $Configuration -KeepEnvironment:$KeepEnvironment



