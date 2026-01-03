#!/bin/bash
# Bash script to build, pack, and publish Ep.Platform to GitHub Packages
# Usage: ./publish-platform.sh [version] [github-token]
# Example: ./publish-platform.sh 1.0.3 $GITHUB_TOKEN

set -e

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
PROJECT_FILE="$SCRIPT_DIR/Ep.Platform.csproj"
ARTIFACTS_DIR="$(dirname "$(dirname "$SCRIPT_DIR")")/.artifacts"

echo "========================================"
echo "Ep.Platform NuGet Package Publisher"
echo "========================================"
echo ""

# Read current version from csproj
CURRENT_VERSION=$(grep -oP '<Version>\K[^<]+' "$PROJECT_FILE" | head -1)
echo "Current version in csproj: $CURRENT_VERSION"

# Get new version
if [ -z "$1" ]; then
    echo ""
    echo "Enter the new version number (e.g., 1.0.3):"
    read -r VERSION
else
    VERSION="$1"
fi

if [ -z "$VERSION" ]; then
    echo "❌ Version cannot be empty!"
    exit 1
fi

# Validate version format
if ! [[ $VERSION =~ ^[0-9]+\.[0-9]+\.[0-9]+ ]]; then
    echo "❌ Invalid version format. Use semantic versioning (e.g., 1.0.3)"
    exit 1
fi

# Get GitHub token
if [ -z "$2" ]; then
    if [ -z "$GITHUB_TOKEN" ]; then
        echo ""
        echo "GitHub Personal Access Token required (with 'write:packages' scope)"
        echo "You can set it as: export GITHUB_TOKEN='your-token'"
        echo ""
        read -sp "Enter GitHub Token: " GITHUB_TOKEN
        echo ""
    fi
else
    GITHUB_TOKEN="$2"
fi

if [ -z "$GITHUB_TOKEN" ]; then
    echo "❌ GitHub token cannot be empty!"
    exit 1
fi

echo ""
echo "New version: $VERSION"
echo "Publishing to: https://nuget.pkg.github.com/RahulSharma2309"
echo ""

# Confirm
read -p "Continue? (y/N) " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo "Cancelled."
    exit 0
fi

echo ""
echo "Step 1: Updating version in Ep.Platform.csproj..."
sed -i.bak "s/<Version>[^<]*<\/Version>/<Version>$VERSION<\/Version>/" "$PROJECT_FILE"
rm -f "$PROJECT_FILE.bak"
echo "✅ Version updated to $VERSION"

echo ""
echo "Step 2: Creating artifacts directory..."
mkdir -p "$ARTIFACTS_DIR"
echo "✅ Artifacts directory ready"

echo ""
echo "Step 3: Building and packing Ep.Platform..."
dotnet pack "$PROJECT_FILE" -c Release -o "$ARTIFACTS_DIR"
echo "✅ Package created successfully"

NUPKG_FILE="$ARTIFACTS_DIR/Ep.Platform.$VERSION.nupkg"
if [ ! -f "$NUPKG_FILE" ]; then
    echo "❌ Package file not found: $NUPKG_FILE"
    exit 1
fi

echo ""
echo "Step 4: Publishing to GitHub Packages..."
dotnet nuget push "$NUPKG_FILE" \
    --source "https://nuget.pkg.github.com/RahulSharma2309/index.json" \
    --api-key "$GITHUB_TOKEN"

echo ""
echo "========================================"
echo "✅ Successfully published Ep.Platform v$VERSION!"
echo "========================================"
echo ""
echo "Next steps:"
echo "1. Update services to use version $VERSION"
echo "2. Update Directory.Build.props in each service:"
echo "   <EpPlatformVersion Condition=\"'\$(EpPlatformVersion)' == ''\">$VERSION</EpPlatformVersion>"
echo ""
echo "Package location: https://github.com/RahulSharma2309?tab=packages"
echo ""

