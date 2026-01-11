# 🏷️ Docker Image Tagging Strategy

> **Epic 6 - PBI 6.2: Docker Build Automation**  
> **Last Updated:** January 10, 2026  
> **Status:** Active

---

## 📋 Table of Contents

- [Overview](#overview)
- [Tag Format Specification](#tag-format-specification)
- [Version Determination](#version-determination)
- [Workflow Examples](#workflow-examples)
- [Registry Structure](#registry-structure)
- [Best Practices](#best-practices)

---

## 🎯 Overview

This document defines the complete image tagging strategy for Electronic Paradise microservices, ensuring versioning consistency, traceability, and cost-effective registry management.

### **Goals:**

✅ **Traceability:** Every image links to exact Git commit  
✅ **Versioning:** Semantic versioning for production releases  
✅ **Cost Control:** Unpublished alpha images save registry storage  
✅ **Kubernetes Ready:** Clean, versioned images for deployment  
✅ **Rollback Capable:** Easy rollback to any previous version

---

## 🏗️ Tag Format Specification

### **Alpha Images (PR Builds)**

**Format:** `alpha-<semantic-version>-<git-sha>`

**Components:**
- `alpha`: Prefix indicating pre-release/testing image
- `<semantic-version>`: Next version based on branch type (e.g., `1.1.0`)
- `<git-sha>`: 7-character Git commit hash (e.g., `a1b2c3d`)

**Examples:**
```
alpha-1.0.1-a1b2c3d  (fix/chore branch)
alpha-1.1.0-e4f5g6h  (feat branch)
alpha-2.0.0-i7j8k9l  (breaking branch)
```

**Publishing:**
- **Default:** NOT published (exists only in CI cache)
- **Override:** Publish when `PUBLISH_ALPHA=true` (manual trigger)
- **Cleanup:** Auto-deleted after 7 days from CI cache

**Use Cases:**
- PR validation and testing
- Optional pre-merge validation in K8s
- Development/QA testing environments

---

### **Production Images (Main Branch)**

**Formats:** Three tags per image (all point to same image)

1. **Semantic Version:** `v<major>.<minor>.<patch>`
2. **Version with SHA:** `v<major>.<minor>.<patch>-<git-sha>`
3. **Latest Pointer:** `latest`

**Examples:**
```
v1.0.0           (primary version)
v1.0.0-x9y8z7w   (with Git SHA for traceability)
latest           (convenience pointer, always newest)
```

**Publishing:**
- **Always:** ALL three tags published to registry
- **Storage:** Only one image (243MB), three aliases

**Use Cases:**
- Production deployments
- Staging validation
- Rollback scenarios
- Audit trail and debugging

---

## 🔄 Version Determination

### **Branch-Based Semantic Versioning**

Version bumps are determined automatically based on branch naming:

| Branch Prefix | Version Bump | From → To | Description |
|---------------|--------------|-----------|-------------|
| `fix/*` | Patch (+0.0.1) | v1.0.0 → v1.0.1 | Bug fixes |
| `chore/*` | Patch (+0.0.1) | v1.0.0 → v1.0.1 | Maintenance, dependencies |
| `feat/*` | Minor (+0.1.0) | v1.0.0 → v1.1.0 | New features (backward compatible) |
| `breaking/*` | Major (+1.0.0) | v1.0.0 → v2.0.0 | Breaking changes |

### **Branch Naming Convention**

```bash
# Patch (x.x.+1)
fix/login-crash
fix/typo-in-error-message
fix/null-reference-exception
chore/update-dependencies
chore/cleanup-dead-code

# Minor (x.+1.0)
feat/add-2fa
feat/user-profile-page
feat/email-notifications
feat/product-wishlist

# Major (+1.0.0)
breaking/new-auth-system
breaking/remove-old-api-v1
breaking/database-schema-v2
breaking/require-https-only
```

### **Semantic Versioning Rules**

```
v1.0.0 → v1.0.1  Patch:   Bug fix, backward compatible
v1.0.0 → v1.1.0  Minor:   New feature, backward compatible
v1.0.0 → v2.0.0  Major:   Breaking change, NOT backward compatible
```

**Breaking Change Definition:**
- Changes that require client code updates
- API endpoint removals or signature changes
- Database schema changes requiring migration
- Configuration format changes
- Removal of deprecated features

---

## 📊 Workflow Examples

### **Example 1: Bug Fix (Patch)**

```bash
# Current production version
v1.0.0 running in production

# Developer creates fix branch
git checkout -b fix/login-timeout
# ... fix the bug ...
git commit -m "fix: resolve login timeout issue"
git push origin fix/login-timeout

# Create Pull Request → GitHub Actions CI triggers
# CI detects: fix/* branch → patch bump
# CI calculates: v1.0.0 + patch → v1.0.1
# CI builds and tags: alpha-1.0.1-abc123d
# CI publishes: NO (default behavior)
# Tests run: ✅ All pass

# Code review and approval
# Developer merges to main and tags
git checkout main
git pull origin main
git tag v1.0.1
git push origin main --tags

# CI on main branch triggers
# CI detects tag: v1.0.1
# CI builds once, creates 3 tags:
#   - v1.0.1
#   - v1.0.1-xyz789w (with SHA)
#   - latest (updated to point here)
# CI publishes: ✅ All three tags

# Deploy to production
kubectl set image deployment/auth-service \
  auth=ghcr.io/rahulsharma/electronic-paradise-auth:v1.0.1
```

---

### **Example 2: New Feature (Minor)**

```bash
# Current production
v1.0.1 running

# Developer creates feature branch
git checkout -b feat/add-2fa
# ... implement two-factor authentication ...
git commit -m "feat: add two-factor authentication support"
git push origin feat/add-2fa

# Create PR → CI triggers
# CI detects: feat/* branch → minor bump
# CI calculates: v1.0.1 + minor → v1.1.0
# CI builds: alpha-1.1.0-def456e
# CI publishes: NO (default)

# Push more commits
git commit -m "feat: add 2FA UI components"
git push origin feat/add-2fa
# New CI run: alpha-1.1.0-ghi789f (same base version, new SHA)

# QA wants to test before merge
# Rerun CI workflow with PUBLISH_ALPHA=true
# CI publishes: alpha-1.1.0-ghi789f → registry

# QA deploys to test cluster
kubectl set image deployment/auth-service \
  auth=ghcr.io/rahulsharma/electronic-paradise-auth:alpha-1.1.0-ghi789f
# ... QA validates ...
# ✅ QA approves

# Merge to main
git checkout main
git merge feat/add-2fa
git tag v1.1.0
git push origin main --tags

# CI creates production release
# Tags: v1.1.0, v1.1.0-jkl012m, latest
# All published ✅
```

---

### **Example 3: Breaking Change (Major)**

```bash
# Current production
v1.1.0 running

# Developer creates breaking change branch
git checkout -b breaking/new-jwt-format
# ... implement new JWT token structure ...
git commit -m "breaking: change JWT token to include user roles"
git push origin breaking/new-jwt-format

# Create PR → CI triggers
# CI detects: breaking/* branch → major bump
# CI calculates: v1.1.0 + major → v2.0.0
# CI builds: alpha-2.0.0-mno345p
# CI publishes: NO (default)

# IMPORTANT: Breaking changes need extensive testing
# Publish alpha for staging validation
# Rerun with PUBLISH_ALPHA=true
# CI publishes: alpha-2.0.0-mno345p

# Deploy to staging
kubectl set image deployment/auth-service \
  auth=ghcr.io/rahulsharma/electronic-paradise-auth:alpha-2.0.0-mno345p
# ... extensive testing in staging ...
# ... update all dependent services ...
# ✅ Staging validates successfully

# Code review and merge
git checkout main
git merge breaking/new-jwt-format
git tag v2.0.0
git push origin main --tags

# CI creates v2.0.0 release
# Tags: v2.0.0, v2.0.0-qrs678t, latest
# All published ✅

# Gradual rollout to production
# (blue-green deployment or canary release recommended)
```

---

### **Example 4: Hotfix Workflow**

```bash
# Production issue discovered
v1.1.0 has critical bug in production

# Create hotfix from main
git checkout main
git checkout -b fix/critical-payment-bug
# ... urgent fix ...
git commit -m "fix: critical payment processing error"
git push origin fix/critical-payment-bug

# Create PR → CI triggers
# CI detects: fix/* branch → patch bump
# CI builds: alpha-1.1.1-uvw901x
# Optional: Publish alpha for validation
# PUBLISH_ALPHA=true → alpha-1.1.1-uvw901x published

# Fast-track review (2 approvals for critical fixes)
# Merge to main
git checkout main
git merge fix/critical-payment-bug
git tag v1.1.1
git push origin main --tags

# CI creates v1.1.1 release
# Deploy immediately to production
kubectl set image deployment/payment-service \
  payment=ghcr.io/rahulsharma/electronic-paradise-payment:v1.1.1
```

---

## 🗄️ Registry Structure

### **GitHub Container Registry Layout**

```
ghcr.io/rahulsharma/electronic-paradise-auth/
├── v1.0.0           (243MB)
├── v1.0.0-abc123    (alias to v1.0.0)
├── v1.0.1           (243MB)
├── v1.0.1-def456    (alias to v1.0.1)
├── v1.1.0           (243MB)
├── v1.1.0-ghi789    (alias to v1.1.0)
├── v2.0.0           (243MB)
├── v2.0.0-jkl012    (alias to v2.0.0)
├── latest           (alias to v2.0.0) ← Always points to newest
└── alpha-1.2.0-mno345 (243MB) ← Only if published manually

Total Storage: ~972MB (4 unique images + 1 alpha)
Monthly Cost: ~$0.02 (GitHub Container Registry: $0.008/GB)
```

### **Storage Optimization**

**Without Alpha Publishing Control:**
```
100 commits/day × 5 services = 500 alpha images/day
500 × 243MB = 121.5GB/day
Monthly: ~3.6TB
Cost: ~$29/month (registry storage + bandwidth)
```

**With Alpha Publishing Control (Our Strategy):**
```
Unpublished alphas: Stored in CI cache only (7-day retention)
Published alphas: Only when PUBLISH_ALPHA=true (rare)
Typical: 5-10 production releases/month
Monthly Storage: ~2.4GB
Cost: ~$0.02/month
Savings: $28.98/month (99.9% reduction!) 💰
```

---

## 🎯 Best Practices

### **DO:**

✅ **Use specific versions in K8s production**
```yaml
image: ghcr.io/rahulsharma/electronic-paradise-auth:v1.1.0
```

✅ **Tag manually before merging to main**
```bash
git tag v1.1.0
git push origin main --tags
```

✅ **Use SHA tags for debugging**
```bash
# Find exact code from production issue
kubectl describe pod auth-xyz
# Image: v1.1.0-ghi789j
git checkout ghi789j  # Exact code!
```

✅ **Keep at least 5-10 versions for rollback**
```bash
# Easy rollback
kubectl set image deployment/auth auth=ghcr.io/.../auth:v1.0.5
```

✅ **Test staging with same image as production**
```bash
# Staging
kubectl set image deployment/auth auth=...:v1.1.0
# ... test ...
# Production (exact same image!)
kubectl set image deployment/auth auth=...:v1.1.0
```

---

### **DON'T:**

❌ **Don't use `latest` in production**
```yaml
# BAD - unpredictable, hard to rollback
image: ghcr.io/rahulsharma/electronic-paradise-auth:latest

# GOOD - explicit, traceable
image: ghcr.io/rahulsharma/electronic-paradise-auth:v1.1.0
```

❌ **Don't overwrite semantic version tags**
```bash
# BAD - breaks trust in versions
git tag v1.0.0  # Create
git tag -f v1.0.0  # Force overwrite (DON'T DO THIS!)

# v1.0.0 should always point to same commit!
```

❌ **Don't rebuild images for production**
```bash
# BAD - staging and prod differ
git checkout v1.1.0
docker build -t prod:v1.1.0 .  # Different build!

# GOOD - use exact image from registry
docker pull ghcr.io/.../auth:v1.1.0  # Same as staging
```

❌ **Don't skip version numbers**
```bash
# BAD - creates confusion
v1.0.0 → v1.0.2 (where's v1.0.1?)

# GOOD - sequential
v1.0.0 → v1.0.1 → v1.0.2
```

❌ **Don't publish all alphas (cost!)**
```bash
# BAD - expensive
Every commit → publish alpha → $$$

# GOOD - selective
Default: don't publish (CI cache only)
When needed: PUBLISH_ALPHA=true
```

---

## 🔧 Tools & Scripts

### **Local Scripts:**
- `scripts/get-next-version.sh` - Calculate next version from branch name
- `scripts/tag-images.ps1` - Tag local Docker images for testing

### **CI/CD:**
- `.github/workflows/docker-build.yml` - Automated image building and tagging
- `.github/workflows/docker-publish.yml` - Push images to registry

---

## 📚 References

- [Semantic Versioning 2.0.0](https://semver.org/)
- [Docker Tag Best Practices](https://docs.docker.com/engine/reference/commandline/tag/)
- [GitHub Container Registry Docs](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry)
- [Kubernetes Image Pull Policy](https://kubernetes.io/docs/concepts/containers/images/#image-pull-policy)

---

## 📊 Version History

| Date | Version | Changes |
|------|---------|---------|
| 2026-01-10 | 1.0.0 | Initial tagging strategy defined |

---

**Maintained by:** Engineering Team  
**Review Schedule:** Quarterly or when significant changes needed
