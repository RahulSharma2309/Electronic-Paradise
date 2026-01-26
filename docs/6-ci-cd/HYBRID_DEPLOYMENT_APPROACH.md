# 🚀 Hybrid Deployment Approach - Implementation Guide

## Overview

This document explains the **Hybrid Deployment Approach** implemented in Electronic Paradise. This approach combines the benefits of automated CI/CD with manual deployment control through Pull Requests.

---

## 🎯 How It Works

### Flow Diagram

```
┌─────────────────────────────────────────────────────────┐
│ 1. Developer merges code to main                        │
└──────────────────┬──────────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 2. CI Pipeline Runs                                      │
│    - Builds & tests code                                 │
│    - Creates Docker images                               │
│    - Pushes images to GHCR (ghcr.io)                    │
└──────────────────┬──────────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 3. CI Creates Deployment PR Automatically                │
│    - Updates image tags in k8s deployment files          │
│    - Includes any k8s infrastructure changes            │
│    - Creates PR with label "deploy-to-staging"          │
└──────────────────┬──────────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 4. Review & Approve Deployment PR                        │
│    - Team reviews changes                                │
│    - Option: Add "deploy-to-prod" label for production  │
│    - Merge PR to trigger deployment                     │
└──────────────────┬──────────────────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 5. CD Pipeline Runs (on PR merge)                        │
│    - Detects environment from PR label                  │
│    - Deploys to staging (default) or prod               │
│    - Runs smoke tests                                    │
└─────────────────────────────────────────────────────────┘
```

---

## 📋 Step-by-Step Process

### Step 1: Code Changes → Merge to Main

**What happens:**
- Developer pushes code or merges PR to `main` branch
- CI pipeline automatically triggers

**Files involved:**
- `.github/workflows/ci.yml` - CI pipeline definition

---

### Step 2: CI Pipeline Execution

**What happens:**
1. **Calculate Version** - Determines version from git tags
2. **Build & Test** - Compiles code, runs tests
3. **Docker Build** - Creates images for all 7 services
4. **Push to GHCR** - Images tagged as `v1.0.0`, `v1.0.0-abc123d`, `latest`
5. **Create Deployment PR** - Automatically creates PR with updated image tags

**Key Job:** `create-deployment-pr`
- Creates branch: `deploy/staging-v{VERSION}-{SHA}`
- Updates all deployment YAML files with new image tags
- Includes any k8s infrastructure changes from the merge
- Creates PR with default label: `deploy-to-staging`

**Example PR created:**
```
Title: 🚀 Deploy v1.1.0 to Staging
Branch: deploy/staging-v1.1.0-abc123d
Labels: deployment, staging, automated
```

---

### Step 3: Review Deployment PR

**What to check:**
- ✅ Image tags are correct (should be `v1.1.0`)
- ✅ K8s changes are included (if any)
- ✅ All services are updated

**To deploy to Production:**
1. Add label `deploy-to-prod` to the PR
2. Remove label `deploy-to-staging` (optional)
3. Merge the PR

**Default behavior:**
- Without `deploy-to-prod` label → Deploys to **staging**
- With `deploy-to-prod` label → Deploys to **production**

---

### Step 4: Merge Deployment PR

**What happens:**
- PR merge triggers CD pipeline
- CD pipeline detects environment from PR labels
- Deploys to the appropriate namespace

**Files involved:**
- `.github/workflows/cd-staging.yml` - CD pipeline definition

---

### Step 5: CD Pipeline Execution

**What happens:**
1. **Determine Environment** - Checks PR labels:
   - `deploy-to-prod` → Production
   - Default → Staging

2. **Verify Images** - Confirms images exist in GHCR

3. **Deploy to Kubernetes**:
   - Creates/updates namespace
   - Applies all k8s manifests
   - Updates deployments with new image tags
   - Waits for pods to be ready

4. **Smoke Tests** - Verifies services are healthy

5. **Deployment Summary** - Reports deployment status

---

## 🔧 Configuration

### CI Pipeline Job: `create-deployment-pr`

**Location:** `.github/workflows/ci.yml`

**Key Features:**
- Only runs on `main` branch after successful image build
- Creates branch with pattern: `deploy/staging-v{VERSION}-{SHA}`
- Updates image tags in all deployment files
- Automatically includes k8s infrastructure changes
- Creates PR with informative description

**Service Mapping:**
```yaml
auth-service → electronic-paradise-auth
user-service → electronic-paradise-user
product-service → electronic-paradise-product
order-service → electronic-paradise-order
payment-service → electronic-paradise-payment
gateway → electronic-paradise-gateway
frontend → electronic-paradise-frontend
```

---

### CD Pipeline: Environment Detection

**Location:** `.github/workflows/cd-staging.yml`

**Environment Detection Logic:**
```yaml
if PR has label "deploy-to-prod":
  → Deploy to production namespace
else:
  → Deploy to staging namespace (default)
```

**Namespaces:**
- Staging: `staging`
- Production: `prod`

---

## 🎨 PR Labels

### Default Labels (Auto-added)
- `deployment` - Marks as deployment PR
- `staging` - Default deployment target
- `automated` - Created by CI pipeline

### Optional Labels
- `deploy-to-prod` - Deploy to production instead of staging

---

## 📝 Example Scenarios

### Scenario 1: Normal Deployment to Staging

1. Merge code to `main`
2. CI runs → Images built → PR created
3. Review PR → Merge
4. CD runs → Deploys to staging ✅

### Scenario 2: Deploy to Production

1. Merge code to `main`
2. CI runs → Images built → PR created
3. **Add label `deploy-to-prod`** to PR
4. Review PR → Merge
5. CD runs → Deploys to production ✅

### Scenario 3: K8s Infrastructure Changes

1. Merge code to `main` (includes k8s changes)
2. CI runs → Images built → PR created
3. **PR automatically includes k8s changes** ✅
4. Review PR → Merge
5. CD runs → Deploys images + infrastructure changes ✅

---

## ✅ Benefits

1. **Control** - Manual review before deployment
2. **Transparency** - See exactly what will be deployed
3. **Flexibility** - Choose staging or production
4. **Safety** - No accidental deployments
5. **Automation** - PR creation is automatic
6. **Inclusion** - K8s changes automatically included

---

## 🔍 Troubleshooting

### Issue: PR not created after CI

**Check:**
- CI pipeline completed successfully
- Images were pushed to GHCR
- Job `create-deployment-pr` ran without errors

**Solution:**
- Check CI pipeline logs
- Verify `GITHUB_TOKEN` has write permissions

---

### Issue: Wrong environment deployed

**Check:**
- PR labels (should have `deploy-to-prod` for production)
- CD pipeline logs show detected environment

**Solution:**
- Verify PR labels before merging
- Check `determine-environment` job output

---

### Issue: K8s changes not included

**Check:**
- K8s files were changed in the merge to main
- CI job `create-deployment-pr` detected changes

**Solution:**
- K8s changes should be automatically included
- If not, check CI logs for `k8s-changes` step

---

## 🚀 Next Steps

1. **Test the flow:**
   - Merge a small change to main
   - Verify PR is created
   - Review and merge PR
   - Verify deployment succeeds

2. **Set up production namespace:**
   - Ensure `infra/k8s/prod/` has all required manifests
   - Create production secrets

3. **Configure access:**
   - Set up kubectl access for GitHub Actions
   - Create image pull secrets in both namespaces

---

## 📚 Related Documentation

- [CI Pipeline Guide](./README.md)
- [CD Pipeline Guide](./CD_PIPELINE_GUIDE.md)
- [Kubernetes Deployment](../11-kubernetes/README.md)

---

*Last updated: January 2026*
