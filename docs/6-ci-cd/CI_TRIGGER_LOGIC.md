# CI Pipeline Trigger Logic - Complete Explanation

## When Does CI Pipeline Start?

The CI pipeline (`ci.yml`) has **3 triggers** defined at the top:

```yaml
on:
  push:              # Trigger 1: When you push code
    branches:
      - '**'         # To ANY branch (**, main, feature/xyz, etc.)
  
  pull_request:      # Trigger 2: When PR events happen
    types: [opened, synchronize, reopened, ready_for_review]
    branches:
      - '**'         # PRs targeting ANY branch
  
  workflow_dispatch: # Trigger 3: Manual run via GitHub UI
    inputs:
      PublishBuild: 'true' or 'false'
```

---

## Trigger 1: Push to ANY Branch

**What triggers it:**
- `git push origin <branch-name>` to any branch (main, feature/xyz, chore/abc, etc.)

**When it runs:**
- Immediately after push

**What happens:**
```
Event: push
Branch: feature/xyz (for example)
github.event_name = 'push'
github.ref = 'refs/heads/feature/xyz'
```

**Jobs that run:**
1. ✅ `calculate-version` - Calculates version (always runs)
2. ✅ `dotnet-services` - Builds .NET services (always runs)
3. ✅ `frontend-build` - Builds frontend (always runs)
4. ✅ `dependency-scanning` - Scans dependencies (always runs)
5. ✅ `frontend-dependency-scanning` - Scans frontend deps (always runs)
6. ✅ `sonarcloud` - Code quality scan (always runs)
7. ✅ `docker-build` - Builds Docker images (always runs)
   - **Push images?** ❌ NO (unless it's main branch)
8. ❌ `docker-scanning` - SKIPPED (only if images pushed)
9. ✅ `pipeline-summary` - Shows summary (always runs)
10. ❌ `create-deployment-pr` - SKIPPED (only if images pushed)

---

## Trigger 2: Pull Request Events

**What triggers it:**
- Create PR: `opened`
- Push to PR branch: `synchronize`
- Reopen PR: `reopened`
- Mark PR ready: `ready_for_review`

**When it runs:**
- When you create PR to any branch
- When you push commits to an existing PR branch

**What happens:**
```
Event: pull_request
PR: feature/xyz → main
github.event_name = 'pull_request'
github.ref = 'refs/pull/123/merge' (GitHub's merge preview)
```

**Jobs that run:**
1. ✅ `calculate-version` - Calculates version (always runs)
2. ✅ `dotnet-services` - Builds .NET services (always runs)
3. ✅ `frontend-build` - Builds frontend (always runs)
4. ✅ `dependency-scanning` - Scans dependencies (always runs)
5. ✅ `frontend-dependency-scanning` - Scans frontend deps (always runs)
6. ✅ `sonarcloud` - Code quality scan (always runs)
7. ✅ `docker-build` - Builds Docker images (always runs)
   - **Push images?** ❌ NO (PRs never push)
8. ❌ `docker-scanning` - SKIPPED (only if images pushed)
9. ✅ `pipeline-summary` - Shows summary (always runs)
10. ❌ `create-deployment-pr` - SKIPPED (only if images pushed)

**Why no image push on PR?**
- Security: Prevent malicious PRs from pushing bad images
- Fast feedback: Just validate code, don't publish
- Cost: Save registry space

---

## Trigger 3: Manual Run (workflow_dispatch)

**How to trigger:**
1. Go to GitHub → Actions → CI Pipeline
2. Click "Run workflow"
3. Select branch
4. Choose PublishBuild: `true` or `false`
5. Click "Run workflow"

**What happens:**
```
Event: workflow_dispatch
Branch: feature/xyz (your selection)
github.event_name = 'workflow_dispatch'
github.event.inputs.PublishBuild = 'true' or 'false'
```

**Jobs that run (PublishBuild = false):**
- Same as push to branch (builds but doesn't push)

**Jobs that run (PublishBuild = true):**
1. ✅ `calculate-version`
2. ✅ `dotnet-services`
3. ✅ `frontend-build`
4. ✅ `dependency-scanning`
5. ✅ `frontend-dependency-scanning`
6. ✅ `sonarcloud`
7. ✅ `docker-build` - **Push images?** ✅ YES (alpha images)
8. ✅ `docker-scanning` - Scans pushed images
9. ✅ `pipeline-summary`
10. ✅ `create-deployment-pr` - Creates CD PR for staging

---

## Job-Level Logic

### Job 1: `calculate-version`
**Runs when:** ALWAYS

**Logic:**
```yaml
# No if condition = always runs
```

---

### Job 7: `docker-build`
**Runs when:** ALWAYS (builds images)

**Push logic:**
```yaml
push: ${{ (github.event_name == 'push' && github.ref == 'refs/heads/main') || 
          (github.event_name == 'workflow_dispatch' && github.event.inputs.PublishBuild == 'true') }}
```

**Translation:**
- Push images if: (push to main) OR (manual run with PublishBuild=true)
- Otherwise: Build but don't push

**Examples:**
| Scenario | Event | Branch | PublishBuild | Push? |
|----------|-------|--------|--------------|-------|
| Push to feature branch | push | feature/xyz | N/A | ❌ NO |
| Push to main | push | main | N/A | ✅ YES |
| PR created | pull_request | PR branch | N/A | ❌ NO |
| PR updated | pull_request | PR branch | N/A | ❌ NO |
| Manual run | workflow_dispatch | any | false | ❌ NO |
| Manual run | workflow_dispatch | any | true | ✅ YES |

---

### Job 8: `docker-scanning`
**Runs when:** Images were pushed

**Logic:**
```yaml
if: (github.event_name == 'push' && github.ref == 'refs/heads/main') || 
    (github.event_name == 'workflow_dispatch' && github.event.inputs.PublishBuild == 'true')
```

**Why:** Only scan images that are in the registry

---

### Job 10: `create-deployment-pr`
**Runs when:** Images were pushed AND docker-build succeeded

**Logic:**
```yaml
if: (github.event_name == 'push' && github.ref == 'refs/heads/main' && needs.docker-build.result == 'success') || 
    (github.event_name == 'workflow_dispatch' && github.event.inputs.PublishBuild == 'true' && needs.docker-build.result == 'success')
```

**Translation:**
- Create CD PR if:
  1. Images were pushed (main OR PublishBuild=true)
  2. AND docker-build job succeeded
  3. AND all images verified in GHCR (checked in job step)

---

## Complete Flow Examples

### Example 1: Create Branch and Push
```
Action: git push origin feature/new-feature
Trigger: push
Branch: feature/new-feature

Flow:
1. CI starts ✅
2. calculate-version runs ✅
3. All build/test jobs run ✅
4. docker-build runs ✅
   - Builds images ✅
   - Push images? ❌ NO (not main branch)
5. docker-scanning ❌ SKIPPED
6. create-deployment-pr ❌ SKIPPED
7. CI completes ✅
```

---

### Example 2: Create PR
```
Action: Create PR (feature/new-feature → main)
Trigger: pull_request (opened)
Branch: PR merge preview

Flow:
1. CI starts ✅
2. calculate-version runs ✅
3. All build/test jobs run ✅
4. docker-build runs ✅
   - Builds images ✅
   - Push images? ❌ NO (PR event)
5. docker-scanning ❌ SKIPPED
6. create-deployment-pr ❌ SKIPPED
7. CI completes ✅
```

---

### Example 3: Push to Existing PR
```
Action: git push origin feature/new-feature (PR already exists)
Trigger: pull_request (synchronize)
Branch: PR merge preview

Flow:
1. CI starts ✅ (triggered by PR update)
2. Same as Example 2
```

---

### Example 4: Merge PR to Main
```
Action: Merge PR → main
Trigger: push (to main)
Branch: main

Flow:
1. CI starts ✅
2. calculate-version runs ✅ (production mode)
3. All build/test jobs run ✅
4. docker-build runs ✅
   - Builds images ✅
   - Push images? ✅ YES (main branch!)
   - Tags: v1.1.0, v1.1.0-abc123d, latest
5. docker-scanning runs ✅ (scans pushed images)
6. create-deployment-pr runs ✅
   - Verifies all images in GHCR ✅
   - Updates image tags in k8s YAMLs ✅
   - Creates CD PR for staging/prod ✅
7. CI completes ✅
```

---

### Example 5: Manual Run with PublishBuild=true
```
Action: Manual trigger → Select branch → PublishBuild=true
Trigger: workflow_dispatch
Branch: feature/new-feature

Flow:
1. CI starts ✅
2. calculate-version runs ✅ (alpha mode)
3. All build/test jobs run ✅
4. docker-build runs ✅
   - Builds images ✅
   - Push images? ✅ YES (PublishBuild=true!)
   - Tags: alpha-1.1.0-abc123d
5. docker-scanning runs ✅ (scans pushed images)
6. create-deployment-pr runs ✅
   - Verifies all images in GHCR ✅
   - Creates CD PR for staging ✅
7. CI completes ✅
```

---

## Why CI Might Not Run

### 1. GitHub Actions Disabled
- Check: Settings → Actions → General
- Fix: Enable "Allow all actions and reusable workflows"

### 2. Workflow File Issues
- Check: `.github/workflows/ci.yml` exists
- Fix: Ensure file is in correct location

### 3. Branch Protection Rules
- Check: Settings → Branches → Protection rules
- Fix: Ensure workflows are allowed to run

### 4. Syntax Errors in YAML
- Check: GitHub Actions tab for error messages
- Fix: Validate YAML syntax

### 5. Push Doesn't Match Trigger
- Check: You're pushing to a branch (not tags)
- Fix: `git push origin <branch-name>`

---

## Debugging CI Issues

### Step 1: Check if CI even started
1. Go to GitHub → Actions tab
2. Look for workflow runs
3. If no runs: Check triggers and permissions

### Step 2: Check which jobs ran
1. Click on the workflow run
2. See list of jobs
3. Check which succeeded/failed/skipped

### Step 3: Check job logs
1. Click on failed job
2. Read error messages
3. Look for specific errors

### Step 4: Check if conditions
1. Look at job conditions (`if:`)
2. Verify event type matches
3. Verify branch matches

---

## Quick Reference

| You Do | Trigger | Builds? | Pushes? | CD PR? |
|--------|---------|---------|---------|--------|
| Push to feature branch | push | ✅ | ❌ | ❌ |
| Push to main | push | ✅ | ✅ | ✅ |
| Create PR | pull_request | ✅ | ❌ | ❌ |
| Update PR | pull_request | ✅ | ❌ | ❌ |
| Manual (PB=false) | workflow_dispatch | ✅ | ❌ | ❌ |
| Manual (PB=true) | workflow_dispatch | ✅ | ✅ | ✅ |

**PB** = PublishBuild input

---

## Expected Behavior Summary

✅ **CI ALWAYS runs on:**
- Any push to any branch
- Any PR event (create, update, reopen, ready)
- Manual trigger

✅ **Images are pushed ONLY on:**
- Push to main branch
- Manual trigger with PublishBuild=true

✅ **CD PR is created ONLY when:**
- Images were pushed
- All builds succeeded
- All images verified in GHCR

❌ **CI NEVER pushes images on:**
- PR events
- Push to non-main branches (unless manual with PB=true)
