# Get Next Version Based on Branch Name
# Usage: .\get-next-version.ps1 -BranchName "feat/add-login"
# Output: 1.1.0

param(
    [string]$BranchName = "",
    [switch]$Verbose
)

# Get current branch if not provided
if ([string]::IsNullOrEmpty($BranchName)) {
    $BranchName = git rev-parse --abbrev-ref HEAD
}

# Get the latest tag from main branch
try {
    $LatestTag = git describe --tags --abbrev=0 2>$null
    if ([string]::IsNullOrEmpty($LatestTag)) {
        $LatestTag = "v0.0.0"
    }
} catch {
    $LatestTag = "v0.0.0"
}

# Remove 'v' prefix if present
$LatestVersion = $LatestTag.TrimStart('v')

# Parse current version
$VersionParts = $LatestVersion -split '\.'
$Major = [int]$VersionParts[0]
$Minor = [int]$VersionParts[1]
$Patch = [int]$VersionParts[2]

# Determine version bump based on branch prefix
if ($BranchName -match '^breaking/') {
    # Major bump: +1.0.0
    $Major++
    $Minor = 0
    $Patch = 0
    $BumpType = "major"
} elseif ($BranchName -match '^feat/') {
    # Minor bump: x.+1.0
    $Minor++
    $Patch = 0
    $BumpType = "minor"
} elseif ($BranchName -match '^(fix|chore|hotfix)/') {
    # Patch bump: x.x.+1
    $Patch++
    $BumpType = "patch"
} else {
    # Unknown branch type: default to patch
    $Patch++
    $BumpType = "patch"
    Write-Warning "Unknown branch type '$BranchName', defaulting to patch bump"
}

$NextVersion = "$Major.$Minor.$Patch"

# Output the next version
Write-Output $NextVersion

# Optionally output detailed info
if ($Verbose) {
    Write-Host ""
    Write-Host "Branch: $BranchName" -ForegroundColor Cyan
    Write-Host "Latest version: $LatestVersion" -ForegroundColor Yellow
    Write-Host "Bump type: $BumpType" -ForegroundColor Magenta
    Write-Host "Next version: $NextVersion" -ForegroundColor Green
}
