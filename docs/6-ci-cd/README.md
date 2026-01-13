# 🚀 CI/CD Documentation

This folder contains all documentation related to Continuous Integration and Continuous Deployment (CI/CD) for Electronic Paradise.

---

## 📚 Documentation Index

### **🎯 CI/CD Fundamentals**

| Document | Purpose | Status |
|----------|---------|--------|
| [PBIS_COMPARISON.md](./PBIS_COMPARISON.md) | Understanding CI/CD PBIs | ✅ Complete |
| [CD_PIPELINE_GUIDE.md](./CD_PIPELINE_GUIDE.md) | Complete CD pipeline theory & concepts | ✅ Complete |

### **🏷️ CI Pipeline & Image Management**

| Document | Purpose | Status |
|----------|---------|--------|
| [MODULAR_CI_ARCHITECTURE.md](./MODULAR_CI_ARCHITECTURE.md) | Parallel CI with matrix strategy | ✅ Complete |
| [IMAGE_TAGGING_STRATEGY.md](./IMAGE_TAGGING_STRATEGY.md) | Complete tagging specification | ✅ Complete |
| [TAGGING_QUICK_REFERENCE.md](./TAGGING_QUICK_REFERENCE.md) | Quick command reference | ✅ Complete |
| [TESTING_IMAGE_TAGGING.md](./TESTING_IMAGE_TAGGING.md) | Testing guide | ✅ Complete |
| [SEMANTIC_RELEASE_GUIDE.md](./SEMANTIC_RELEASE_GUIDE.md) | Automated releases & changelog | ✅ Complete |

**💡 Start Here:**
- **New to CI/CD?** → [PBIS_COMPARISON.md](./PBIS_COMPARISON.md) (explains what each PBI does)
- **Understanding CD?** → [CD_PIPELINE_GUIDE.md](./CD_PIPELINE_GUIDE.md) (staging, smoke tests, deployment)
- **Want faster CI builds?** → [MODULAR_CI_ARCHITECTURE.md](./MODULAR_CI_ARCHITECTURE.md) (60-70% faster!)
- **Setting up Docker builds?** → [IMAGE_TAGGING_STRATEGY.md](./IMAGE_TAGGING_STRATEGY.md)
- **Setting up releases?** → [SEMANTIC_RELEASE_GUIDE.md](./SEMANTIC_RELEASE_GUIDE.md)
- **Quick reference?** → [TAGGING_QUICK_REFERENCE.md](./TAGGING_QUICK_REFERENCE.md)

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

1. **[PBIS_COMPARISON.md](./PBIS_COMPARISON.md)** - Understand what each PBI does (CI vs CD)
2. **[CD_PIPELINE_GUIDE.md](./CD_PIPELINE_GUIDE.md)** - Deep dive into CD concepts (staging, smoke tests)
3. **[MODULAR_CI_ARCHITECTURE.md](./MODULAR_CI_ARCHITECTURE.md)** - How the CI pipeline works (parallel builds)
4. **[IMAGE_TAGGING_STRATEGY.md](./IMAGE_TAGGING_STRATEGY.md)** - Complete tagging strategy
5. **[TAGGING_QUICK_REFERENCE.md](./TAGGING_QUICK_REFERENCE.md)** - Quick commands and workflows
6. **[TESTING_IMAGE_TAGGING.md](./TESTING_IMAGE_TAGGING.md)** - Test locally before implementing
7. **[SEMANTIC_RELEASE_GUIDE.md](./SEMANTIC_RELEASE_GUIDE.md)** - Automated releases and changelogs

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

---

## 📊 CI vs CD: Understanding the Boundary

### **The Debate: Is Pushing to Registry CI or CD?**

This is commonly debated, but **industry standard says: Pushing to Registry is CI** ✅

### **The Clear Boundary:**

```
┌─────────────────────────────────────────────────────────┐
│                    CI (Continuous Integration)          │
│  "Is the code correct? Can it be packaged?"             │
├─────────────────────────────────────────────────────────┤
│  1. Checkout code                                       │
│  2. Run linters/static analysis                         │
│  3. Build code                                          │
│  4. Run unit tests                                      │
│  5. Run integration tests                               │
│  6. Code quality checks (SonarCloud)                    │
│  7. Build Docker images                                 │
│  8. Push to Registry (GHCR)          ← WE ARE HERE      │
└─────────────────────────────────────────────────────────┘
                         ↓
        IMAGE STORED BUT NOT RUNNING YET
                         ↓
┌─────────────────────────────────────────────────────────┐
│              CD (Continuous Deployment/Delivery)        │
│  "Get the code to users in production"                  │
├─────────────────────────────────────────────────────────┤
│  1. Pull image from registry                            │
│  2. Deploy to staging environment                       │
│  3. Run smoke tests on staging                          │
│  4. (Manual approval for prod?)                         │
│  5. Deploy to production                                │
│  6. Health checks                                       │
│  7. Rollback if issues                                  │
│  8. Notify team (Slack, email)                          │
└─────────────────────────────────────────────────────────┘
                         ↓
              USERS CAN ACCESS IT NOW!
```

### **Key Differences:**

| Aspect | CI (What We Have) | CD (Future) |
|--------|-------------------|-------------|
| **Purpose** | Verify code quality | Deliver to users |
| **Output** | Build artifact (image in registry) | Running service in production |
| **Trigger** | Every commit/PR | Merge to main (or manual) |
| **Risk** | Low (just building) | High (affecting real users) |
| **Rollback** | Delete bad image | Rollback K8s deployment |
| **User Impact** | None (they don't see it) | Direct (they access it) |

### **Why Push to Registry is CI, Not CD:**

1. **Image in registry ≠ Deployed**
   - Just because it's in GHCR doesn't mean users can access it
   - It's a **build artifact**, like a .jar or .exe file

2. **CD = Deployment Actions**
   - Updating Kubernetes deployments
   - Rolling out to servers
   - Making code accessible to end users

3. **Industry Analogy:**
   ```
   Java:     Build → Test → Push to Maven  ← CI stops here
   .NET:     Build → Test → Push to NuGet  ← CI stops here
   Docker:   Build → Test → Push to GHCR   ← CI stops here
   
   Then CD:  Pull from registry → Deploy to K8s → Users access it
   ```

### **Current Implementation Status:**

**✅ CI Pipeline (Complete):**
```yaml
.github/workflows/ci.yml:
  ├─ Job 1: dotnet-analysis (build, test, SonarCloud)
  ├─ Job 2: frontend-build (React build)
  └─ Job 3: docker-build (build images, push to GHCR)
```

**Result:** 7 Docker images in GitHub Container Registry
**User Impact:** None (images stored, not deployed)

**❌ CD Pipeline (Not Built Yet):**
```yaml
.github/workflows/cd.yml (future):
  ├─ Job 1: deploy-staging (pull images, deploy to K8s staging)
  ├─ Job 2: smoke-tests (validate staging deployment)
  └─ Job 3: deploy-production (deploy to K8s production)
```

**Result:** Running services accessible to users
**User Impact:** Direct (they can use the application)

### **Real-World Example:**

**Scenario 1: Push to Feature Branch**
```bash
# You push code to feat/add-2fa
git push origin feat/add-2fa

# CI Pipeline runs:
✅ Code builds
✅ Tests pass
✅ Image created: alpha-0.1.0-abc123d
✅ Image pushed to GHCR

# Question: Can users access your new feature?
# Answer: ❌ NO! It's just sitting in the registry.
#         Users are still using the old production version.
```

**Scenario 2: Merge to Main (CI only)**
```bash
# You merge to main
git checkout main
git merge feat/add-2fa

# CI Pipeline runs:
✅ Code builds
✅ Tests pass
✅ Image created: v1.1.0
✅ Image pushed to GHCR

# Question: Can users access your new feature?
# Answer: ❌ STILL NO! Image is in GHCR but not deployed.
#         Production is still running v1.0.0
```

**Scenario 3: Full CI/CD (Future State)**
```bash
# You merge to main
git checkout main
git merge feat/add-2fa

# CI Pipeline runs:
✅ Image v1.1.0 pushed to GHCR

# CD Pipeline triggers:
✅ Pulls v1.1.0 from GHCR
✅ Deploys to K8s staging
✅ Smoke tests pass
✅ Deploys to K8s production

# Question: Can users access your new feature?
# Answer: ✅ YES! Now they can use it!
```

---

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

### **CI/CD Definitions:**

**CI (Continuous Integration):**
- Purpose: Verify code integrates correctly
- Actions: Build, test, package (create Docker images)
- Output: Build artifacts (images in registry)
- When: Every commit, every PR
- Risk: Low (no user impact)

**CD (Continuous Deployment/Delivery):**
- Purpose: Deliver code to users
- Actions: Deploy, release, monitor
- Output: Running services in production
- When: Merge to main (automated or manual approval)
- Risk: High (direct user impact)

**Registry (The Middle Ground):**
- Not CI output: It's just storage
- Not CD yet: Code not running/accessible
- Think of it as: A warehouse between factory (CI) and store (CD)

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
Alpha (PR):        NOT published by default
Production (main): ALWAYS published (all 3 tags)
```

### **Where We Are:**
```
✅ CI Complete:    Images in GHCR
❌ CD Not Built:   Not deployed to K8s yet
```

---

## 🔗 Related Documentation

- [CD Pipeline Guide](./CD_PIPELINE_GUIDE.md) - Complete CD theory (staging, smoke tests, deployment)
- [PBIs Comparison](./PBIS_COMPARISON.md) - Understanding all CI/CD PBIs
- [Modular CI Architecture](./MODULAR_CI_ARCHITECTURE.md) - Parallel CI pipeline design
- [Image Tagging Strategy](./IMAGE_TAGGING_STRATEGY.md) - Complete specification with examples
- [Testing Image Tagging](./TESTING_IMAGE_TAGGING.md) - How to test scripts locally
- [Tagging Quick Reference](./TAGGING_QUICK_REFERENCE.md) - Command cheat sheet
- [Semantic Release Guide](./SEMANTIC_RELEASE_GUIDE.md) - Automated versioning and releases
- [Scripts README](../../scripts/README.md) - All automation scripts explained
- [Project Roadmap](../9-roadmap-and-tracking/PROJECT_ROADMAP.md) - See Epic 2 (CI/CD Pipeline)
- [Tech Stack](../1-getting-started/TECH_STACK.md) - CI/CD technologies
- [Dockerfile Guide](../10-tools-and-automation/DOCKERFILE_EXPLAINED.md) - Docker best practices

---

## 🎓 Understanding the Complete Picture

### **Documentation Structure:**

```
docs/6-ci-cd/
├── README.md (this file)            ← Start here: Overview & index
├── PBIS_COMPARISON.md               ← Understanding: CI/CD PBIs explained
├── CD_PIPELINE_GUIDE.md             ← Theory: Complete CD concepts
├── MODULAR_CI_ARCHITECTURE.md       ← Architecture: Parallel CI design
├── IMAGE_TAGGING_STRATEGY.md        ← Deep dive: Tag formats, versioning rules
├── TAGGING_QUICK_REFERENCE.md       ← Quick lookup: Commands & examples
├── TESTING_IMAGE_TAGGING.md         ← Hands-on: Test scripts locally
└── SEMANTIC_RELEASE_GUIDE.md        ← Automation: Releases & changelog

scripts/
├── README.md                         ← Scripts explained: Why they exist
├── get-next-version.ps1/.sh         ← Logic: Version calculation
└── tag-images.ps1                   ← Logic: Image tagging

.github/workflows/
├── ci.yml                           ← Executor: CI pipeline (parallel builds)
└── cd-staging.yml (future)          ← Executor: CD pipeline (deployment)
```

### **Learning Path:**

1. **Understand CI/CD** → Read `PBIS_COMPARISON.md` (what's CI vs CD)
2. **Understand CD** → Read `CD_PIPELINE_GUIDE.md` (staging, smoke tests, deployment)
3. **Understand CI** → Read `MODULAR_CI_ARCHITECTURE.md` (parallel builds)
4. **Understand Tagging** → Read `IMAGE_TAGGING_STRATEGY.md` (complete specification)
5. **Test LOCALLY** → Follow `TESTING_IMAGE_TAGGING.md` (hands-on)
6. **Quick Reference** → Bookmark `TAGGING_QUICK_REFERENCE.md` (commands)
7. **Scripts Details** → See `../../scripts/README.md` (implementation)

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
