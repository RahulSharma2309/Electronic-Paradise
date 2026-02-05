<#
.SYNOPSIS
Copies product images from repo Images/ into frontend/public/ and seeds productdb with matching ProductImages URLs.

.DESCRIPTION
This script is designed for the local Docker setup in infra/docker-compose.yml.
It will:
  - Copy existing files under Images/images/** into frontend/public/product-images/** using slugged folders
  - Run EF Core migrations for ProductService to ensure productdb schema exists
  - Generate SQL seed script based on Images/mapping.csv (skipping missing files)
  - Execute that SQL inside the mssql container using sqlcmd (so you don't need sqlcmd installed locally)

.PARAMETER SaPassword
SQL Server SA password (default matches infra/docker-compose.yml).

.PARAMETER Database
Database name (default: productdb).

.PARAMETER ComposeFile
Path to docker-compose file used for local dev (default: infra/docker-compose.yml).
#>

[CmdletBinding()]
param(
  [string]$SaPassword = "Your_password123",
  [string]$Database = "productdb",
  [string]$ComposeFile = "infra/docker-compose.yml"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Get-RepoRoot {
  $here = Split-Path -Parent $PSCommandPath
  return (Resolve-Path (Join-Path $here "..")).Path
}

function To-Slug([string]$value) {
  if ([string]::IsNullOrWhiteSpace($value)) { return "" }
  $lower = $value.Trim().ToLowerInvariant()
  $chars = $lower.ToCharArray() | ForEach-Object {
    if ([char]::IsLetterOrDigit($_)) { $_ } else { '-' }
  }
  $slug = -join $chars
  while ($slug.Contains("--")) { $slug = $slug.Replace("--", "-") }
  return $slug.Trim('-')
}

function Escape-Sql([string]$value) {
  if ($null -eq $value) { return "" }
  return $value.Replace("'", "''")
}

function Get-DeterministicInt([string]$seed, [int]$min, [int]$max) {
  if ($max -lt $min) { throw "Invalid range: $min..$max" }
  $bytes = [System.Text.Encoding]::UTF8.GetBytes($seed)
  $hash = [System.Security.Cryptography.SHA256]::Create().ComputeHash($bytes)
  $n = [BitConverter]::ToUInt32($hash, 0)
  $span = ($max - $min + 1)
  return $min + [int]($n % [uint32]$span)
}

function Get-UnitForCategory([string]$categoryName) {
  $c = $categoryName.ToLowerInvariant()
  if ($c.Contains("vegetable")) { return "bunch" }
  if ($c.Contains("fruit")) { return "kg" }
  if ($c.Contains("grain")) { return "kg" }
  if ($c.Contains("legume")) { return "kg" }
  if ($c.Contains("nut")) { return "kg" }
  if ($c.Contains("herb")) { return "bunch" }
  if ($c.Contains("dairy")) { return "pack" }
  if ($c.Contains("beverage")) { return "bottle" }
  return "unit"
}

function Get-PriceForCategory([string]$categoryName, [string]$seed) {
  $c = $categoryName.ToLowerInvariant()
  if ($c.Contains("vegetable")) { return Get-DeterministicInt $seed 60 160 }
  if ($c.Contains("fruit")) { return Get-DeterministicInt $seed 80 220 }
  if ($c.Contains("grain")) { return Get-DeterministicInt $seed 40 140 }
  if ($c.Contains("legume")) { return Get-DeterministicInt $seed 60 180 }
  if ($c.Contains("nut")) { return Get-DeterministicInt $seed 300 950 }
  if ($c.Contains("herb")) { return Get-DeterministicInt $seed 20 90 }
  if ($c.Contains("dairy")) { return Get-DeterministicInt $seed 50 200 }
  if ($c.Contains("beverage")) { return Get-DeterministicInt $seed 100 380 }
  return Get-DeterministicInt $seed 50 250
}

function Get-StockForCategory([string]$categoryName, [string]$seed) {
  $c = $categoryName.ToLowerInvariant()
  if ($c.Contains("nut")) { return Get-DeterministicInt $seed 10 60 }
  if ($c.Contains("beverage")) { return Get-DeterministicInt $seed 10 80 }
  return Get-DeterministicInt $seed 15 120
}

$repoRoot = Get-RepoRoot

$mappingCsv = Join-Path $repoRoot "Images/mapping.csv"
$imagesRoot = Join-Path $repoRoot "Images/images"
$publicImagesRoot = Join-Path $repoRoot "frontend/public/product-images"
$sqlOut = Join-Path $repoRoot "scripts/seed-productdb-images.sql"

if (-not (Test-Path $mappingCsv)) { throw "mapping.csv not found at $mappingCsv" }
if (-not (Test-Path $imagesRoot)) { throw "Images root not found at $imagesRoot" }

New-Item -ItemType Directory -Force -Path $publicImagesRoot | Out-Null

Write-Host "Copying images into frontend/public/product-images/..."

$rows = Import-Csv -Path $mappingCsv
$usableRows = @()

foreach ($r in $rows) {
  $localPath = $r.LocalPath
  if ([string]::IsNullOrWhiteSpace($localPath)) { continue }
  $src = Join-Path (Join-Path $repoRoot "Images") $localPath
  if (-not (Test-Path $src)) {
    continue
  }

  $cat = $r.Category.Trim()
  $item = $r.Item.Trim()
  $catSlug = To-Slug $cat
  $itemSlug = To-Slug $item

  $fileName = Split-Path -Leaf $src
  $destDir = Join-Path (Join-Path $publicImagesRoot $catSlug) $itemSlug
  New-Item -ItemType Directory -Force -Path $destDir | Out-Null
  $dest = Join-Path $destDir $fileName

  Copy-Item -Force -Path $src -Destination $dest

  $usableRows += [pscustomobject]@{
    Category = $cat
    Item = $item
    CategorySlug = $catSlug
    ItemSlug = $itemSlug
    FileName = $fileName
    PublicUrl = "/product-images/$catSlug/$itemSlug/$fileName"
  }
}

if ($usableRows.Count -eq 0) {
  throw "No usable images found. Ensure Images/images/** exists and mapping.csv LocalPath points to real files."
}

Write-Host "Running EF Core migrations for ProductService (creates schema + $Database)..."

$productCoreProj = Join-Path $repoRoot "services/product-service/src/ProductService.Core/ProductService.Core.csproj"
$productApiProj = Join-Path $repoRoot "services/product-service/src/ProductService.API/ProductService.API.csproj"

$env:ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=$Database;User Id=sa;Password=$SaPassword;TrustServerCertificate=True;"

try {
  dotnet ef --version | Out-Null
} catch {
  Write-Host "dotnet-ef not found. Installing dotnet-ef as a global tool..."
  dotnet tool install --global dotnet-ef | Out-Host
  if ($LASTEXITCODE -ne 0) {
    throw "Failed to install dotnet-ef (exit code $LASTEXITCODE). Ensure .NET SDK is installed and try again."
  }
}

Write-Host "Building ProductService projects (required for migrations)..."
dotnet build $productApiProj | Out-Host
if ($LASTEXITCODE -ne 0) {
  throw "ProductService build failed (exit code $LASTEXITCODE). Run: dotnet build `"$productApiProj`" to see full errors."
}

dotnet ef database update --project $productCoreProj --startup-project $productApiProj | Out-Host
if ($LASTEXITCODE -ne 0) {
  throw "EF migration failed (exit code $LASTEXITCODE). Fix the build errors above and re-run the script."
}

Write-Host "Generating seed SQL at scripts/seed-productdb-images.sql..."

$now = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")

$categories = $usableRows | Select-Object Category, CategorySlug -Unique | Sort-Object CategorySlug
$products = $usableRows | Group-Object CategorySlug, ItemSlug | ForEach-Object {
  $first = $_.Group | Select-Object -First 1
  $cat = $first.Category
  $item = $first.Item
  $catSlug = $first.CategorySlug
  $itemSlug = $first.ItemSlug
  $seed = "$catSlug/$itemSlug"
  $price = Get-PriceForCategory $cat $seed
  $stock = Get-StockForCategory $cat $seed
  $unit = Get-UnitForCategory $cat

  [pscustomobject]@{
    Category = $cat
    CategorySlug = $catSlug
    Item = $item
    ItemSlug = $itemSlug
    ProductId = [Guid]::NewGuid().ToString()
    Name = $item
    Description = "Fresh, organically grown $item."
    Brand = "FreshHarvest Market Organics"
    Sku = ("EP-" + ($catSlug.Substring(0, [Math]::Min(3, $catSlug.Length))).ToUpperInvariant() + "-" + ($itemSlug.Substring(0, [Math]::Min(6, $itemSlug.Length))).ToUpperInvariant())
    Unit = $unit
    Price = $price
    Stock = $stock
    IsActive = 1
    Slug = "$catSlug-$itemSlug"
  }
} | Sort-Object CategorySlug, ItemSlug

$categoryIdBySlug = @{}
foreach ($c in $categories) {
  $categoryIdBySlug[$c.CategorySlug] = [Guid]::NewGuid().ToString()
}

$tagOrganicId = [Guid]::NewGuid().ToString()
$tagSeasonalId = [Guid]::NewGuid().ToString()
$tagFarmTrustId = [Guid]::NewGuid().ToString()

$sb = New-Object System.Text.StringBuilder

[void]$sb.AppendLine("SET NOCOUNT ON;")
[void]$sb.AppendLine("SET ANSI_NULLS ON;")
[void]$sb.AppendLine("SET QUOTED_IDENTIFIER ON;")
[void]$sb.AppendLine("SET ANSI_PADDING ON;")
[void]$sb.AppendLine("SET ANSI_WARNINGS ON;")
[void]$sb.AppendLine("SET CONCAT_NULL_YIELDS_NULL ON;")
[void]$sb.AppendLine("SET NUMERIC_ROUNDABORT OFF;")
[void]$sb.AppendLine("BEGIN TRY")
[void]$sb.AppendLine("  BEGIN TRANSACTION;")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("  -- Clean existing data (safe to rerun)")
[void]$sb.AppendLine("  DELETE FROM [ProductTags];")
[void]$sb.AppendLine("  DELETE FROM [ProductAttributes];")
[void]$sb.AppendLine("  DELETE FROM [ProductImages];")
[void]$sb.AppendLine("  DELETE FROM [ProductMetadata];")
[void]$sb.AppendLine("  DELETE FROM [ProductCertifications];")
[void]$sb.AppendLine("  DELETE FROM [Products];")
[void]$sb.AppendLine("  DELETE FROM [Tags];")
[void]$sb.AppendLine("  DELETE FROM [Categories];")
[void]$sb.AppendLine("")

[void]$sb.AppendLine("  -- Tags")
[void]$sb.AppendLine("  INSERT INTO [Tags] ([Id],[Name],[Slug]) VALUES")
[void]$sb.AppendLine("    ('$tagOrganicId','Organic','organic'),")
[void]$sb.AppendLine("    ('$tagSeasonalId','Seasonal','seasonal'),")
[void]$sb.AppendLine("    ('$tagFarmTrustId','Farm-trust','farm-trust');")
[void]$sb.AppendLine("")

[void]$sb.AppendLine("  -- Categories")
foreach ($c in $categories) {
  $cid = $categoryIdBySlug[$c.CategorySlug]
  $name = Escape-Sql $c.Category
  $slug = Escape-Sql $c.CategorySlug
  [void]$sb.AppendLine("  INSERT INTO [Categories] ([Id],[Name],[Slug],[Description],[ParentId],[IsActive],[CreatedAt],[UpdatedAt]) VALUES ('$cid','$name','$slug',NULL,NULL,1,SYSUTCDATETIME(),NULL);")
}
[void]$sb.AppendLine("")

[void]$sb.AppendLine("  -- Products + Metadata")
foreach ($p in $products) {
  $productId = $p.ProductId
  $cid = $categoryIdBySlug[$p.CategorySlug]
  $name = Escape-Sql $p.Name
  $desc = Escape-Sql $p.Description
  $brand = Escape-Sql $p.Brand
  $sku = Escape-Sql $p.Sku
  $unit = Escape-Sql $p.Unit
  $slug = Escape-Sql $p.Slug

  [void]$sb.AppendLine("  INSERT INTO [Products] ([Id],[Name],[Description],[Price],[Stock],[CategoryId],[Brand],[Sku],[Unit],[IsActive],[CreatedAt],[UpdatedAt])")
  [void]$sb.AppendLine("  VALUES ('$productId','$name','$desc',$($p.Price),$($p.Stock),'$cid','$brand','$sku','$unit',1,SYSUTCDATETIME(),NULL);")
  [void]$sb.AppendLine("")

  $metaId = [Guid]::NewGuid().ToString()
  $seoJson = Escape-Sql ("{""title"":""$name"",""description"":""$desc"",""keywords"":""$name,$($p.Category),organic,fresh"",""canonicalUrl"":""/products/$slug""}")

  [void]$sb.AppendLine("  INSERT INTO [ProductMetadata] ([Id],[ProductId],[Slug],[SeoMetadataJson],[CreatedAt],[UpdatedAt])")
  [void]$sb.AppendLine("  VALUES ('$metaId','$productId','$slug','$seoJson',SYSUTCDATETIME(),NULL);")
  [void]$sb.AppendLine("")

  [void]$sb.AppendLine("  -- Default tags for $name")
  [void]$sb.AppendLine("  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('$productId','$tagOrganicId');")
  [void]$sb.AppendLine("  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('$productId','$tagSeasonalId');")
  [void]$sb.AppendLine("  INSERT INTO [ProductTags] ([ProductId],[TagId]) VALUES ('$productId','$tagFarmTrustId');")
  [void]$sb.AppendLine("")
}

[void]$sb.AppendLine("  -- Product Images")
foreach ($p in $products) {
  $productId = $p.ProductId
  $imgs = $usableRows |
    Where-Object { $_.CategorySlug -eq $p.CategorySlug -and $_.ItemSlug -eq $p.ItemSlug } |
    Sort-Object FileName

  $sort = 0
  foreach ($img in $imgs) {
    $imgId = [Guid]::NewGuid().ToString()
    $url = Escape-Sql $img.PublicUrl
    $alt = Escape-Sql ("$($p.Name) image")
    $isPrimary = if ($sort -eq 0) { 1 } else { 0 }
    [void]$sb.AppendLine("  INSERT INTO [ProductImages] ([Id],[ProductId],[Url],[AltText],[SortOrder],[IsPrimary],[CreatedAt]) VALUES ('$imgId','$productId','$url','$alt',$sort,$isPrimary,SYSUTCDATETIME());")
    $sort++
  }
  [void]$sb.AppendLine("")
}

[void]$sb.AppendLine("  COMMIT TRANSACTION;")
[void]$sb.AppendLine("END TRY")
[void]$sb.AppendLine("BEGIN CATCH")
[void]$sb.AppendLine("  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;")
[void]$sb.AppendLine("  THROW;")
[void]$sb.AppendLine("END CATCH")

Set-Content -Path $sqlOut -Value $sb.ToString() -Encoding UTF8

Write-Host "Executing seed SQL inside the mssql container..."

# Ensure mssql container is up (sqlcmd is executed inside container).
docker compose -f $ComposeFile up -d mssql | Out-Host

$sqlcmd = "/opt/mssql-tools18/bin/sqlcmd"
$fallbackSqlcmd = "/opt/mssql-tools/bin/sqlcmd"

$sqlText = Get-Content -Path $sqlOut -Raw

try {
  # mssql-tools18 uses ODBC Driver 18, which enables encryption by default.
  # -C trusts the server certificate (dev/self-signed friendly).
  $sqlText | docker compose -f $ComposeFile exec -T mssql $sqlcmd -S localhost -U sa -P $SaPassword -d $Database -b -C -i /dev/stdin | Out-Host
} catch {
  Write-Warning "sqlcmd tools18 failed; trying legacy path..."
  $sqlText | docker compose -f $ComposeFile exec -T mssql $fallbackSqlcmd -S localhost -U sa -P $SaPassword -d $Database -b -i /dev/stdin | Out-Host
}

Write-Host ""
Write-Host "Done."
Write-Host "Images copied to: frontend/public/product-images/"
Write-Host "Seed SQL generated at: scripts/seed-productdb-images.sql"
Write-Host "Now (re)build frontend container so images are included in the image."
Write-Host "Suggested: docker compose -f $ComposeFile up -d --build product-service gateway frontend"

