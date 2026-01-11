# 📝 Semantic Release Guide

> **Automated versioning, changelog generation, and GitHub releases**

---

## 🤔 What Is This? (Simple Explanation)

Think of your project like a product you're shipping:

### **Without Semantic Release:**
```
1. You make a product (write code)
2. You pack it in a box (Docker image)
3. You write version on box: "1.1.0" (tag the image)
4. You put box in warehouse (GHCR)
✅ Product ready!

❌ But nobody knows what's new!
❌ No announcement made
❌ No list of changes
```

### **With Semantic Release:**
```
1. You make a product (write code)
2. You pack it in a box (Docker image)
3. You write version on box: "1.1.0" (tag the image)
4. You put box in warehouse (GHCR)
✅ Product ready!

✅ Tool automatically creates:
   - Press release on GitHub
   - Changelog file with history
   - Announcement of what's new
```

**In Short:** Semantic Release = Auto-generated release announcements and changelogs

---

## 🎯 What Does It Actually Do?

Semantic Release automatically:
1. ✅ Reads your commit messages
2. ✅ Determines version bump (major, minor, patch)
3. ✅ Creates Git tags
4. ✅ Generates CHANGELOG.md
5. ✅ Creates GitHub releases with notes
6. ✅ All automatically - **you write NOTHING!**

---

## ⚠️ Important: What It Does NOT Do

### **Semantic Release Does NOT:**
- ❌ Build Docker images (CI does this)
- ❌ Push to registry (CI does this)
- ❌ Deploy to Kubernetes (CD does this)
- ❌ Change any code
- ❌ Affect how your app runs

### **Semantic Release ONLY:**
- ✅ Creates documentation
- ✅ Creates announcements
- ✅ Makes GitHub repo look professional

**Docker/K8s deployment works EXACTLY THE SAME with or without it!**

---

## 🔄 How It Fits with What We Already Have

### **What We Already Built (PBI 6.2) - Docker Image Versioning:**

```bash
# Branch name determines Docker image version
feat/add-2fa → CI calculates: 1.1.0
             → Docker image: alpha-1.1.0-abc123d
             → Push to GHCR

# On main branch
merge to main → You manually: git tag v1.1.0
              → Docker images: v1.1.0, v1.1.0-abc123d, latest
              → Push to GHCR
```

**Result:** ✅ Docker images in GHCR, ❌ No GitHub releases, ❌ No CHANGELOG

---

### **What Semantic Release Adds (PBI 6.3) - Release Automation:**

```bash
# Commit messages determine releases
git commit -m "feat: add 2FA"
git push origin main

# semantic-release automatically:
1. Reads commit: "feat: add 2FA"
2. Determines: feat = minor bump
3. Creates Git tag: v1.1.0 (NO MANUAL TAG NEEDED!)
4. Generates CHANGELOG.md automatically
5. Creates GitHub release with notes
```

**Result:** ✅ Docker images in GHCR, ✅ GitHub releases, ✅ CHANGELOG

---

### **Complete Flow (With Both):**

```
Developer commits:
  "feat: add two-factor authentication"
       ↓
Push to main
       ↓
┌─────────────────────────────────────┐
│  CI Workflow (builds Docker)        │
│  - Build images                     │
│  - Tag: v1.1.0                      │
│  - Push to GHCR                     │
└─────────────────────────────────────┘
       ↓
┌─────────────────────────────────────┐
│  Release Workflow (documentation)   │
│  - Read commits                     │
│  - Create tag: v1.1.0               │
│  - Generate CHANGELOG                │
│  - Create GitHub release            │
└─────────────────────────────────────┘
       ↓
Both work independently!
Docker images AND GitHub releases created!
```

---

## 💡 Key Differences Explained

| Aspect | PBI 6.2 (Docker Versioning) | PBI 6.3 (Semantic Release) |
|--------|----------------------------|---------------------------|
| **What it versions** | Docker images | Git repository (tags, releases) |
| **Input** | Branch name (feat/add-2fa) | Commit messages (feat: add 2fa) |
| **Who creates Git tags** | You manually | Tool automatically |
| **GitHub Release** | ❌ Not created | ✅ Created with notes |
| **CHANGELOG.md** | ❌ Not generated | ✅ Auto-generated |
| **Required format** | Branch naming | Commit message format |
| **Affects deployment** | Yes (creates images) | No (just documentation) |
| **When you need it** | Always (for Docker) | Optional (for announcements) |

---

## 📝 Who Writes the Changelog? (Answer: NOBODY!)

### **You Write:**
```bash
git commit -m "feat: add OAuth login"
git commit -m "feat: add email verification"
git commit -m "fix: password reset bug"
```

### **Semantic Release Automatically Creates:**

**CHANGELOG.md:**
```markdown
## [1.2.0] - 2026-01-11

### 🚀 Features
- add OAuth login
- add email verification

### 🐛 Bug Fixes
- password reset bug
```

**GitHub Release:**
```markdown
## What's Changed
* feat: add OAuth login
* feat: add email verification  
* fix: password reset bug

**Full Changelog**: v1.1.0...v1.2.0
```

**You literally wrote ZERO lines of documentation!** 🎉

---

## 🚀 How It Works

### **Step 1: You Write Commits in Special Format**

```bash
# Feature (minor version bump: 1.0.0 → 1.1.0)
git commit -m "feat: add two-factor authentication"
git commit -m "feat(auth): support OAuth login"

# Bug fix (patch version bump: 1.0.0 → 1.0.1)
git commit -m "fix: resolve login timeout"
git commit -m "fix(payment): handle null wallet balance"

# Breaking change (major version bump: 1.0.0 → 2.0.0)
git commit -m "feat!: remove deprecated API v1"
# OR
git commit -m "feat: new auth system

BREAKING CHANGE: JWT format changed, all clients must upgrade"
```

### **Step 2: Push to Main Branch**

```bash
git push origin main
```

### **Step 3: Semantic Release Runs Automatically**

The release workflow triggers and:
- Analyzes all commits since last release
- Calculates new version
- Creates Git tag
- Generates changelog
- Creates GitHub release

**You did NOTHING except write good commit messages!**

---

## 📋 Commit Message Format (Conventional Commits)

### **Format:**
```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### **Types and Their Effects:**

| Type | Version Bump | Appears in Changelog | Example |
|------|--------------|---------------------|---------|
| `feat:` | Minor (1.0.0 → 1.1.0) | ✅ Yes | `feat: add user dashboard` |
| `fix:` | Patch (1.0.0 → 1.0.1) | ✅ Yes | `fix: resolve login bug` |
| `perf:` | Patch (1.0.0 → 1.0.1) | ✅ Yes | `perf: optimize database queries` |
| `docs:` | None | ✅ Yes | `docs: update API documentation` |
| `style:` | None | ❌ No | `style: format code` |
| `refactor:` | None | ❌ No | `refactor: simplify auth logic` |
| `test:` | None | ❌ No | `test: add unit tests` |
| `chore:` | None | ❌ No | `chore: update dependencies` |
| `ci:` | None | ❌ No | `ci: update GitHub Actions` |
| `build:` | None | ❌ No | `build: update Docker config` |

### **Breaking Changes (Major Bump):**

```bash
# Method 1: Add ! after type
git commit -m "feat!: remove old API endpoints"

# Method 2: Add BREAKING CHANGE in footer
git commit -m "feat: new authentication system

BREAKING CHANGE: Old JWT tokens are no longer valid"
```

---

## 📖 Real Examples

### **Example 1: Adding a Feature**

```bash
# Your work
git checkout -b feat/add-2fa
# ... write code ...
git commit -m "feat: add two-factor authentication support"
git commit -m "test: add 2FA integration tests"
git commit -m "docs: document 2FA setup process"

# Merge to main
git checkout main
git merge feat/add-2fa
git push origin main
```

**Semantic Release creates:**
```markdown
## [1.1.0] - 2026-01-11

### 🚀 Features
- add two-factor authentication support

### 📚 Documentation
- document 2FA setup process
```

---

### **Example 2: Bug Fixes**

```bash
git commit -m "fix: resolve payment gateway timeout"
git commit -m "fix: handle null user profile gracefully"
git push origin main
```

**Semantic Release creates:**
```markdown
## [1.0.1] - 2026-01-11

### 🐛 Bug Fixes
- resolve payment gateway timeout
- handle null user profile gracefully
```

---

### **Example 3: Breaking Changes**

```bash
git commit -m "feat!: migrate to new API v2

BREAKING CHANGE: All clients must update to use /api/v2 endpoints. Old /api/v1 endpoints removed."
git push origin main
```

**Semantic Release creates:**
```markdown
## [2.0.0] - 2026-01-11

### ⚠ BREAKING CHANGES

* All clients must update to use /api/v2 endpoints. Old /api/v1 endpoints removed.

### 🚀 Features
- migrate to new API v2
```

---

## 🎨 Commit Message Best Practices

### **✅ Good Examples:**

```bash
feat: add user profile editing
feat(auth): implement OAuth2 login
fix: resolve database connection timeout
fix(payment): prevent duplicate transactions
docs: add API usage examples
perf: optimize product search query
```

### **❌ Bad Examples:**

```bash
"updated stuff"              # ❌ No type, unclear
"fix bug"                    # ❌ Not descriptive
"WIP"                        # ❌ Not conventional format
"feat added login feature"   # ❌ Wrong format (missing colon)
```

### **Tips:**

1. **Use imperative mood:** "add feature" not "added feature"
2. **Be specific:** "fix login timeout" not "fix bug"
3. **Add scope when helpful:** `feat(auth):` for auth-related features
4. **Keep first line short:** 50-70 characters
5. **Add body for context:** Explain WHY, not WHAT

---

## 🔄 Workflow Integration

### **How It Works with CI/CD:**

```
1. Developer commits with conventional format
   ↓
2. Push to main branch
   ↓
3. CI workflow runs (build, test, Docker)
   ↓
4. Release workflow runs
   - Reads commits since last release
   - Creates new version tag
   - Generates CHANGELOG
   - Creates GitHub release
   ↓
5. CI picks up new tag
   - Builds Docker images with new version
   - Pushes to registry
```

**Notice:** CI/CD Docker builds happen **SEPARATELY**. Semantic Release just creates tags and documentation.

---

## 📊 What Gets Created

### **1. Git Tag**
```
Repository → Tags
└── v1.1.0 (newly created)
```

### **2. GitHub Release**
```
Repository → Releases
└── v1.1.0 - January 11, 2026
    └── Release Notes:
        - feat: add 2FA
        - fix: login bug
        - docs: API updates
```

### **3. CHANGELOG.md (Updated)**
```markdown
## [1.1.0] - 2026-01-11

### 🚀 Features
- add 2FA

### 🐛 Bug Fixes
- login bug

### 📚 Documentation
- API updates
```

---

## 🚦 Getting Started

### **1. Install Commitizen (Optional - Helps Write Commits)**

```bash
npm install -g commitizen cz-conventional-changelog
```

**Then instead of `git commit`, use:**
```bash
git cz
```

This gives you an interactive prompt to build commit messages correctly!

### **2. Make Your First Release**

```bash
# Make some changes
git checkout -b feat/initial-release

# Commit with conventional format
git commit -m "feat: initial release with MVP features"

# Merge to main
git checkout main
git merge feat/initial-release
git push origin main

# Watch the magic happen!
# Go to: GitHub → Actions → See "Release" workflow
# Then check: GitHub → Releases → See new release!
```

---

## 🔍 Troubleshooting

### **Issue: No release created**

**Reason:** No commits that trigger releases (feat, fix, perf)

**Solution:** Make sure you have at least one `feat:` or `fix:` commit

### **Issue: Wrong version bump**

**Reason:** Commit message format incorrect

**Solution:** Use exact format: `type: description`

### **Issue: Release says "no changes"**

**Reason:** Only `chore:`, `docs:`, `test:` commits

**Solution:** These don't trigger releases. Add `feat:` or `fix:` commits

---

## 📚 Resources

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Semantic Versioning](https://semver.org/)
- [semantic-release Documentation](https://semantic-release.gitbook.io/)

---

## 🆚 Comparing PBI 6.2 vs PBI 6.3 (Side-by-Side)

### **Scenario: Adding Two-Factor Authentication**

#### **PBI 6.2 Only (Docker Image Versioning):**

```bash
# Step 1: Create branch
git checkout -b feat/add-2fa

# Step 2: Write code & commit (any message works)
git commit -m "added 2fa code"
git commit -m "finished testing"

# Step 3: Push & merge to main
git push origin feat/add-2fa
# ... PR approved, merge to main

# Step 4: CI runs automatically
# ✅ Docker images created: v1.1.0, latest
# ✅ Images pushed to GHCR
# ✅ Ready to deploy!

# Step 5: Manual work
# ❌ You create GitHub release manually
# ❌ You write what changed manually
# ❌ You update CHANGELOG manually
```

**End Result:**
- ✅ Docker images ready
- ❌ No GitHub release
- ❌ No CHANGELOG

---

#### **PBI 6.2 + PBI 6.3 (Docker + Semantic Release):**

```bash
# Step 1: Create branch
git checkout -b feat/add-2fa

# Step 2: Write code & commit (conventional format)
git commit -m "feat: add two-factor authentication"
git commit -m "test: add 2FA integration tests"

# Step 3: Push & merge to main
git push origin feat/add-2fa
# ... PR approved, merge to main

# Step 4: CI runs automatically
# ✅ Docker images created: v1.1.0, latest
# ✅ Images pushed to GHCR
# ✅ Ready to deploy!

# Step 5: semantic-release runs automatically
# ✅ Git tag created: v1.1.0
# ✅ GitHub release created with notes
# ✅ CHANGELOG.md updated
```

**End Result:**
- ✅ Docker images ready
- ✅ GitHub release created
- ✅ CHANGELOG generated
- ✅ Professional appearance

---

### **Key Insight:**

```
PBI 6.2 = Product ready to ship (Docker images)
PBI 6.3 = Product announcement written (GitHub release)

Both independent!
You can have one without the other!
```

**But together:** Your project looks enterprise-grade! 🚀

---

## ❓ FAQ (Frequently Asked Questions)

### **Q: Do I need semantic release to deploy to Kubernetes?**
**A:** No! Docker images work fine without it. Semantic release is just for documentation.

### **Q: Will my CI break if semantic release fails?**
**A:** No! They run separately. Docker build can succeed even if release fails.

### **Q: What if I forget to use conventional commit format?**
**A:** Semantic release won't create a release (no big deal). Your Docker images still work!

### **Q: Can I still create releases manually?**
**A:** Yes! But why? Tool does it better and faster.

### **Q: What if I want to skip a release?**
**A:** Add `[skip ci]` to commit message, or only commit `chore:`, `docs:`, etc.

### **Q: Does this replace PBI 6.2?**
**A:** No! PBI 6.2 is for Docker images (required). PBI 6.3 is for announcements (nice-to-have).

### **Q: What if my team doesn't follow conventional commits?**
**A:** Tool won't work. Need team discipline. Consider adding commit linter (future task).

---

## 🎓 Learning Path

1. **Read this guide** ✅
2. **Try writing conventional commits** (use `git cz` for help)
3. **Make a test commit:** `git commit -m "feat: test semantic release"`
4. **Push to main and watch release workflow**
5. **Check GitHub Releases and CHANGELOG.md**
6. **Celebrate!** 🎉

---

**Remember:** You never write changelogs or release notes manually again!
