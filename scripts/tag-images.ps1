# Tag Docker Images for Electronic Paradise
# Usage: .\tag-images.ps1 -Version "1.1.0" -Registry "ghcr.io/rahulsharma" [-Alpha] [-Push]

param(
    [Parameter(Mandatory=$false)]
    [string]$Version = "",
    
    [Parameter(Mandatory=$false)]
    [string]$Registry = "ghcr.io/rahulsharma",
    
    [switch]$Alpha,
    [switch]$Push,
    [switch]$Verbose
)

# Services to tag
$Services = @(
    "auth-service",
    "user-service",
    "product-service",
    "order-service",
    "payment-service",
    "gateway",
    "frontend"
)

# Get Git information
$GitSHA = (git rev-parse --short HEAD).Trim()
$BranchName = (git rev-parse --abbrev-ref HEAD).Trim()

# Determine version if not provided
if ([string]::IsNullOrEmpty($Version)) {
    if ($Alpha) {
        # For alpha, calculate next version
        $Version = & "$PSScriptRoot\get-next-version.ps1" -BranchName $BranchName
    } else {
        # For production, use latest Git tag
        try {
            $LatestTag = git describe --tags --abbrev=0 2>$null
            $Version = $LatestTag.TrimStart('v')
        } catch {
            Write-Error "No Git tag found. Please create a tag (e.g., 'git tag v1.0.0') or provide -Version parameter"
            exit 1
        }
    }
}

Write-Host "=== Docker Image Tagging ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Version: $Version" -ForegroundColor Green
Write-Host "Git SHA: $GitSHA" -ForegroundColor Yellow
Write-Host "Registry: $Registry" -ForegroundColor Magenta
Write-Host "Alpha: $Alpha" -ForegroundColor $(if ($Alpha) { "Yellow" } else { "Gray" })
Write-Host "Push: $Push" -ForegroundColor $(if ($Push) { "Red" } else { "Gray" })
Write-Host ""

$TaggedCount = 0
$ErrorCount = 0

foreach ($Service in $Services) {
    $LocalImage = "infra-$Service"
    $RepoName = "electronic-paradise-" + $Service.Replace("-service", "")
    
    Write-Host "Processing: $Service" -ForegroundColor Cyan
    
    # Check if local image exists
    $ImageExists = docker images --format "{{.Repository}}" | Select-String -Pattern "^$LocalImage$" -Quiet
    
    if (-not $ImageExists) {
        Write-Warning "  ⚠️  Local image '$LocalImage:latest' not found. Skipping."
        $ErrorCount++
        continue
    }
    
    if ($Alpha) {
        # Alpha tagging: alpha-<version>-<sha>
        $AlphaTag = "alpha-$Version-$GitSHA"
        $FullTag = "$Registry/${RepoName}:$AlphaTag"
        
        Write-Host "  → Tagging: $AlphaTag" -ForegroundColor Yellow
        docker tag "${LocalImage}:latest" $FullTag
        
        if ($LASTEXITCODE -eq 0) {
            $TaggedCount++
            if ($Verbose) {
                Write-Host "    ✅ Tagged: $FullTag" -ForegroundColor Green
            }
            
            if ($Push) {
                Write-Host "  → Pushing: $AlphaTag" -ForegroundColor Red
                docker push $FullTag
                if ($LASTEXITCODE -ne 0) {
                    Write-Error "    ❌ Failed to push: $FullTag"
                    $ErrorCount++
                }
            }
        } else {
            Write-Error "    ❌ Failed to tag: $FullTag"
            $ErrorCount++
        }
        
    } else {
        # Production tagging: v<version>, v<version>-<sha>, latest
        $Tags = @(
            "v$Version",
            "v$Version-$GitSHA",
            "latest"
        )
        
        foreach ($Tag in $Tags) {
            $FullTag = "$Registry/${RepoName}:$Tag"
            
            Write-Host "  → Tagging: $Tag" -ForegroundColor Yellow
            docker tag "${LocalImage}:latest" $FullTag
            
            if ($LASTEXITCODE -eq 0) {
                $TaggedCount++
                if ($Verbose) {
                    Write-Host "    ✅ Tagged: $FullTag" -ForegroundColor Green
                }
                
                if ($Push) {
                    Write-Host "  → Pushing: $Tag" -ForegroundColor Red
                    docker push $FullTag
                    if ($LASTEXITCODE -ne 0) {
                        Write-Error "    ❌ Failed to push: $FullTag"
                        $ErrorCount++
                    }
                }
            } else {
                Write-Error "    ❌ Failed to tag: $FullTag"
                $ErrorCount++
            }
        }
    }
    
    Write-Host ""
}

# Summary
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Successfully tagged: $TaggedCount tags" -ForegroundColor Green
if ($ErrorCount -gt 0) {
    Write-Host "Errors: $ErrorCount" -ForegroundColor Red
}

if ($Push) {
    Write-Host ""
    Write-Host "Images pushed to registry: $Registry" -ForegroundColor Magenta
} else {
    Write-Host ""
    Write-Host "To push images to registry, run with -Push flag" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Done! 🚀" -ForegroundColor Green
