# Centralized Version Management

## Overview

All package versions and framework settings are centralized in `dependencies.props` files. This ensures:

- **Single source of truth** for all versions
- **Easy upgrades** - change version in one place
- **Consistency** across all projects in a service
- **No version conflicts**

## File Structure

Each service has a `.build/` folder containing:

```
services/<service-name>/src/.build/
├── dependencies.props      ← 🎯 ALL VERSIONS DEFINED HERE
├── src.props              ← Uses versions from dependencies.props
├── test.props             ← Uses versions from dependencies.props
├── stylecop.json          ← StyleCop configuration
└── stylecop.ruleset       ← Code analysis rules
```

## dependencies.props - The Single Source of Truth

### What's Defined Here

All versions and framework settings are defined in `dependencies.props`:

#### Framework Settings

```xml
<PropertyGroup Label="Framework Settings">
  <TargetFramework>net8.0</TargetFramework>
  <LangVersion>12</LangVersion>
  <Nullable>enable</Nullable>
  <ImplicitUsings>enable</ImplicitUsings>
</PropertyGroup>
```

#### Platform Packages

```xml
<PropertyGroup Label="Platform and Core Packages">
  <EpPlatformVersion>1.0.2</EpPlatformVersion>
  <AspNetCoreVersion>8.0.0</AspNetCoreVersion>
  <EntityFrameworkVersion>8.0.0</EntityFrameworkVersion>
</PropertyGroup>
```

#### Testing Packages

```xml
<PropertyGroup Label="Testing Packages">
  <XunitVersion>2.6.2</XunitVersion>
  <XunitRunnerVersion>2.6.2</XunitRunnerVersion>
  <MoqVersion>4.20.70</MoqVersion>
  <FluentAssertionsVersion>6.12.0</FluentAssertionsVersion>
  <CoverletVersion>6.0.0</CoverletVersion>
  <MicrosoftTestSdkVersion>17.8.0</MicrosoftTestSdkVersion>
  <AutoFixtureVersion>4.18.1</AutoFixtureVersion>
</PropertyGroup>
```

#### Code Quality

```xml
<PropertyGroup Label="Code Quality and Analysis">
  <StyleCopAnalyzersVersion>1.2.0-beta.556</StyleCopAnalyzersVersion>
  <JetBrainsAnnotationsVersion>2023.3.0</JetBrainsAnnotationsVersion>
</PropertyGroup>
```

#### Test Variables

```xml
<PropertyGroup Label="Additional Test Variables">
  <NetCoreVersion>net8.0</NetCoreVersion>
  <NullableReferences>enable</NullableReferences>
</PropertyGroup>
```

## How It Works

### 1. dependencies.props defines all versions

**Location**: `services/<service-name>/src/.build/dependencies.props`

This file contains ONLY version numbers and framework settings.

### 2. src.props imports dependencies.props

```xml
<Import Project="$(MSBuildThisFileDirectory)dependencies.props" />
```

Then uses the versions:

```xml
<PackageReference Include="StyleCop.Analyzers" Version="$(StyleCopAnalyzersVersion)" />
```

### 3. test.props imports dependencies.props

```xml
<Import Project="$(MSBuildThisFileDirectory)dependencies.props" />
```

Then uses the versions:

```xml
<PackageReference Include="xunit" Version="$(XunitVersion)" />
<PackageReference Include="Moq" Version="$(MoqVersion)" />
```

### 4. Projects reference via Directory.Build.props

**Source Projects** (`src/Directory.Build.props`):

```xml
<Import Project="$(MSBuildThisFileDirectory)../.build/src.props" />
```

**Test Projects** (`test/Directory.Build.props`):

```xml
<Import Project="$(MSBuildThisFileDirectory)../src/.build/test.props" />
```

## How to Upgrade Versions

### Upgrading .NET Version

**Edit ONE file**: `services/<service-name>/src/.build/dependencies.props`

```xml
<PropertyGroup Label="Framework Settings">
  <TargetFramework>net9.0</TargetFramework>  <!-- Change from net8.0 to net9.0 -->
  <LangVersion>13</LangVersion>              <!-- Update language version -->
</PropertyGroup>

<PropertyGroup Label="Additional Test Variables">
  <NetCoreVersion>net9.0</NetCoreVersion>    <!-- Update test variable -->
</PropertyGroup>
```

✅ **Result**: All projects in the service now use .NET 9.0

### Upgrading Package Versions

**Edit ONE file**: `services/<service-name>/src/.build/dependencies.props`

```xml
<PropertyGroup Label="Platform and Core Packages">
  <EntityFrameworkVersion>9.0.0</EntityFrameworkVersion>  <!-- Upgrade EF Core -->
</PropertyGroup>

<PropertyGroup Label="Testing Packages">
  <XunitVersion>2.7.0</XunitVersion>  <!-- Upgrade xUnit -->
  <MoqVersion>4.21.0</MoqVersion>     <!-- Upgrade Moq -->
</PropertyGroup>
```

✅ **Result**: All projects using these packages are upgraded

### Upgrading StyleCop

**Edit ONE file**: `services/<service-name>/src/.build/dependencies.props`

```xml
<PropertyGroup Label="Code Quality and Analysis">
  <StyleCopAnalyzersVersion>1.2.0-beta.600</StyleCopAnalyzersVersion>
</PropertyGroup>
```

✅ **Result**: All source projects use the new StyleCop version

## Verifying Centralization

### ❌ WRONG - Hardcoded Versions

```xml
<!-- DON'T DO THIS in .csproj or test.props -->
<PackageReference Include="xunit" Version="2.6.2" />
<TargetFramework>net8.0</TargetFramework>
```

### ✅ CORRECT - Using Variables

```xml
<!-- DO THIS - use variables from dependencies.props -->
<PackageReference Include="xunit" Version="$(XunitVersion)" />
<TargetFramework>$(TargetFramework)</TargetFramework>
```

## Service-Specific Versions

Each service has its own `dependencies.props`, allowing different services to use different versions if needed:

- **auth-service**: May use specific authentication packages
- **user-service**: Standard packages
- **order-service**: May include HTTP client packages
- **payment-service**: Payment-specific packages
- **product-service**: Standard packages

But within each service, ALL projects use the same versions from that service's `dependencies.props`.

## Benefits

1. **Upgrade Once, Apply Everywhere**

   - Change version in `dependencies.props`
   - All projects in that service are upgraded

2. **No Version Conflicts**

   - Impossible to have different projects using different versions
   - No dependency resolution issues

3. **Clear Documentation**

   - One file to see all versions used in a service
   - Easy to audit and review

4. **Easier Maintenance**

   - No need to hunt through multiple `.csproj` files
   - Consistent upgrade process

5. **CI/CD Friendly**
   - Automated version bumps only touch one file
   - Easy to validate versions in pipelines

## Example: Complete Version Upgrade

**Scenario**: Upgrade all services to use xUnit 2.7.0

**Steps**:

1. Edit `services/auth-service/src/.build/dependencies.props`
2. Edit `services/user-service/src/.build/dependencies.props`
3. Edit `services/order-service/src/.build/dependencies.props`
4. Edit `services/payment-service/src/.build/dependencies.props`
5. Edit `services/product-service/src/.build/dependencies.props`

In each file, change:

```xml
<XunitVersion>2.7.0</XunitVersion>
```

**Result**: All test projects across all services now use xUnit 2.7.0

## Validation

To verify centralization is working:

```bash
# 1. Check dependencies.props has all versions
cat services/auth-service/src/.build/dependencies.props

# 2. Verify no hardcoded versions in .csproj files
grep -r "Version=\"[0-9]" services/auth-service/src/ --include="*.csproj"
# Should only find project references and packages not in dependencies.props

# 3. Build to ensure all variables resolve
cd services/auth-service
dotnet build
```

## Summary

✅ **ALL versions** defined in `dependencies.props`  
✅ **NO hardcoded versions** in `src.props` or `test.props`  
✅ **Single file to upgrade** per service  
✅ **Consistent versions** across all projects in a service

---

**Last Updated**: January 2026  
**Status**: ✅ Implemented Across All Services






