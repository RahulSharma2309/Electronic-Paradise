# 🔧 Automation Scripts

> **All automation scripts for FreshHarvest Market**

---

## 📋 Script Index

### 🐳 Docker & Local Development

**Recommended: Use VS Code Tasks**

1. Press `Ctrl+Shift+P` in VS Code
2. Type "Tasks: Run Task"
3. Select "Docker: Build & Start All Services"

**Or use manual commands:**
```powershell
cd infra
$env:DOCKER_BUILDKIT='0'
docker-compose down -v
docker-compose build
docker-compose up -d
```

**What it does:**
- Disables BuildKit (fixes ordering issues)
- Cleans existing containers
- Builds all 7 services
- Starts all services

---

### 🏷️ CI/CD & Versioning

| Script | Purpose | When to Use |
|--------|---------|-------------|
| [`get-next-version.ps1`](./get-next-version.ps1) | Calculate next semantic version | Local testing (Windows) |
| [`get-next-version.sh`](./get-next-version.sh) | Calculate next semantic version (Bash) | GitHub Actions CI (Linux) |
| [`tag-images.ps1`](./tag-images.ps1) | Tag Docker images with versions | Local testing, manual operations |

**How They Work:**

These scripts contain the **logic** for versioning and tagging. The CI workflow **calls these scripts** rather than duplicating the logic inline.

**Architecture:**
```
Scripts (Logic) ← Called by ← CI Workflow (Executor)
     ↓                              ↓
  Reusable                      Automated
  Testable                      Runs on push
```

**Example: Version Calculation**

**Local Testing (Before CI):**
```powershell
# Test on your Windows machine
.\scripts\get-next-version.ps1 -BranchName "feat/add-2fa" -Verbose

# Output: 1.1.0 (minor bump for feat/ branches)
```

**CI Automation (After push):**
```yaml
# In .github/workflows/ci.yml (when implemented)
- name: Calculate version
  run: |
    VERSION=$(./scripts/get-next-version.sh "$BRANCH")  # ← Calls script!
    echo "Calculated: $VERSION"
```

**Why This Design?**
- ✅ **Single source of truth**: Change logic once, works everywhere
- ✅ **Test before CI**: Instant feedback without waiting for CI
- ✅ **Readable workflows**: CI file stays clean and declarative
- ✅ **Reusable**: Same scripts for CI, local testing, manual operations

**Example: Image Tagging (Local Testing)**
```powershell
# Build images locally first
cd infra
docker-compose build

# Test alpha tagging
cd ..
.\scripts\tag-images.ps1 -Alpha -Verbose

# Verify tags created (doesn't push by default)
docker images | Select-String "freshharvest-market"
```

**Note:** These scripts are the **BRAIN** (logic), CI is the **EXECUTOR** (automation). For daily local development, just use `docker-compose build`.

---

### 📝 Documentation Utilities

| Script | Purpose | When to Use |
|--------|---------|-------------|
| [`utilities/collect-md-files.ps1`](./utilities/collect-md-files.ps1) | List all markdown files | Documentation audits |

**Usage:**
```powershell
.\scripts\utilities\collect-md-files.ps1
```

Generates `md-files-list.txt` at repository root.

---

## 🗂️ Other Scripts in Repository

### Infrastructure
- **Location:** `infra/setup-env.ps1`
- **Purpose:** Set up environment variables for Docker Compose
- **Usage:** Run from `infra/` directory

### Platform
- **Location:** `platform/Ep.Platform/publish-platform.ps1`
- **Purpose:** Publish platform library to NuGet/GitHub Packages
- **Usage:** Run from platform directory

### Documentation
- **Location:** `docs/6-architecture/diagrams/render-diagrams.ps1`
- **Purpose:** Render Mermaid diagrams to PNG/SVG
- **Usage:** Run from diagrams directory

---

## 🎯 Daily Workflow Scripts

### For Local Development:
```powershell
# 1. Start development environment (use VS Code task or manual commands)
# VS Code: Ctrl+Shift+P -> "Tasks: Run Task" -> "Docker: Build & Start All Services"
# OR manually:
cd infra
$env:DOCKER_BUILDKIT='0'
docker-compose down -v
docker-compose build
docker-compose up -d

# 2. Make code changes...

# 3. Rebuild specific service (faster)
cd infra
docker-compose build auth-service
docker-compose up -d

# 4. View logs
docker-compose logs -f auth-service
```

### For CI/CD (Automated):
```yaml
# In GitHub Actions workflow
- name: Get Next Version
  run: ./scripts/get-next-version.sh

- name: Tag Images
  run: ./scripts/tag-images.ps1 -Alpha
```

---

## 📚 Related Documentation

- [Docker Commands Reference](../docs/10-tools-and-automation/DOCKER_COMMANDS.md) - Quick Docker commands
- [Image Tagging Strategy](../docs/6-ci-cd/IMAGE_TAGGING_STRATEGY.md) - Complete tagging specification
- [CI/CD Documentation](../docs/6-ci-cd/) - Automated pipelines

---

## 🔧 Script Requirements

### Windows (PowerShell)
- PowerShell 5.1+ (built into Windows 10/11)
- Docker Desktop installed

### Linux/Mac (Bash)
- Bash 4.0+
- Docker installed
- Git installed

---

## ⚠️ Important Notes

### For Local Development:
- ✅ **Use:** VS Code tasks (recommended) or manual docker-compose commands
- ❌ **Don't use:** `tag-images.ps1` (that's for CI)

### For CI/CD:
- Scripts handle versioning automatically
- No manual tagging needed
- CI uses `get-next-version.sh` (Bash version)

### BuildKit:
- Must be disabled for builds (`$env:DOCKER_BUILDKIT=0`)
- VS Code tasks handle this automatically

---

## 🆘 Troubleshooting

### Script Not Found
```powershell
# Use VS Code tasks instead, or run manually:
cd C:\path\to\FreshHarvest-Market\infra
$env:DOCKER_BUILDKIT='0'
docker-compose build
docker-compose up -d
```

### Permission Denied (Linux/Mac)
```bash
chmod +x scripts/*.sh
./scripts/get-next-version.sh
```

### Docker Build Fails
```powershell
# Ensure BuildKit is disabled
$env:DOCKER_BUILDKIT=0
cd infra
docker-compose build
```

---

**For more help, see:** [`docs/10-tools-and-automation/`](../docs/10-tools-and-automation/)
