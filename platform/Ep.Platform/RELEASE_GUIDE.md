# Ep.Platform Release Guide

This guide walks you through releasing a new version of the Ep.Platform NuGet package to GitHub Packages.

## Prerequisites

1. **GitHub Personal Access Token** with `write:packages` scope
   - Go to: https://github.com/settings/tokens
   - Generate new token (classic)
   - Select `write:packages` scope
   - Copy the token

2. **.NET SDK 8.0** installed

## Quick Release (Automated)

### Windows (PowerShell)

```powershell
cd platform/Ep.Platform
.\publish-platform.ps1
```

The script will:
1. Show current version
2. Ask for new version (e.g., `1.0.3`)
3. Ask for GitHub token (or use `$env:GITHUB_TOKEN`)
4. Update version in `.csproj`
5. Build and pack the package
6. Publish to GitHub Packages

### Linux/Mac (Bash)

```bash
cd platform/Ep.Platform
chmod +x publish-platform.sh
./publish-platform.sh
```

Or with parameters:
```bash
./publish-platform.sh 1.0.3 $GITHUB_TOKEN
```

## Manual Release Steps

If you prefer to do it manually:

### Step 1: Update Version

Edit `platform/Ep.Platform/Ep.Platform.csproj`:

```xml
<Version>1.0.3</Version>  <!-- Change this -->
```

### Step 2: Build and Pack

```powershell
cd platform/Ep.Platform
dotnet pack -c Release -o ../../.artifacts
```

This creates: `.artifacts/Ep.Platform.1.0.3.nupkg`

### Step 3: Publish to GitHub Packages

```powershell
dotnet nuget push .\.artifacts\Ep.Platform.1.0.3.nupkg `
    --source "https://nuget.pkg.github.com/RahulSharma2309/index.json" `
    --api-key "your-github-token"
```

**Linux/Mac:**
```bash
dotnet nuget push ./.artifacts/Ep.Platform.1.0.3.nupkg \
    --source "https://nuget.pkg.github.com/RahulSharma2309/index.json" \
    --api-key "$GITHUB_TOKEN"
```

## Step 4: Update Services to Use New Version

After publishing, update all services to use the new version:

### Update Directory.Build.props

In each service's `src/Directory.Build.props`:

```xml
<EpPlatformVersion Condition="'$(EpPlatformVersion)' == ''">1.0.3</EpPlatformVersion>
```

**Services to update:**
- `services/auth-service/src/Directory.Build.props`
- `services/user-service/src/Directory.Build.props`
- `services/product-service/src/Directory.Build.props`
- `services/payment-service/src/Directory.Build.props`
- `services/order-service/src/Directory.Build.props`

### Verify the Update

```powershell
# Test restore in one service
cd services/user-service/src
dotnet restore UserService.API/UserService.API.csproj
```

## Version Numbering

Follow [Semantic Versioning](https://semver.org/):

- **MAJOR.MINOR.PATCH** (e.g., `1.0.3`)
- **MAJOR**: Breaking changes
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes (backward compatible)

**Examples:**
- `1.0.1` → `1.0.2` (bug fix)
- `1.0.2` → `1.1.0` (new feature)
- `1.1.0` → `2.0.0` (breaking change)

## Troubleshooting

### Error: "Unable to find package"

- Wait a few minutes after publishing (GitHub Packages can take 1-2 minutes to index)
- Verify the version number matches exactly
- Check your GitHub token has `read:packages` scope

### Error: "401 Unauthorized"

- Verify your GitHub token has `write:packages` scope
- Check the token hasn't expired
- Ensure you're using the correct GitHub username in the source URL

### Error: "Package already exists"

- Version numbers must be unique
- Increment the version number
- You cannot overwrite an existing version

## Verifying the Release

1. **Check GitHub Packages:**
   - Go to: https://github.com/RahulSharma2309?tab=packages
   - Find `Ep.Platform`
   - Verify the new version is listed

2. **Test in a Service:**
   ```powershell
   cd services/user-service/src
   dotnet restore UserService.API/UserService.API.csproj
   ```
   Should download the new version without errors.

## After Publishing

1. ✅ Update all services' `Directory.Build.props` files
2. ✅ Test restore in at least one service
3. ✅ Commit the version changes
4. ✅ Update Docker builds (they'll automatically use the new version)

---

**Quick Reference:**

```powershell
# Windows: Publish new version
cd platform/Ep.Platform
.\publish-platform.ps1

# Then update services
# Edit: services/*/src/Directory.Build.props
# Change: <EpPlatformVersion>1.0.3</EpPlatformVersion>
```
















