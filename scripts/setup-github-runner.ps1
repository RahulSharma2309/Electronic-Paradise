# GitHub Actions Self-Hosted Runner Setup Script
# This script helps you set up a self-hosted runner for GitHub Actions

param(
    [string]$RunnerName = "docker-desktop-runner",
    [string]$InstallLocation = "$env:USERPROFILE\actions-runner",
    [switch]$InstallAsService = $false
)

Write-Host "🚀 GitHub Actions Self-Hosted Runner Setup" -ForegroundColor Green
Write-Host "==========================================`n" -ForegroundColor Green

# Check if running as administrator
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if ($InstallAsService -and -not $isAdmin) {
    Write-Host "❌ Administrator privileges required to install as service" -ForegroundColor Red
    Write-Host "Please run PowerShell as Administrator" -ForegroundColor Yellow
    exit 1
}

# Step 1: Get repository information
Write-Host "Step 1: Repository Information" -ForegroundColor Cyan
Write-Host "Please provide your GitHub repository details:" -ForegroundColor Yellow

$repoUrl = Read-Host "GitHub repository URL (e.g., https://github.com/username/repo)"
if ([string]::IsNullOrWhiteSpace($repoUrl)) {
    Write-Host "❌ Repository URL is required" -ForegroundColor Red
    exit 1
}

# Extract owner and repo from URL
if ($repoUrl -match "github\.com/([^/]+)/([^/]+)") {
    $owner = $matches[1]
    $repo = $matches[2].Replace(".git", "")
    Write-Host "✅ Repository: $owner/$repo" -ForegroundColor Green
} else {
    Write-Host "❌ Invalid repository URL format" -ForegroundColor Red
    exit 1
}

# Step 2: Create directory
Write-Host "`nStep 2: Creating runner directory..." -ForegroundColor Cyan
if (Test-Path $InstallLocation) {
    Write-Host "⚠️ Directory already exists: $InstallLocation" -ForegroundColor Yellow
    $overwrite = Read-Host "Do you want to remove it and start fresh? (y/N)"
    if ($overwrite -eq "y" -or $overwrite -eq "Y") {
        Remove-Item -Path $InstallLocation -Recurse -Force
        Write-Host "✅ Removed existing directory" -ForegroundColor Green
    } else {
        Write-Host "❌ Setup cancelled" -ForegroundColor Red
        exit 1
    }
}

New-Item -ItemType Directory -Path $InstallLocation -Force | Out-Null
Write-Host "✅ Created directory: $InstallLocation" -ForegroundColor Green

# Step 3: Download runner
Write-Host "`nStep 3: Downloading GitHub Actions Runner..." -ForegroundColor Cyan

# Get latest runner version
$latestVersion = "2.311.0"  # Update this to latest version
$runnerUrl = "https://github.com/actions/runner/releases/download/v$latestVersion/actions-runner-win-x64-$latestVersion.zip"
$zipFile = Join-Path $InstallLocation "actions-runner-win-x64-$latestVersion.zip"

try {
    Write-Host "Downloading from: $runnerUrl" -ForegroundColor Yellow
    Invoke-WebRequest -Uri $runnerUrl -OutFile $zipFile -UseBasicParsing
    Write-Host "✅ Download complete" -ForegroundColor Green
} catch {
    Write-Host "❌ Failed to download runner: $_" -ForegroundColor Red
    exit 1
}

# Step 4: Extract runner
Write-Host "`nStep 4: Extracting runner..." -ForegroundColor Cyan
try {
    Expand-Archive -Path $zipFile -DestinationPath $InstallLocation -Force
    Remove-Item -Path $zipFile -Force
    Write-Host "✅ Extraction complete" -ForegroundColor Green
} catch {
    Write-Host "❌ Failed to extract runner: $_" -ForegroundColor Red
    exit 1
}

# Step 5: Configure runner
Write-Host "`nStep 5: Configure Runner" -ForegroundColor Cyan
Write-Host "You need to get a registration token from GitHub:" -ForegroundColor Yellow
Write-Host "1. Go to: https://github.com/$owner/$repo/settings/actions/runners/new" -ForegroundColor Cyan
Write-Host "2. Copy the registration token" -ForegroundColor Cyan
Write-Host "3. Paste it below`n" -ForegroundColor Cyan

$token = Read-Host "Registration token" -AsSecureString
$tokenPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [Runtime.InteropServices.Marshal]::SecureStringToBSTR($token)
)

if ([string]::IsNullOrWhiteSpace($tokenPlain)) {
    Write-Host "❌ Token is required" -ForegroundColor Red
    exit 1
}

# Run config
Write-Host "`nConfiguring runner..." -ForegroundColor Yellow
Push-Location $InstallLocation

try {
    $configArgs = @(
        "--url", "https://github.com/$owner/$repo",
        "--token", $tokenPlain,
        "--name", $RunnerName,
        "--work", "_work"
    )
    
    if ($InstallAsService) {
        $configArgs += "--runasservice"
    }
    
    & .\config.cmd $configArgs
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Runner configured successfully" -ForegroundColor Green
    } else {
        Write-Host "❌ Runner configuration failed" -ForegroundColor Red
        Pop-Location
        exit 1
    }
} catch {
    Write-Host "❌ Configuration error: $_" -ForegroundColor Red
    Pop-Location
    exit 1
}

Pop-Location

# Step 6: Install as service (if requested)
if ($InstallAsService) {
    Write-Host "`nStep 6: Installing as Windows Service..." -ForegroundColor Cyan
    Push-Location $InstallLocation
    
    try {
        & .\svc.cmd install
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Service installed" -ForegroundColor Green
            
            & .\svc.cmd start
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✅ Service started" -ForegroundColor Green
            } else {
                Write-Host "⚠️ Service installed but failed to start" -ForegroundColor Yellow
            }
        } else {
            Write-Host "❌ Service installation failed" -ForegroundColor Red
        }
    } catch {
        Write-Host "❌ Service installation error: $_" -ForegroundColor Red
    }
    
    Pop-Location
} else {
    Write-Host "`nStep 6: Manual Start" -ForegroundColor Cyan
    Write-Host "To start the runner manually, run:" -ForegroundColor Yellow
    Write-Host "  cd $InstallLocation" -ForegroundColor Cyan
    Write-Host "  .\run.cmd" -ForegroundColor Cyan
}

# Summary
Write-Host "`n✅ Setup Complete!" -ForegroundColor Green
Write-Host "==================`n" -ForegroundColor Green
Write-Host "Runner Name: $RunnerName" -ForegroundColor Yellow
Write-Host "Location: $InstallLocation" -ForegroundColor Yellow
Write-Host "Repository: $owner/$repo" -ForegroundColor Yellow
Write-Host "Service: $(if ($InstallAsService) { 'Installed' } else { 'Manual' })" -ForegroundColor Yellow

Write-Host "`nNext Steps:" -ForegroundColor Cyan
Write-Host "1. Verify runner appears in GitHub: https://github.com/$owner/$repo/settings/actions/runners" -ForegroundColor White
Write-Host "2. Test with a workflow that uses 'runs-on: self-hosted'" -ForegroundColor White
Write-Host "3. Monitor runner status in GitHub Actions" -ForegroundColor White

if (-not $InstallAsService) {
    Write-Host "`n⚠️ Remember to start the runner before running workflows:" -ForegroundColor Yellow
    Write-Host "   cd $InstallLocation && .\run.cmd" -ForegroundColor Cyan
}

Write-Host "`nFor more information, see: docs/6-ci-cd/SELF_HOSTED_RUNNER_SETUP.md" -ForegroundColor Gray
