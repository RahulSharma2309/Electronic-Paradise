# CI/CD Pipeline Flow

## Complete Workflow

### Scenario 1: Create Branch and Push Code
**Action:** Cut a branch from main and push code

**Result:**
- ✅ CI pipeline runs automatically
- ✅ Builds all services (validates code)
- ✅ Runs tests
- ✅ Creates alpha image tags (e.g., `alpha-1.1.0-abc123d`)
- ❌ Does NOT push images to registry
- ❌ No CD PR created

**Why CI runs:** Trigger `on: push: branches: ['**']` runs CI on all branch pushes

---

### Scenario 2: Raise PR to Main
**Action:** Create pull request from branch to main

**Result:**
- ✅ CI pipeline runs automatically
- ✅ Builds all services
- ✅ Runs tests
- ✅ Creates alpha image (version bump based on branch name: `fix/`, `feat/`, `breaking/`, `bug/`)
- ❌ Does NOT push images to registry (local build only)
- ❌ No CD PR created

**Purpose:** Verify CI passes before merging

**Why CI runs:** Trigger `on: pull_request:` runs CI on PR events

---

### Scenario 3: Push Changes to Existing PR
**Action:** Push additional commits to PR branch

**Result:**
- ✅ CI pipeline runs again
- ✅ Re-validates all changes
- ❌ Does NOT push images to registry

---

### Scenario 4: Manual Test Before Merge (Optional)
**Action:** Manually trigger CI with `PublishBuild=true` on PR branch

**Steps:**
1. Go to Actions → CI Pipeline
2. Click "Run workflow"
3. Select your branch
4. Set `PublishBuild` to `true`
5. Click "Run workflow"

**Result:**
- ✅ CI pipeline runs
- ✅ Builds all services
- ✅ **PUSHES alpha images to GHCR** (e.g., `alpha-1.1.0-abc123d`)
- ✅ If all 3 conditions met → **CD PR created automatically for STAGING**
- ✅ Merge CD PR → Deploys to staging K8s
- ✅ Test in staging environment

**3 Conditions for CD PR:**
1. ✅ CI pipeline passes (no errors, warnings OK)
2. ✅ All 7 images pushed to registry
3. ✅ Images verified in GHCR

---

### Scenario 5: Merge PR to Main
**Action:** Merge pull request to main branch

**Result:**
- ✅ CI pipeline runs automatically
- ✅ Builds all services
- ✅ Creates production semantic version (e.g., `v1.1.0` - no alpha)
- ✅ **ALWAYS pushes images to GHCR**
  - Tags: `v1.1.0`, `v1.1.0-abc123d`, `latest`
- ✅ If all 3 conditions met → **CD PR created automatically**
- ✅ CD PR targets:
  - **STAGING** (default) - if no k8s/prod changes
  - **PROD** - if changes in `infra/k8s/prod/`

---

### Scenario 6: Deploy to Staging (Automatic)
**Action:** Merge to main (or manual trigger with PublishBuild=true)

**CD PR Creation Logic:**
- ✅ No k8s changes → CD PR for **STAGING**
- ✅ Changes in `infra/k8s/staging/` → CD PR for **STAGING**
- ✅ Code changes only → CD PR for **STAGING**

**Flow:**
1. CI creates CD PR with updated image tags in `infra/k8s/staging/`
2. Review CD PR
3. Merge CD PR
4. CD pipeline deploys to staging K8s

---

### Scenario 7: Deploy to Production (Manual)
**Action:** After testing in staging, promote to production

**Steps:**
1. Create new branch from main
2. Update image tags in `infra/k8s/prod/deployments/*/deployment.yaml`
3. Commit and push
4. Create PR to main
5. Merge PR to main

**Result:**
- ✅ CI detects changes in `infra/k8s/prod/`
- ✅ CD PR created automatically for **PROD**
- ✅ Review CD PR carefully (production deployment)
- ✅ Merge CD PR → Deploys to production K8s

---

## Image Push Logic

```yaml
# CI workflow - docker-build job
push: (main merge) OR (PublishBuild=true)
```

| Scenario | Event | PublishBuild | Images Pushed? |
|----------|-------|--------------|----------------|
| PR raised/updated | pull_request | N/A | ❌ No |
| Branch push | push (not main) | N/A | ❌ No |
| Manual trigger on branch | workflow_dispatch | true | ✅ Yes (alpha) |
| Manual trigger on branch | workflow_dispatch | false | ❌ No |
| Merge to main | push (main) | N/A | ✅ Yes (production) |

---

## CD PR Creation Logic

```yaml
# Only create CD PR when images are ACTUALLY PUSHED
if: ((main push) OR (PublishBuild=true)) AND (docker-build succeeded)
```

| Scenario | Images Pushed? | CD PR Created? | Target Environment |
|----------|----------------|----------------|--------------------|
| PR | ❌ No | ❌ No | N/A |
| Manual (PublishBuild=true) | ✅ Yes | ✅ Yes | Staging (alpha) |
| Merge to main (no k8s changes) | ✅ Yes | ✅ Yes | Staging |
| Merge to main (k8s/staging changes) | ✅ Yes | ✅ Yes | Staging |
| Merge to main (k8s/prod changes) | ✅ Yes | ✅ Yes | **Production** |

---

## Version Tagging

### Branch Name → Version Bump

| Branch Prefix | Version Bump | Example |
|---------------|--------------|---------|
| `feat/` | Minor | 1.0.0 → 1.1.0 |
| `fix/` | Patch | 1.0.0 → 1.0.1 |
| `bug/` | Patch | 1.0.0 → 1.0.1 |
| `breaking/` | Major | 1.0.0 → 2.0.0 |
| Other | Patch | 1.0.0 → 1.0.1 |

### Image Tags

| Mode | Branch | Tags |
|------|--------|------|
| Alpha | Not main | `alpha-1.1.0-abc123d` |
| Production | main | `v1.1.0`, `v1.1.0-abc123d`, `latest` |

---

## Workflow Triggers

### CI Pipeline (`ci.yml`)
```yaml
on:
  push:
    branches: ['**']  # All branches
  pull_request:
    types: [opened, synchronize, reopened, ready_for_review]
    branches: ['**']  # PRs to any branch
  workflow_dispatch:  # Manual trigger
    inputs:
      PublishBuild: 'true' or 'false'
```

### CD Pipeline (`cd-staging.yml`)
```yaml
on:
  pull_request:
    types: [closed]  # When deployment PR is merged
    branches: [main]
  workflow_dispatch:  # Manual trigger
```

---

## Environment Detection

The CD pipeline automatically detects the target environment:

1. **Priority 1:** Changes in `infra/k8s/prod/` → **PROD**
2. **Priority 2:** Changes in `infra/k8s/staging/` → **STAGING**
3. **Priority 3:** Any other k8s changes → **STAGING** (default)
4. **Priority 4:** No k8s changes → **STAGING** (default)

---

## Quick Reference

### To test changes before merging:
1. Create PR
2. Wait for CI to pass
3. Manually trigger CI with `PublishBuild=true`
4. Review CD PR for staging
5. Merge CD PR to deploy to staging
6. Test in staging

### To deploy to production:
1. Test in staging first
2. Create branch
3. Update `infra/k8s/prod/` deployment YAMLs
4. Merge to main
5. Review CD PR for production
6. Merge CD PR to deploy to production

---

## Troubleshooting

### CI not running on PR
- Check workflow triggers in `.github/workflows/ci.yml`
- Verify GitHub Actions is enabled
- Check branch protection rules

### Images not pushed
- Check if condition is met: `(main push) OR (PublishBuild=true)`
- Verify GITHUB_TOKEN has packages: write permission

### CD PR not created
- Verify all 3 conditions:
  1. CI passed
  2. Images pushed to registry
  3. Images verified in GHCR
- Check `create-deployment-pr` job logs

### Wrong environment targeted
- Check which k8s folder has changes
- Prod changes always take priority
- Default is staging
