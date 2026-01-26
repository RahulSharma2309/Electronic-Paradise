# CD Pipeline - Local Kubernetes Deployment Setup

## Overview

This document explains how the CD (Continuous Deployment) pipeline works with your local Docker Desktop Kubernetes cluster using a self-hosted GitHub Actions runner.

---

## How It Works

### The Challenge

**Problem:** GitHub Actions runs in the cloud and cannot access your local Kubernetes cluster.

```
GitHub Actions (Cloud)          Your Local Machine
┌─────────────────────┐        ┌─────────────────────┐
│                     │        │                     │
│  CI/CD Pipeline     │   ❌   │  Docker Desktop K8s │
│  (GitHub Servers)   │  Can't │  (Your Computer)    │
│                     │  Reach │                     │
│                     │        │                     │
└─────────────────────┘        └─────────────────────┘
```

### The Solution

**Self-Hosted Runner:** Run GitHub Actions runner on your local machine, which can access your local Kubernetes cluster.

```
┌─────────────────────────────────────────────────────────┐
│         Your Local Machine                               │
│                                                         │
│  ┌──────────────────────┐                              │
│  │ GitHub Actions Runner │                              │
│  │ (Runs locally)        │                              │
│  └──────────────────────┘                              │
│         │                                               │
│         │ Can access local resources                   │
│         ▼                                               │
│  ┌──────────────────────┐                              │
│  │ Docker Desktop K8s    │                              │
│  │ kubectl commands      │                              │
│  └──────────────────────┘                              │
│                                                         │
└─────────────────────────────────────────────────────────┘
         │
         │ Communicates with GitHub
         ▼
┌─────────────────────────────────────────────────────────┐
│         GitHub (Cloud)                                  │
│                                                         │
│  - Receives code push                                   │
│  - Sends job to your runner                            │
│  - Receives job results                                │
└─────────────────────────────────────────────────────────┘
```

---

## Complete Flow

### 1. Code Push

```bash
git push origin main
```

### 2. CI Pipeline (Runs on GitHub)

- Builds and tests code
- Creates Docker images
- Pushes images to GitHub Container Registry (GHCR)

### 3. CD Pipeline (Runs on Your Machine)

**Trigger:** After CI completes successfully

**Steps:**
1. **Prepare Deployment** - Calculates version, gets Git SHA
2. **Verify Images** - Checks images exist in GHCR
3. **Deploy to Kubernetes** (runs on self-hosted runner):
   - Configures kubectl for Docker Desktop
   - Updates deployment YAMLs with new image tags
   - Applies Kubernetes manifests
   - Waits for deployments to be ready
4. **Smoke Tests** - Verifies services are running
5. **Deployment Summary** - Reports status

---

## Setup Steps

### Step 1: Install Self-Hosted Runner

**Quick Setup:**
```powershell
.\scripts\setup-github-runner.ps1
```

**Or Manual Setup:**
See: `docs/6-ci-cd/SELF_HOSTED_RUNNER_SETUP.md`

### Step 2: Verify Runner

1. Go to GitHub: **Settings** → **Actions** → **Runners**
2. Should see your runner as "Idle" (green)

### Step 3: Test Runner

1. Go to: **Actions** tab → **Test Self-Hosted Runner** → **Run workflow**
2. Should complete successfully

### Step 4: Trigger CD Pipeline

**Option 1: Push code**
```bash
git push origin main
```

**Option 2: Manual trigger**
- Go to: **Actions** tab → **CD Pipeline - Deploy to Staging** → **Run workflow**

---

## What Was Updated

### 1. CD Pipeline (`.github/workflows/cd-staging.yml`)

**Changes:**
- ✅ Changed `runs-on: ubuntu-latest` to `runs-on: self-hosted` for deployment jobs
- ✅ Fixed kubectl context configuration (uncommented and made it work)
- ✅ Updated all scripts to use PowerShell (Windows compatible)
- ✅ Fixed image tag update logic for Windows
- ✅ Updated smoke tests for Windows

### 2. Documentation

**Created:**
- ✅ `docs/6-ci-cd/SELF_HOSTED_RUNNER_SETUP.md` - Complete setup guide
- ✅ `docs/6-ci-cd/QUICK_START_RUNNER.md` - Quick start guide
- ✅ `docs/6-ci-cd/CD_PIPELINE_LOCAL_SETUP.md` - This document

### 3. Helper Scripts

**Created:**
- ✅ `scripts/setup-github-runner.ps1` - Automated runner setup script
- ✅ `.github/workflows/test-runner.yml` - Test workflow for runner

---

## How to Use

### First Time Setup

1. **Install runner:**
   ```powershell
   .\scripts\setup-github-runner.ps1
   ```

2. **Verify:**
   - Runner appears in GitHub
   - Test workflow runs successfully

3. **Done!** ✅

### Regular Usage

1. **Push code:**
   ```bash
   git push origin main
   ```

2. **CI Pipeline runs** (on GitHub)
   - Builds images
   - Pushes to GHCR

3. **CD Pipeline runs** (on your machine)
   - Deploys to local Kubernetes
   - Updates all services

4. **Access services:**
   - Frontend: `http://staging.electronic-paradise.local`
   - Services: `http://auth.staging.electronic-paradise.local/swagger`

---

## Troubleshooting

### CD Pipeline Fails

**Check:**
1. Is runner running? (`.\svc.cmd status`)
2. Is Docker Desktop running?
3. Is kubectl configured? (`kubectl config current-context`)
4. Check runner logs in `_diag` folder

### kubectl Not Found

**Solution:**
- Ensure Docker Desktop is installed
- kubectl comes with Docker Desktop
- Add to PATH if needed

### Cannot Access Cluster

**Solution:**
```powershell
kubectl config use-context docker-desktop
kubectl get nodes  # Should work
```

### Images Not Found

**Solution:**
- Ensure CI pipeline completed successfully
- Check images in GHCR: `https://github.com/YOUR_USERNAME?tab=packages`
- Verify image tags in deployment YAMLs

---

## Security Considerations

### ⚠️ Important Notes

1. **Self-hosted runners have full access to your machine**
   - Only use for trusted repositories
   - Don't run untrusted code

2. **Runner can access all files**
   - Use GitHub Secrets for sensitive data
   - Don't store credentials in code

3. **Keep runner updated**
   - GitHub releases updates regularly
   - Update runner periodically

---

## Next Steps

1. ✅ **Set up runner** (if not done)
2. ✅ **Test runner** with test workflow
3. ✅ **Push code** to trigger CD pipeline
4. ✅ **Monitor deployment** in GitHub Actions
5. ✅ **Verify services** are running

---

## Additional Resources

- **Full Setup Guide:** `docs/6-ci-cd/SELF_HOSTED_RUNNER_SETUP.md`
- **Quick Start:** `docs/6-ci-cd/QUICK_START_RUNNER.md`
- **GitHub Docs:** [Self-Hosted Runners](https://docs.github.com/en/actions/hosting-your-own-runners)

---

**Ready to deploy!** 🚀
