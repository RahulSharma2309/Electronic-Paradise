# Quick Start: Self-Hosted Runner Setup

## 🚀 Quick Setup (5 Minutes)

### Option 1: Automated Setup (Recommended)

1. **Run the setup script:**
   ```powershell
   # From repository root
   .\scripts\setup-github-runner.ps1
   ```

2. **Follow the prompts:**
   - Enter your GitHub repository URL
   - Get registration token from GitHub (see below)
   - Choose to install as service (recommended)

3. **Done!** ✅

### Option 2: Manual Setup

1. **Go to GitHub:**
   - Navigate to: `https://github.com/YOUR_USERNAME/Electronic-Paradise`
   - Click **Settings** → **Actions** → **Runners** → **New self-hosted runner**

2. **Download and configure:**
   ```powershell
   # Create folder
   mkdir $env:USERPROFILE\actions-runner
   cd $env:USERPROFILE\actions-runner
   
   # Download (get latest URL from GitHub)
   Invoke-WebRequest -Uri https://github.com/actions/runner/releases/download/v2.311.0/actions-runner-win-x64-2.311.0.zip -OutFile runner.zip
   
   # Extract
   Expand-Archive runner.zip
   Remove-Item runner.zip
   
   # Configure (use token from GitHub)
   .\config.cmd --url https://github.com/YOUR_USERNAME/Electronic-Paradise --token YOUR_TOKEN
   ```

3. **Install as service (optional but recommended):**
   ```powershell
   # Run PowerShell as Administrator
   .\svc.cmd install
   .\svc.cmd start
   ```

## ✅ Verify Setup

1. **Check runner in GitHub:**
   - Go to: **Settings** → **Actions** → **Runners**
   - Should see your runner as "Idle" (green)

2. **Test with test workflow:**
   - Go to: **Actions** tab → **Test Self-Hosted Runner** → **Run workflow**
   - Should complete successfully

## 🎯 Next Steps

1. ✅ Runner is set up
2. ✅ CD pipeline is configured (already done)
3. ✅ Push code to trigger CD pipeline
4. ✅ Monitor deployment in GitHub Actions

## 📚 Full Documentation

For detailed setup, troubleshooting, and maintenance, see:
- **Full Guide:** `docs/6-ci-cd/SELF_HOSTED_RUNNER_SETUP.md`

## 🔧 Common Commands

```powershell
# Start runner (if not running as service)
cd $env:USERPROFILE\actions-runner
.\run.cmd

# Check service status
.\svc.cmd status

# Restart service
.\svc.cmd restart

# Stop service
.\svc.cmd stop
```

## ❓ Troubleshooting

**Runner not appearing in GitHub?**
- Check if runner is running: `.\svc.cmd status`
- Check runner logs in `_diag` folder
- Verify internet connection

**kubectl not found?**
- Ensure Docker Desktop is installed
- kubectl comes with Docker Desktop
- Add to PATH if needed

**Need help?** See full guide: `docs/6-ci-cd/SELF_HOSTED_RUNNER_SETUP.md`
