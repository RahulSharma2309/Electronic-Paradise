# Testing Image Tagging Strategy

This guide helps you test the tagging scripts locally before implementing in CI/CD.

---

## Prerequisites

- Docker images built locally (run `docker-compose build` in `infra/` first)
- Git repository with at least one tag (we'll create `v1.0.0` if needed)

---

## Test 1: Version Calculation

### Test different branch types:

```powershell
# Test patch bump (fix)
.\scripts\get-next-version.ps1 -BranchName "fix/login-bug" -Verbose

# Expected output:
# Branch: fix/login-bug
# Latest version: 1.0.0
# Bump type: patch
# Next version: 1.0.1

# Test minor bump (feat)
.\scripts\get-next-version.ps1 -BranchName "feat/add-2fa" -Verbose

# Expected output:
# Branch: feat/add-2fa
# Latest version: 1.0.0
# Bump type: minor
# Next version: 1.1.0

# Test major bump (breaking)
.\scripts\get-next-version.ps1 -BranchName "breaking/new-auth" -Verbose

# Expected output:
# Branch: breaking/new-auth
# Latest version: 1.0.0
# Bump type: major
# Next version: 2.0.0
```

---

## Test 2: Alpha Tagging (Local Only)

### Create alpha tags without pushing:

```powershell
# Create a test branch
git checkout -b feat/test-tagging

# Tag as alpha (will calculate version from branch name)
.\scripts\tag-images.ps1 -Alpha -Verbose

# Expected output:
# Version: 1.1.0 (calculated from feat/* branch)
# Git SHA: abc1234 (your current commit)
# Alpha: Yellow
# Push: Gray
# 
# Processing: auth-service
#   → Tagging: alpha-1.1.0-abc1234
#   ✅ Tagged: ghcr.io/rahulsharma/electronic-paradise-auth:alpha-1.1.0-abc1234
# ...

# Verify tags were created
docker images | Select-String "alpha"
```

---

## Test 3: Production Tagging (Local Only)

### Create production tags without pushing:

```powershell
# Switch to main branch
git checkout main

# Create a version tag (if not exists)
git tag v1.0.0

# Tag as production version
.\scripts\tag-images.ps1 -Version "1.0.0" -Verbose

# Expected output:
# Version: 1.0.0
# Git SHA: xyz7890
# Alpha: Gray
# Push: Gray
#
# Processing: auth-service
#   → Tagging: v1.0.0
#   ✅ Tagged: ghcr.io/rahulsharma/electronic-paradise-auth:v1.0.0
#   → Tagging: v1.0.0-xyz7890
#   ✅ Tagged: ghcr.io/rahulsharma/electronic-paradise-auth:v1.0.0-xyz7890
#   → Tagging: latest
#   ✅ Tagged: ghcr.io/rahulsharma/electronic-paradise-auth:latest
# ...

# Verify tags were created
docker images | Select-String "ghcr.io"
```

---

## Test 4: Inspect Tagged Images

```powershell
# List all tagged images
docker images --format "table {{.Repository}}\t{{.Tag}}\t{{.ID}}" | Select-String "ghcr.io"

# Check that multiple tags point to same image ID
docker images --format "{{.Repository}}:{{.Tag}} = {{.ID}}" | Select-String "electronic-paradise-auth"

# Example output:
# ghcr.io/rahulsharma/electronic-paradise-auth:v1.0.0 = 0a58bd13545c
# ghcr.io/rahulsharma/electronic-paradise-auth:v1.0.0-xyz7890 = 0a58bd13545c (SAME ID!)
# ghcr.io/rahulsharma/electronic-paradise-auth:latest = 0a58bd13545c (SAME ID!)
```

---

## Test 5: Clean Up Test Tags

```powershell
# Remove test tags (keeps original infra-* images)
docker images --format "{{.Repository}}:{{.Tag}}" | Select-String "ghcr.io" | ForEach-Object {
    docker rmi $_
}

# Or remove specific service
docker rmi ghcr.io/rahulsharma/electronic-paradise-auth:alpha-1.1.0-abc1234
docker rmi ghcr.io/rahulsharma/electronic-paradise-auth:v1.0.0
docker rmi ghcr.io/rahulsharma/electronic-paradise-auth:v1.0.0-xyz7890
docker rmi ghcr.io/rahulsharma/electronic-paradise-auth:latest
```

---

## Expected Test Results

### ✅ Success Criteria:

1. **Version calculation works** for all branch types (fix, feat, breaking, chore)
2. **Alpha tags** include version + SHA
3. **Production tags** create 3 tags (version, version-sha, latest)
4. **Multiple tags point to same image ID** (no duplicate storage)
5. **Scripts don't push** without `-Push` flag (safe by default)

---

## Next Steps

After successful local testing:

1. **Phase 3:** Setup GitHub Container Registry authentication
2. **Phase 4:** Test pushing to registry manually
3. **Phase 5:** Create CI/CD workflow for automation

---

## Troubleshooting

### Issue: "No Git tag found"

**Solution:**
```powershell
git tag v1.0.0
git push origin v1.0.0
```

### Issue: "Local image not found"

**Solution:**
```powershell
cd infra
docker-compose build
```

### Issue: Script permission denied (Linux/Mac)

**Solution:**
```bash
chmod +x scripts/get-next-version.sh
```
