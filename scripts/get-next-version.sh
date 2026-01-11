#!/bin/bash
# Get Next Version Based on Branch Name
# Usage: ./get-next-version.sh <branch-name>
# Example: ./get-next-version.sh feat/add-login → 1.1.0

set -e

BRANCH_NAME="${1:-$(git rev-parse --abbrev-ref HEAD)}"

# Get the latest tag from main branch
LATEST_TAG=$(git describe --tags --abbrev=0 2>/dev/null || echo "v0.0.0")
# Remove 'v' prefix if present
LATEST_VERSION="${LATEST_TAG#v}"

# Parse current version
IFS='.' read -r MAJOR MINOR PATCH <<< "$LATEST_VERSION"

# Determine version bump based on branch prefix
if [[ "$BRANCH_NAME" == breaking/* ]]; then
    # Major bump: x.0.0
    MAJOR=$((MAJOR + 1))
    MINOR=0
    PATCH=0
    BUMP_TYPE="major"
elif [[ "$BRANCH_NAME" == feat/* ]]; then
    # Minor bump: x.y.0
    MINOR=$((MINOR + 1))
    PATCH=0
    BUMP_TYPE="minor"
elif [[ "$BRANCH_NAME" == fix/* ]] || [[ "$BRANCH_NAME" == chore/* ]]; then
    # Patch bump: x.y.z
    PATCH=$((PATCH + 1))
    BUMP_TYPE="patch"
elif [[ "$BRANCH_NAME" == hotfix/* ]]; then
    # Hotfix: treat as patch
    PATCH=$((PATCH + 1))
    BUMP_TYPE="patch"
else
    # Unknown branch type: default to patch
    PATCH=$((PATCH + 1))
    BUMP_TYPE="patch"
    echo "Warning: Unknown branch type '$BRANCH_NAME', defaulting to patch bump" >&2
fi

NEXT_VERSION="${MAJOR}.${MINOR}.${PATCH}"

# Output results (can be parsed by CI)
echo "$NEXT_VERSION"

# Optionally output detailed info to stderr
if [[ "${VERBOSE:-false}" == "true" ]]; then
    echo "Branch: $BRANCH_NAME" >&2
    echo "Latest version: $LATEST_VERSION" >&2
    echo "Bump type: $BUMP_TYPE" >&2
    echo "Next version: $NEXT_VERSION" >&2
fi
