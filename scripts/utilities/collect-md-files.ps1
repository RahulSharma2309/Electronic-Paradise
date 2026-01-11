# Collect All Markdown Files
# Utility script to list all .md files in the repository

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host " Collecting All MD Files" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$rootPath = $PSScriptRoot
if ([string]::IsNullOrEmpty($rootPath)) {
    $rootPath = Get-Location
}

# Go up two levels to repository root (from scripts/utilities/)
$rootPath = Split-Path (Split-Path $rootPath -Parent) -Parent

Write-Host "Searching in: $rootPath" -ForegroundColor Yellow
Write-Host ""

# Get all .md files recursively
$mdFiles = Get-ChildItem -Path $rootPath -Include *.md -Recurse -File | 
    Where-Object { $_.FullName -notmatch '\\node_modules\\' -and $_.FullName -notmatch '\\.git\\' }

if ($mdFiles.Count -eq 0) {
    Write-Warning "No .md files found."
    exit 0
}

Write-Host "Found $($mdFiles.Count) .md files:" -ForegroundColor Green
Write-Host ""

# Group by directory
$byDirectory = $mdFiles | Group-Object DirectoryName | Sort-Object Name

foreach ($dirGroup in $byDirectory) {
    $relativeDir = $dirGroup.Name.Replace($rootPath, ".").TrimStart('\')
    Write-Host "[$relativeDir]" -ForegroundColor Cyan
    
    foreach ($file in $dirGroup.Group | Sort-Object Name) {
        $size = "{0:N1} KB" -f ($file.Length / 1KB)
        Write-Host "  - $($file.Name) ($size)"
    }
    Write-Host ""
}

# Summary
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Summary:" -ForegroundColor Yellow
Write-Host "  Total .md files: $($mdFiles.Count)"
$totalSize = ($mdFiles | Measure-Object -Property Length -Sum).Sum
Write-Host "  Total size: $("{0:N1} KB" -f ($totalSize / 1KB))"
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Export list to file
$listFile = Join-Path $rootPath "md-files-list.txt"
$content = @()
$content += "Markdown Files in Electronic Paradise"
$content += "Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$content += "="*70
$content += ""

foreach ($dirGroup in $byDirectory) {
    $relativeDir = $dirGroup.Name.Replace($rootPath, ".").TrimStart('\')
    $content += "[$relativeDir]"
    
    foreach ($file in $dirGroup.Group | Sort-Object Name) {
        $relativePath = $file.FullName.Replace($rootPath, ".").TrimStart('\')
        $size = "{0:N1} KB" -f ($file.Length / 1KB)
        $content += "  $relativePath ($size)"
    }
    $content += ""
}

$content += "="*70
$content += "Total: $($mdFiles.Count) files"
$content += "Total size: $("{0:N1} KB" -f ($totalSize / 1KB))"

$content | Out-File -FilePath $listFile -Encoding UTF8

Write-Host "File list exported to: $listFile" -ForegroundColor Green
Write-Host ""
