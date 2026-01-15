# 🔒 Dependency Scanning & Security Guide

> **Automated security scanning for dependencies and Docker images - Your 24/7 security guard**

---

## 🎯 What is This?

**In Simple Terms:**
Imagine you're building a house using materials from different suppliers. You want to make sure:
- The lumber doesn't have termites
- The pipes don't have lead
- The wiring is up to code

**In Software:**
- Your app uses hundreds of libraries (dependencies)
- Each library could have security vulnerabilities
- Dependency scanning automatically checks all of them
- Alerts you when something is unsafe

**Real-World Impact:**
```
Without Scanning:
❌ Use vulnerable library → Hacker exploits it → Data breach → $$$

With Scanning:
✅ Scan detects vulnerability → Auto-creates PR to fix → Update before deploy → Safe!
```

---

## 📊 Our Multi-Layer Security Approach

We scan **4 different layers** for complete security:

```
┌─────────────────────────────────────────────────────────────┐
│  Layer 1: .NET Dependencies (NuGet packages)                │
│  Tool: dotnet list package --vulnerable                     │
│  Scans: Microsoft.AspNetCore, Dapper, etc.                  │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│  Layer 2: Frontend Dependencies (npm packages)              │
│  Tool: npm audit                                            │
│  Scans: React, axios, etc.                                  │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│  Layer 3: Docker Images (base images + OS packages)         │
│  Tool: Trivy                                                │
│  Scans: Alpine, .NET SDK, OS vulnerabilities                │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│  Layer 4: Automated Updates (Dependabot)                    │
│  Tool: GitHub Dependabot                                    │
│  Auto-creates PRs: Weekly scans, grouped updates            │
└─────────────────────────────────────────────────────────────┘
```

---

## 🤖 Layer 4: Dependabot (The Automated Security Guard)

### **What is Dependabot?**

**Simple Analogy:**
Think of Dependabot as a **security guard that patrols your code 24/7**:
- Checks all your dependencies every week
- Notices when a library gets a security update
- Automatically creates a Pull Request to update it
- You just review and merge!

**Real-World Example:**
```
Monday 9 AM:
├─ Dependabot scans all dependencies
├─ Finds: Microsoft.AspNetCore 8.0.0 has security vulnerability
├─ New version: Microsoft.AspNetCore 8.0.1 fixes it
└─ Auto-creates PR: "chore(deps): Bump Microsoft.AspNetCore from 8.0.0 to 8.0.1"

You:
├─ Review PR (CI tests run automatically)
├─ Tests pass? → Merge!
└─ Vulnerability fixed! 🎉
```

### **Configuration File**

**File:** `.github/dependabot.yml`

**What it does:**
```yaml
# Scans 4 ecosystems:

1. NuGet (.NET packages)
   - Checks all services for vulnerable Microsoft.*, System.*, etc.
   - Groups updates by type (Microsoft, Testing, Other)
   
2. npm (Frontend packages)
   - Checks React, axios, testing-library, etc.
   - Separates security updates from regular updates
   
3. GitHub Actions (Workflow dependencies)
   - Checks actions/checkout, actions/setup-dotnet, etc.
   - Keeps CI pipeline secure
   
4. Docker (Base images)
   - Checks FROM mcr.microsoft.com/dotnet/aspnet, node:20-alpine
   - Updates base images for security patches
```

### **How It Works**

**Weekly Schedule:**
```
Every Monday at 9 AM:
1. Dependabot wakes up
2. Scans all package.json, *.csproj, Dockerfiles, workflows
3. Compares your versions vs latest secure versions
4. Creates PRs for outdated/vulnerable dependencies
5. Assigns to @rahulsharma2309 for review
6. Labels PR with "dependencies", "dotnet", "frontend", etc.
```

**Grouping Strategy:**
```
Instead of 20 individual PRs:
❌ PR #1: Update Microsoft.AspNetCore
❌ PR #2: Update Microsoft.EntityFramework
❌ PR #3: Update System.Text.Json
... (17 more PRs)

With grouping:
✅ PR #1: Update all Microsoft dependencies (18 packages)
```

**Why This is Smart:**
- Fewer PRs to review
- Related updates stay together
- Faster to merge
- Less CI/CD runs

### **PR Limits**

**Configuration:**
```yaml
open-pull-requests-limit: 5  # Max 5 PRs per ecosystem
```

**Why?**
- Prevents PR spam (imagine 50 PRs on Monday morning!)
- Forces prioritization (security updates first)
- Manageable workload

### **Commit Message Format**

**Dependabot creates:**
```
chore(deps): Bump Microsoft.AspNetCore from 8.0.0 to 8.0.1

Updates Microsoft.AspNetCore package from version 8.0.0 to 8.0.1.
- Release notes: https://...
- Changelog: https://...
- Commits: https://...
```

**Why "chore(deps)"?**
- Follows conventional commits
- Semantic-release knows it's a patch bump (8.0.1 → 8.0.2)
- CHANGELOG.md gets updated automatically

---

## 🛡️ Layer 1: .NET Dependency Scanning

### **What It Scans**

**Files checked:**
```
services/auth-service/src/AuthService.csproj
services/user-service/src/UserService.csproj
services/product-service/src/ProductService.csproj
services/order-service/src/OrderService.csproj
services/payment-service/src/PaymentService.csproj
gateway/src/Gateway.csproj
platform/Ep.Platform/Ep.Platform.csproj
```

**Example vulnerable packages:**
```
Microsoft.AspNetCore.Authentication.JwtBearer 7.0.0  ← Vulnerable!
  └─ CVE-2023-12345: Authentication bypass
  └─ Fix: Update to 7.0.5 or higher

Dapper 2.0.90  ← Vulnerable!
  └─ CVE-2023-67890: SQL injection
  └─ Fix: Update to 2.0.123 or higher
```

### **How It Works**

**Command:**
```bash
dotnet list package --vulnerable --include-transitive
```

**Explanation:**
- `--vulnerable`: Only show packages with known CVEs
- `--include-transitive`: Check dependencies of dependencies
  - Example: You use Library A → which uses Library B (vulnerable)
  - Transitive scan catches this!

**CI Integration:**

**File:** `.github/workflows/ci.yml`

```yaml
# Phase 4: .NET Dependency Scanning (runs for all 6 services in parallel)
dotnet-dependency-scan:
  runs-on: ubuntu-latest
  strategy:
    fail-fast: false
    matrix:
      service:
        - { name: "auth-service", path: "services/auth-service" }
        - { name: "user-service", path: "services/user-service" }
        # ... all services
  steps:
    - name: Scan for vulnerable packages
      run: |
        cd ${{ matrix.service.path }}
        dotnet list package --vulnerable --include-transitive 2>&1 | tee scan.txt
        
        # Fail if high/critical vulnerabilities found
        if grep -q "Critical\|High" scan.txt; then
          echo "❌ High/Critical vulnerabilities found!"
          cat scan.txt
          exit 1
        fi
```

**What happens:**
```
✅ No vulnerabilities → Build continues
❌ Critical/High found → Build fails, you see report, fix before merge
```

---

## 🌐 Layer 2: Frontend (npm) Dependency Scanning

### **What It Scans**

**Files checked:**
```
frontend/package.json
frontend/package-lock.json
```

**Example vulnerable packages:**
```
react-scripts 4.0.0  ← Vulnerable!
  └─ CVE-2023-45678: XSS vulnerability
  └─ Fix: Update to 5.0.1 or higher

axios 0.21.0  ← Vulnerable!
  └─ CVE-2023-11111: SSRF vulnerability
  └─ Fix: Update to 1.6.0 or higher
```

### **How It Works**

**Command:**
```bash
npm audit --audit-level=high
```

**Explanation:**
- `--audit-level=high`: Only fail on high/critical (ignore low/moderate)
- Checks npm registry for known vulnerabilities
- Shows upgrade path

**CI Integration:**

**File:** `.github/workflows/ci.yml`

```yaml
# Phase 5: Frontend npm Dependency Scanning
frontend-dependency-scan:
  runs-on: ubuntu-latest
  steps:
    - name: Install dependencies
      run: npm ci --ignore-scripts
      working-directory: frontend
      
    - name: Run npm audit
      run: npm audit --audit-level=high
      working-directory: frontend
```

**Output Example:**
```
┌───────────────┬──────────────────────────────────────────────────────────┐
│ High          │ Cross-Site Scripting (XSS)                               │
├───────────────┼──────────────────────────────────────────────────────────┤
│ Package       │ react-scripts                                            │
├───────────────┼──────────────────────────────────────────────────────────┤
│ Patched in    │ >=5.0.1                                                  │
├───────────────┼──────────────────────────────────────────────────────────┤
│ More info     │ https://github.com/advisories/GHSA-...                   │
└───────────────┴──────────────────────────────────────────────────────────┘

found 1 high severity vulnerability
  run `npm audit fix` to fix them, or `npm audit` for details
```

**Auto-fix:**
```bash
# Many vulnerabilities can be fixed automatically:
npm audit fix

# Or for breaking changes:
npm audit fix --force
```

---

## 🐳 Layer 3: Docker Image Scanning (Trivy)

### **What is Trivy?**

**Simple Analogy:**
Think of Trivy as a **X-ray machine for Docker images**:
- Scans every layer of your image
- Checks OS packages (Alpine, Debian, etc.)
- Checks application dependencies
- Finds vulnerabilities before you deploy

**What It Scans:**

```
Your Docker Image:
├─ Base Image (mcr.microsoft.com/dotnet/aspnet:8.0)
│  └─ OS packages (Alpine Linux, apt packages)
│  └─ .NET runtime libraries
│
├─ Your Application
│  └─ NuGet packages
│  └─ npm packages (if frontend)
│
└─ All Layers Combined
   └─ Trivy scans everything!
```

### **How It Works**

**CI Integration:**

**File:** `.github/workflows/ci.yml`

```yaml
# Phase 6: Docker Image Scanning (runs for all 7 images in parallel)
docker-image-scan:
  needs: [calculate-version, docker-build]
  runs-on: ubuntu-latest
  strategy:
    fail-fast: false
    matrix:
      service:
        - auth-service
        - user-service
        - product-service
        - order-service
        - payment-service
        - gateway
        - frontend
  steps:
    - name: Run Trivy scanner
      uses: aquasecurity/trivy-action@master
      with:
        image-ref: ghcr.io/${{ github.repository_owner }}/electronic-paradise-${{ matrix.service }}:${{ needs.calculate-version.outputs.version }}
        format: 'sarif'
        output: 'trivy-results.sarif'
        severity: 'CRITICAL,HIGH'
        
    - name: Upload to GitHub Security
      uses: github/codeql-action/upload-sarif@v3
      with:
        sarif_file: 'trivy-results.sarif'
```

**Severity Levels:**
```
CRITICAL: Remote code execution, authentication bypass
HIGH:     SQL injection, XSS, privilege escalation
MEDIUM:   Information disclosure, weak crypto
LOW:      Minor issues, best practice violations

We fail on: CRITICAL + HIGH only
```

**Output Example:**
```
┌────────────────────────────────────────────────────────────────┐
│ Library: openssl                                                │
├────────────────────────────────────────────────────────────────┤
│ Vulnerability: CVE-2023-12345                                   │
│ Severity: HIGH                                                  │
│ Installed Version: 1.1.1k                                       │
│ Fixed Version: 1.1.1l                                           │
│ Description: Buffer overflow in SSL handshake                   │
└────────────────────────────────────────────────────────────────┘

❌ Found 1 HIGH severity vulnerability
❌ Build FAILED - Fix before deploying!
```

**How to Fix:**
```dockerfile
# Before (vulnerable)
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# After (fixed)
FROM mcr.microsoft.com/dotnet/aspnet:8.0.1  # ← Updated base image
```

### **GitHub Security Integration**

**Where to view:**
```
GitHub Repo → Security tab → Code scanning alerts

You'll see:
├─ All vulnerabilities found by Trivy
├─ Severity level
├─ Which image/service
├─ Recommended fix
└─ CVE details and links
```

---

## 🔄 Complete Security Workflow

### **Scenario: New Vulnerability Discovered**

```
Monday 9 AM: Microsoft announces CVE-2024-12345 in ASP.NET Core 8.0.0
              └─ Fix available: 8.0.1

Monday 9:30 AM: Dependabot detects it
                ├─ Creates PR: "chore(deps): Bump Microsoft.AspNetCore 8.0.0 → 8.0.1"
                └─ Assigns to @rahulsharma2309

Monday 10 AM: CI Pipeline runs on Dependabot PR
              ├─ ✅ Build succeeds
              ├─ ✅ Tests pass
              ├─ ✅ .NET dependency scan: No vulnerabilities
              ├─ ✅ Docker build succeeds
              └─ ✅ Trivy scan: No vulnerabilities

Monday 10:30 AM: You review PR
                 ├─ All checks green ✅
                 ├─ Click "Merge"
                 └─ Vulnerability fixed!

Monday 11 AM: CI builds new images with fix
              ├─ Pushes to GHCR
              └─ CD deploys to staging

Result: Vulnerability patched in 90 minutes! 🎉
```

---

## 📈 Security Dashboard

### **Where to View Scan Results**

**1. GitHub Security Tab**
```
Repo → Security → Code scanning alerts
├─ Trivy scan results (Docker images)
├─ Dependabot alerts (outdated dependencies)
└─ Secret scanning (if enabled)
```

**2. Pull Request Checks**
```
PR → Checks tab
├─ ✅ .NET dependency scan (passed)
├─ ✅ Frontend dependency scan (passed)
└─ ✅ Docker image scan (passed)
```

**3. CI/CD Summary**
```
Actions → CI Pipeline → Summary
├─ Dependency scan results
├─ Vulnerability count
└─ Upload artifacts (scan reports)
```

---

## 🎓 Best Practices

### **DO:**

✅ **Review Dependabot PRs weekly**
```bash
# Set aside time every Monday
# Review, test, merge security updates
```

✅ **Prioritize security updates**
```
Critical/High → Merge immediately
Medium → Merge this week
Low → Batch with other updates
```

✅ **Keep dependencies up-to-date**
```
Outdated dependencies = more vulnerabilities
Update regularly, not just when forced
```

✅ **Test Dependabot PRs**
```
Don't blindly merge!
CI runs tests, but manual testing is good too
```

✅ **Use Dependabot grouping**
```yaml
# Group related updates
groups:
  microsoft:
    patterns: ["Microsoft.*"]
```

### **DON'T:**

❌ **Don't ignore Dependabot PRs**
```
Ignoring = accumulating security debt
Harder to update later (breaking changes pile up)
```

❌ **Don't disable security scans to "make CI green"**
```
❌ BAD: continue-on-error: true (hides problems)
✅ GOOD: Fix the vulnerability
```

❌ **Don't use vulnerable dependencies in production**
```
❌ Deploy with known CVEs
✅ Fix first, deploy after
```

❌ **Don't update major versions blindly**
```
Major version bump = breaking changes
Read changelog, test thoroughly
```

---

## 🔧 Configuration Files Reference

### **Dependabot Configuration**

**File:** `.github/dependabot.yml`

```yaml
version: 2
updates:
  # .NET NuGet packages
  - package-ecosystem: "nuget"
    directory: "/"
    schedule:
      interval: "weekly"
      day: "monday"
      time: "09:00"
    open-pull-requests-limit: 5
    groups:
      microsoft:
        patterns: ["Microsoft.*", "System.*"]
      testing:
        patterns: ["*Test*", "*Mock*", "*xunit*", "*moq*"]

  # Frontend npm packages  
  - package-ecosystem: "npm"
    directory: "/frontend"
    schedule:
      interval: "weekly"
    open-pull-requests-limit: 5
    groups:
      security-updates:
        dependency-type: "development"
        update-types: ["security"]

  # GitHub Actions
  - package-ecosystem: "github-actions"
    directory: "/"
    schedule:
      interval: "weekly"
    open-pull-requests-limit: 3

  # Docker base images
  - package-ecosystem: "docker"
    directory: "/"
    schedule:
      interval: "weekly"
    open-pull-requests-limit: 3
```

---

## 🐛 Troubleshooting

### **Issue 1: Dependabot PR Conflicts**

**Problem:**
```
Dependabot PR has merge conflicts
```

**Solution:**
```bash
# Option 1: Close PR, Dependabot will recreate
Close PR → Dependabot creates new PR next week

# Option 2: Manual merge
git checkout main
git pull
git checkout -b dependabot/nuget/Microsoft.AspNetCore-8.0.1
git merge main
# Resolve conflicts
git push
```

### **Issue 2: npm audit finds too many vulnerabilities**

**Problem:**
```
npm audit shows 50+ vulnerabilities
```

**Solution:**
```bash
# Step 1: Try automatic fix
npm audit fix

# Step 2: Force update (breaking changes)
npm audit fix --force

# Step 3: Manual update
npm outdated  # See what needs updating
npm update    # Update to latest compatible

# Step 4: If stuck, update package.json manually
# Then: npm install
```

### **Issue 3: Trivy finds vulnerabilities in base image**

**Problem:**
```
Trivy finds CVE in mcr.microsoft.com/dotnet/aspnet:8.0
```

**Solution:**
```dockerfile
# Update to latest patch version
FROM mcr.microsoft.com/dotnet/aspnet:8.0     # ❌ Old
FROM mcr.microsoft.com/dotnet/aspnet:8.0.1   # ✅ Updated

# Or use specific digest (most secure)
FROM mcr.microsoft.com/dotnet/aspnet@sha256:abc123...
```

### **Issue 4: False positives**

**Problem:**
```
Scan reports vulnerability that doesn't affect you
```

**Solution:**
```yaml
# Trivy: Ignore specific CVEs
# Create .trivyignore file:
CVE-2023-12345  # Reason: We don't use the vulnerable feature

# npm audit: Ignore specific advisories
npm audit --audit-level=high --omit=dev  # Skip dev dependencies
```

---

## 📊 Metrics & Reporting

### **Security Health Metrics**

**Track weekly:**
```
Week of Jan 13, 2026:
├─ Dependabot PRs created: 3
├─ Dependabot PRs merged: 3
├─ Vulnerabilities found: 2 (1 high, 1 medium)
├─ Vulnerabilities fixed: 2
├─ Time to fix: 2 hours
└─ Status: ✅ Healthy
```

**Goals:**
```
✅ Merge Dependabot PRs within 48 hours
✅ Fix high/critical within 24 hours
✅ Zero known vulnerabilities in production
✅ 100% of services scanned weekly
```

---

## 🔗 Related Documentation

- [CI Pipeline Architecture](./MODULAR_CI_ARCHITECTURE.md) - How scanning integrates with CI
- [CD Pipeline Guide](./CD_PIPELINE_GUIDE.md) - Deployment only happens if scans pass
- [Complete DevOps Flow](./COMPLETE_DEVOPS_FLOW.md) - Security in the full pipeline
- [SonarCloud Setup](./SONARCLOUD_SETUP_GUIDE.md) - Code quality scanning
- [Project Roadmap](../9-roadmap-and-tracking/PROJECT_ROADMAP.md) - See PBI 2.5

---

## 📝 Summary

**Dependency scanning is your automated security team:**

```
┌─────────────────────────────────────────────────────────┐
│  Without Scanning                                        │
│  ❌ Manual dependency checks (time-consuming)           │
│  ❌ Miss vulnerabilities (human error)                  │
│  ❌ Deploy with known CVEs (security risk)              │
│  ❌ Reactive (fix after breach)                         │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│  With Our 4-Layer Scanning                              │
│  ✅ Automated weekly scans (Dependabot)                 │
│  ✅ Pre-merge checks (.NET + npm + Trivy)               │
│  ✅ Auto-created PRs with fixes                         │
│  ✅ Fail CI on high/critical                            │
│  ✅ Proactive (fix before deploy)                       │
│  ✅ Sleep better at night! 😴                           │
└─────────────────────────────────────────────────────────┘
```

**Your security workflow:**
1. **Dependabot** creates PRs weekly
2. **CI scans** run on every commit
3. **You review** and merge security updates
4. **CD deploys** only if scans pass
5. **Production stays secure** 🔒

**Remember:** Security is a process, not a one-time task. Keep dependencies updated, review Dependabot PRs weekly, and your application stays secure!

---

**Last Updated:** January 14, 2026  
**Maintained by:** Engineering Team
