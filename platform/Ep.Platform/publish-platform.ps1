# PowerShell script to build, pack, and publish Ep.Platform to GitHub Packages
# Usage: .\publish-platform.ps1 [version] [github-token]
# Example: .\publish-platform.ps1 1.0.3 $env:GITHUB_TOKEN

param(
    [Parameter(Mandatory=$false)]
    [string]$Version = "",
    
    [Parameter(Mandatory=$false)]
    [string]$GitHubToken = ""
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Ep.Platform NuGet Package Publisher" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Get the script directory
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectFile = Join-Path $ScriptDir "Ep.Platform.csproj"
$ArtifactsDir = Join-Path (Split-Path -Parent (Split-Path -Parent $ScriptDir)) ".artifacts"

# Read current version from csproj
$csprojContent = Get-Content $ProjectFile -Raw
$currentVersionMatch = [regex]::Match($csprojContent, '<Version>([^<]+)</Version>')
$currentVersion = $currentVersionMatch.Groups[1].Value

Write-Host "Current version in csproj: $currentVersion" -ForegroundColor Yellow

# Get new version
if ([string]::IsNullOrWhiteSpace($Version)) {
    Write-Host ""
    Write-Host "Enter the new version number (e.g., 1.0.3):" -ForegroundColor Green
    $Version = Read-Host "Version"
}

if ([string]::IsNullOrWhiteSpace($Version)) {
    Write-Host "❌ Version cannot be empty!" -ForegroundColor Red
    exit 1
}

# Validate version format (basic check)
if ($Version -notmatch '^\d+\.\d+\.\d+') {
    Write-Host "❌ Invalid version format. Use semantic versioning (e.g., 1.0.3)" -ForegroundColor Red
    exit 1
}

# Get GitHub token
if ([string]::IsNullOrWhiteSpace($GitHubToken)) {
    # Try to get from environment variable
    $GitHubToken = $env:GITHUB_TOKEN
    
    if ([string]::IsNullOrWhiteSpace($GitHubToken)) {
        Write-Host ""
        Write-Host "GitHub Personal Access Token required (with 'write:packages' scope)" -ForegroundColor Yellow
        Write-Host "You can set it as: `$env:GITHUB_TOKEN = 'your-token'" -ForegroundColor Gray
        Write-Host ""
        # Read as plain string (will be visible in process list when passed to dotnet, so SecureString doesn't add much security)
        $GitHubToken = Read-Host "Enter GitHub Token"
    }
}

if ([string]::IsNullOrWhiteSpace($GitHubToken)) {
    Write-Host "❌ GitHub token cannot be empty!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "New version: $Version" -ForegroundColor Green
Write-Host "Publishing to: https://nuget.pkg.github.com/RahulSharma2309" -ForegroundColor Green
Write-Host ""

# Confirm
$confirm = Read-Host "Continue? (y/N)"
if ($confirm -ne "y" -and $confirm -ne "Y") {
    Write-Host "Cancelled." -ForegroundColor Yellow
    exit 0
}

Write-Host ""
Write-Host "Step 1: Updating version in Ep.Platform.csproj..." -ForegroundColor Cyan
$csprojContent = $csprojContent -replace '<Version>([^<]+)</Version>', "<Version>$Version</Version>"
$csprojContent | Set-Content $ProjectFile -NoNewline
Write-Host "✅ Version updated to $Version" -ForegroundColor Green

Write-Host ""
Write-Host "Step 2: Creating artifacts directory..." -ForegroundColor Cyan
if (-not (Test-Path $ArtifactsDir)) {
    New-Item -ItemType Directory -Path $ArtifactsDir -Force | Out-Null
}
Write-Host "✅ Artifacts directory ready" -ForegroundColor Green

Write-Host ""
Write-Host "Step 3: Building and packing Ep.Platform..." -ForegroundColor Cyan
$packResult = dotnet pack $ProjectFile -c Release -o $ArtifactsDir
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Pack failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Package created successfully" -ForegroundColor Green

$nupkgFile = Join-Path $ArtifactsDir "Ep.Platform.$Version.nupkg"
if (-not (Test-Path $nupkgFile)) {
    Write-Host "❌ Package file not found: $nupkgFile" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Step 4: Publishing to GitHub Packages..." -ForegroundColor Cyan
$publishResult = dotnet nuget push $nupkgFile --source "https://nuget.pkg.github.com/RahulSharma2309/index.json" --api-key $GitHubToken
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Publish failed!" -ForegroundColor Red
    Write-Host "Check your GitHub token has 'write:packages' scope" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ Successfully published Ep.Platform v$Version!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Update services to use version $Version" -ForegroundColor White
Write-Host "2. Update Directory.Build.props in each service:" -ForegroundColor White
Write-Host "   <EpPlatformVersion Condition=`"'`$(EpPlatformVersion)' == ''`">$Version</EpPlatformVersion>" -ForegroundColor Gray
Write-Host ""
Write-Host "Package location: https://github.com/RahulSharma2309?tab=packages" -ForegroundColor Cyan
Write-Host ""

