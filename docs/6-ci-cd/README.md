# 🚀 CI/CD Documentation

This folder contains all documentation related to Continuous Integration and Continuous Deployment (CI/CD) for Electronic Paradise.

---

## 📚 Documentation Index

### **🏷️ Image Tagging & Versioning**

| Document | Purpose | Status |
|----------|---------|--------|
| [IMAGE_TAGGING_STRATEGY.md](./IMAGE_TAGGING_STRATEGY.md) | Complete tagging specification | ✅ Complete |
| [TAGGING_QUICK_REFERENCE.md](./TAGGING_QUICK_REFERENCE.md) | Quick command reference | ✅ Complete |
| [TESTING_IMAGE_TAGGING.md](./TESTING_IMAGE_TAGGING.md) | Testing guide | ✅ Complete |
| [PHASE_2_COMPLETE.md](./PHASE_2_COMPLETE.md) | Phase 2 completion summary | ✅ Complete |

### **🔧 Scripts** (in `/scripts` folder)

| Script | Purpose | Platform |
|--------|---------|----------|
| `get-next-version.ps1` | Calculate next version from branch | Windows |
| `get-next-version.sh` | Calculate next version from branch | Linux/Mac/CI |
| `tag-images.ps1` | Tag Docker images | Windows |

---

## 🎯 Quick Start

### **Test Locally (Before CI):**

```powershell
# 1. Calculate version for your branch
.\scripts\get-next-version.ps1 -BranchName "feat/add-2fa" -Verbose

# Output shows:
#   Branch: feat/add-2fa
#   Latest version: 1.0.0
#   Bump type: minor
#   Next version: 1.1.0

# 2. Tag images locally (no push)
.\scripts\tag-images.ps1 -Alpha -Verbose

# Output: Tags all 7 services with alpha-1.1.0-abc123d

# 3. Verify tags
docker images | Select-String "alpha"
```

### **How CI Will Use Scripts (Automated):**

```yaml
# When implemented in .github/workflows/ci.yml:
- name: Calculate Version
  run: ./scripts/get-next-version.sh "$BRANCH"  # ← Calls script
  
- name: Build Images
  run: docker build -t app:alpha-$VERSION-$SHA .  # ← Uses output
```

### **Key Difference:**

| Aspect | Local Testing | CI Automation |
|--------|---------------|---------------|
| **Who runs it** | You manually | GitHub Actions automatically |
| **When** | Before pushing code | After pushing code |
| **Script used** | `.ps1` (Windows) | `.sh` (Linux) |
| **Purpose** | Validate logic | Build & publish |
| **Push images** | No (test only) | Yes (when configured) |

---

## 📖 Learning Path

**New to CI/CD?** Read in this order:

1. **[IMAGE_TAGGING_STRATEGY.md](./IMAGE_TAGGING_STRATEGY.md)** - Understand the complete strategy
2. **[TAGGING_QUICK_REFERENCE.md](./TAGGING_QUICK_REFERENCE.md)** - Quick commands and workflows
3. **[TESTING_IMAGE_TAGGING.md](./TESTING_IMAGE_TAGGING.md)** - Test locally before implementing
4. **[PHASE_2_COMPLETE.md](./PHASE_2_COMPLETE.md)** - See what's been accomplished

---

## 🏗️ Architecture: How Scripts & CI Work Together

### **The Design Philosophy**

```
┌─────────────────────────────────────────────────────────┐
│              Scripts (Single Source of Truth)           │
│  - Version calculation logic                            │
│  - Image tagging logic                                  │
│  - Reusable, testable, platform-specific               │
└─────────────────────────────────────────────────────────┘
                            ↓
                    Called by (not duplicated in)
                            ↓
┌─────────────────────────────────────────────────────────┐
│              GitHub Actions CI Workflow                 │
│  - Executes scripts                                     │
│  - Provides inputs (branch name, etc.)                  │
│  - Uses outputs (version, tags)                         │
└─────────────────────────────────────────────────────────┘
```

### **Why Separate Scripts Instead of Inline CI Logic?**

**❌ BAD: Logic Embedded in CI**
```yaml
# All logic directly in workflow
- name: Calculate version
  run: |
    LATEST_TAG=$(git describe --tags...)
    if [[ "$BRANCH" == breaking/* ]]; then
      MAJOR=$((MAJOR + 1))
      # ... 50 more lines of bash ...
    fi
```

**Problems:**
- Can't test locally before pushing
- Hard to read/maintain
- Not reusable
- Platform-specific (bash only)

**✅ GOOD: Separate Scripts Called by CI**
```yaml
# CI calls centralized script
- name: Calculate version
  run: |
    VERSION=$(./scripts/get-next-version.sh "$BRANCH")
    echo "version=$VERSION" >> $GITHUB_OUTPUT
```

**Benefits:**
- ✅ Test locally instantly: `.\scripts\get-next-version.ps1 -Verbose`
- ✅ Single source of truth (change once, works everywhere)
- ✅ Clean, readable CI workflow
- ✅ Cross-platform (PS1 for Windows, SH for Linux/CI)
- ✅ Reusable in multiple workflows

### **How CI Uses the Scripts**

**Example Flow:**
```yaml
# In .github/workflows/ci.yml (when implemented)
jobs:
  docker-build:
    runs-on: ubuntu-latest
    steps:
      # Step 1: CI executes the script
      - name: Calculate next version
        id: version
        run: |
          chmod +x ./scripts/get-next-version.sh
          BRANCH_NAME="${GITHUB_HEAD_REF}"
          VERSION=$(./scripts/get-next-version.sh "$BRANCH_NAME")
          echo "version=$VERSION" >> $GITHUB_OUTPUT
      
      # Step 2: CI uses the script's output
      - name: Build Docker image
        run: |
          docker build -t myapp:alpha-${{ steps.version.outputs.version }}-$SHA .
      
      # Step 3: CI pushes to registry
      - name: Push image
        run: docker push myapp:alpha-${{ steps.version.outputs.version }}-$SHA
```

**The workflow CALLS the scripts, it doesn't replace them!**

### **Individual Service Images (Microservices)**

Each service gets its own Docker image:

```
7 Independent Images Created:
├── ghcr.io/user/electronic-paradise-auth:v1.0.0
├── ghcr.io/user/electronic-paradise-user:v1.0.0
├── ghcr.io/user/electronic-paradise-product:v1.0.0
├── ghcr.io/user/electronic-paradise-order:v1.0.0
├── ghcr.io/user/electronic-paradise-payment:v1.0.0
├── ghcr.io/user/electronic-paradise-gateway:v1.0.0
└── ghcr.io/user/electronic-paradise-frontend:v1.0.0
```

**Why Individual Images?**

✅ **Independent Deployment:**
```bash
# Update only auth service
kubectl set image deployment/auth-service \
  auth=ghcr.io/user/electronic-paradise-auth:v1.0.1

# Other services keep running on v1.0.0 (zero downtime!)
```

✅ **Independent Scaling:**
```yaml
# Auth service: 5 replicas (high traffic)
# Product service: 3 replicas (moderate traffic)
# Payment service: 2 replicas (low traffic)
```

✅ **Resource Optimization:**
```yaml
# Each service gets appropriate resources
auth-service:      cpu: 500m, memory: 512Mi
product-service:   cpu: 250m, memory: 1Gi (needs more memory for caching)
```

✅ **Team Autonomy:**
```bash
# Team A updates product service
# Team B updates payment service
# Both deploy independently without conflicts
```

---

## 🎓 Key Concepts

### **Tag Format:**
```
Alpha:       alpha-1.1.0-abc123d
Production:  v1.0.0, v1.0.0-abc123d, latest
```

### **Version Bumping:**
```
fix/* or chore/*  → Patch  (1.0.0 → 1.0.1)
feat/*            → Minor  (1.0.0 → 1.1.0)
breaking/*        → Major  (1.0.0 → 2.0.0)
```

### **Publishing Strategy:**
```
Alpha:       NOT published (unless PUBLISH_ALPHA=true)
Production:  ALWAYS published (all 3 tags)
```

---

## 🔗 Related Documentation

- [Image Tagging Strategy](./IMAGE_TAGGING_STRATEGY.md) - Complete specification with examples
- [Testing Image Tagging](./TESTING_IMAGE_TAGGING.md) - How to test scripts locally
- [Tagging Quick Reference](./TAGGING_QUICK_REFERENCE.md) - Command cheat sheet
- [Scripts README](../../scripts/README.md) - All automation scripts explained
- [Project Roadmap](../9-roadmap-and-tracking/PROJECT_ROADMAP.md) - See Epic 6 (CI/CD Pipeline)
- [Tech Stack](../1-getting-started/TECH_STACK.md) - CI/CD technologies
- [Dockerfile Guide](../10-tools-and-automation/DOCKERFILE_EXPLAINED.md) - Docker best practices

---

## 🎓 Understanding the Complete Picture

### **Documentation Structure:**

```
docs/6-ci-cd/
├── README.md (this file)          ← Start here: Architecture & concepts
├── IMAGE_TAGGING_STRATEGY.md      ← Deep dive: Tag formats, versioning rules
├── TESTING_IMAGE_TAGGING.md       ← Hands-on: Test scripts locally
└── TAGGING_QUICK_REFERENCE.md     ← Quick lookup: Commands & examples

scripts/
├── README.md                       ← Scripts explained: Why they exist
├── get-next-version.ps1/.sh       ← Logic: Version calculation
└── tag-images.ps1                 ← Logic: Image tagging

.github/workflows/
└── ci.yml                          ← Executor: Calls scripts (when updated)
```

### **Learning Path:**

1. **Understand WHY** → Read this `README.md` (architecture & design)
2. **Understand WHAT** → Read `IMAGE_TAGGING_STRATEGY.md` (complete specification)
3. **Test LOCALLY** → Follow `TESTING_IMAGE_TAGGING.md` (hands-on)
4. **Quick Reference** → Bookmark `TAGGING_QUICK_REFERENCE.md` (commands)
5. **Scripts Details** → See `../../scripts/README.md` (implementation)

---

## 💡 Tips

✅ **DO:**
- Test scripts locally before CI
- Use specific versions in K8s (`v1.0.0`)
- Keep 5-10 versions for rollback
- Tag before merging to main

❌ **DON'T:**
- Use `latest` in production
- Overwrite semantic version tags
- Publish all alpha images (cost!)
- Skip version numbers

---

**Last Updated:** January 10, 2026  
**Maintained by:** Engineering Team
