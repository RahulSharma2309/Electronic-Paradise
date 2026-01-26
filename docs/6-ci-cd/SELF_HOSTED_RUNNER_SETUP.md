# Self-Hosted GitHub Actions Runner Setup Guide

## Overview

This guide will help you set up a self-hosted GitHub Actions runner on your local machine so that the CD pipeline can deploy to your local Docker Desktop Kubernetes cluster.

---

## Why Self-Hosted Runner?

**Problem:** GitHub Actions runs in the cloud and cannot access your local Kubernetes cluster.

**Solution:** Run GitHub Actions runner on your local machine, which can access your local Docker Desktop Kubernetes.

---

## Prerequisites

- ✅ Windows 10/11
- ✅ Docker Desktop with Kubernetes enabled
- ✅ kubectl installed and configured
- ✅ Git installed
- ✅ Administrator access (for installation)

---

## Step-by-Step Setup

### Step 1: Download and Install the Runner

1. **Go to your GitHub repository**
   - Navigate to: `https://github.com/YOUR_USERNAME/Electronic-Paradise`
   - Click **Settings** → **Actions** → **Runners**

2. **Click "New self-hosted runner"**

3. **Select your operating system**
   - Choose: **Windows**
   - Choose: **x64** (or ARM64 if you have ARM processor)

4. **Follow the displayed instructions**

   GitHub will show you commands like:
   ```powershell
   # Create a folder
   mkdir actions-runner && cd actions-runner
   
   # Download the latest runner package
   Invoke-WebRequest -Uri https://github.com/actions/runner/releases/download/v2.311.0/actions-runner-win-x64-2.311.0.zip -OutFile actions-runner-win-x64-2.311.0.zip
   
   # Extract the installer
   Add-Type -AssemblyName System.IO.Compression.FileSystem ; [System.IO.Compression.ZipFile]::ExtractToDirectory("$PWD/actions-runner-win-x64-2.311.0.zip", "$PWD")
   ```

5. **Run the configuration script**

   ```powershell
   # Configure the runner
   .\config.cmd --url https://github.com/YOUR_USERNAME/Electronic-Paradise --token YOUR_TOKEN
   ```

   **Important:** Replace:
   - `YOUR_USERNAME` with your GitHub username
   - `YOUR_TOKEN` with the token shown on GitHub (expires in 1 hour)

6. **Choose configuration options**

   When prompted:
   ```
   Enter the name of the runner: [Press Enter for default]
   > docker-desktop-runner
   
   Enter the name of the work folder: [Press Enter for default]
   > _work
   
   Enter additional labels: [Press Enter for none]
   > 
   
   Enter name of the runner group: [Press Enter for default]
   > 
   
   Do you want to run the runner as a service? (Y/N) [Press N for now]
   > N
   ```

### Step 2: Test the Runner

1. **Start the runner**

   ```powershell
   .\run.cmd
   ```

2. **Verify it's connected**

   - Go back to GitHub: **Settings** → **Actions** → **Runners**
   - You should see your runner listed as **"Idle"** (green)

3. **Test with a simple workflow**

   Create a test workflow to verify it works:
   ```yaml
   # .github/workflows/test-runner.yml
   name: Test Runner
   on:
     workflow_dispatch:
   jobs:
     test:
       runs-on: self-hosted
       steps:
         - name: Test
           run: |
             echo "Runner is working!"
             kubectl version --client
   ```

   - Go to **Actions** tab → **Test Runner** → **Run workflow**
   - It should run on your self-hosted runner

### Step 3: Configure Runner as a Service (Optional but Recommended)

Running as a service means the runner starts automatically with Windows and runs in the background.

1. **Stop the runner** (if running)
   - Press `Ctrl+C` in the terminal

2. **Install as a service**

   ```powershell
   # Run PowerShell as Administrator
   .\svc.cmd install
   ```

3. **Start the service**

   ```powershell
   .\svc.cmd start
   ```

4. **Verify service is running**

   ```powershell
   .\svc.cmd status
   ```

   Or check Windows Services:
   - Press `Win + R` → Type `services.msc`
   - Look for "GitHub Actions Runner"

### Step 4: Update CD Pipeline

The CD pipeline needs to be updated to use the self-hosted runner. This has already been done in `.github/workflows/cd-staging.yml`.

**Key changes:**
```yaml
deploy-to-staging:
  runs-on: self-hosted  # ← Changed from ubuntu-latest
  steps:
    - name: Configure kubectl context
      run: |
        kubectl config use-context docker-desktop
```

### Step 5: Verify kubectl Configuration

1. **Check current context**

   ```powershell
   kubectl config current-context
   # Should output: docker-desktop
   ```

2. **If not docker-desktop, switch context**

   ```powershell
   kubectl config get-contexts
   kubectl config use-context docker-desktop
   ```

3. **Test kubectl access**

   ```powershell
   kubectl get nodes
   kubectl get namespaces
   ```

---

## Troubleshooting

### Issue 1: Runner Not Appearing in GitHub

**Symptoms:** Runner doesn't show up in GitHub UI

**Solutions:**
1. Check if runner is running: `.\run.cmd` (or check service status)
2. Verify token is correct (tokens expire in 1 hour)
3. Check firewall/antivirus blocking connection
4. Verify internet connection

### Issue 2: Runner Shows "Offline"

**Symptoms:** Runner appears in GitHub but shows as offline

**Solutions:**
1. Restart the runner: `.\svc.cmd restart` (if service) or `.\run.cmd` (if manual)
2. Check Windows Services if running as service
3. Verify network connectivity
4. Check runner logs in `_diag` folder

### Issue 3: kubectl Not Found in Runner

**Symptoms:** CD pipeline fails with "kubectl: command not found"

**Solutions:**
1. **Add kubectl to PATH**

   ```powershell
   # Find kubectl location
   where.exe kubectl
   
   # Add to system PATH (requires admin)
   # System Properties → Environment Variables → System Variables → Path → Edit → Add kubectl path
   ```

2. **Or use full path in workflow**

   ```yaml
   - name: Configure kubectl
     run: |
       C:\Users\${{ env.USERNAME }}\AppData\Local\Programs\Docker\Docker\resources\bin\kubectl.exe config use-context docker-desktop
   ```

3. **Or install kubectl in runner directory**

   ```powershell
   # Download kubectl
   Invoke-WebRequest -Uri https://dl.k8s.io/release/v1.28.0/bin/windows/amd64/kubectl.exe -OutFile kubectl.exe
   
   # Place in runner directory or add to PATH
   ```

### Issue 4: Docker Not Accessible

**Symptoms:** Pipeline fails with Docker-related errors

**Solutions:**
1. Ensure Docker Desktop is running
2. Add Docker to PATH or use full path
3. Check Docker Desktop settings → General → "Use WSL 2 based engine" (if applicable)

### Issue 5: Permission Denied Errors

**Symptoms:** kubectl commands fail with permission errors

**Solutions:**
1. Run runner as Administrator (not recommended for security)
2. Ensure kubectl context has proper permissions
3. Check RBAC settings in Kubernetes

---

## Security Considerations

### ⚠️ Important Security Notes

1. **Self-hosted runners have full access to your machine**
   - Only use for trusted repositories
   - Don't run untrusted code
   - Consider using separate user account

2. **Runner can access all files on your machine**
   - Be careful with secrets
   - Use GitHub Secrets for sensitive data
   - Don't store credentials in code

3. **Runner runs with user permissions**
   - Consider creating a dedicated service account
   - Limit permissions where possible

4. **Keep runner updated**
   - GitHub releases runner updates regularly
   - Update runner: `.\config.cmd remove` → Download new version → `.\config.cmd`

---

## Maintenance

### Update Runner

```powershell
# Stop runner
.\svc.cmd stop

# Remove old configuration
.\config.cmd remove

# Download latest version
# (Follow Step 1 again with latest version)

# Reconfigure
.\config.cmd --url https://github.com/YOUR_USERNAME/Electronic-Paradise --token NEW_TOKEN

# Install service again
.\svc.cmd install
.\svc.cmd start
```

### View Runner Logs

```powershell
# Service logs
Get-Content "_diag\Runner_*.log" -Tail 50

# Or check Windows Event Viewer
# Windows Logs → Application → Look for "GitHub Actions Runner"
```

### Remove Runner

```powershell
# Stop service
.\svc.cmd stop

# Uninstall service
.\svc.cmd uninstall

# Remove configuration
.\config.cmd remove

# Delete folder
cd ..
Remove-Item -Recurse -Force actions-runner
```

---

## Quick Reference

### Common Commands

```powershell
# Start runner (manual)
.\run.cmd

# Install as service
.\svc.cmd install

# Start service
.\svc.cmd start

# Stop service
.\svc.cmd stop

# Restart service
.\svc.cmd restart

# Check status
.\svc.cmd status

# Remove runner
.\config.cmd remove
```

### Runner Location

Default location: `C:\Users\YOUR_USERNAME\actions-runner`

You can install it anywhere, but recommended locations:
- `C:\actions-runner` (system-wide)
- `C:\Users\YOUR_USERNAME\actions-runner` (user-specific)

---

## Next Steps

After setting up the runner:

1. ✅ Verify runner appears in GitHub (Settings → Actions → Runners)
2. ✅ Test with a simple workflow
3. ✅ Update CD pipeline (already done)
4. ✅ Push code to trigger CD pipeline
5. ✅ Monitor deployment in GitHub Actions

---

## Additional Resources

- [GitHub Actions Runner Documentation](https://docs.github.com/en/actions/hosting-your-own-runners)
- [Self-Hosted Runner Security](https://docs.github.com/en/actions/security-guides/security-hardening-for-github-actions#using-self-hosted-runners)
- [Runner Configuration](https://docs.github.com/en/actions/hosting-your-own-runners/managing-self-hosted-runners/configuring-the-self-hosted-runner-application-as-a-service)

---

**Need Help?** Check the troubleshooting section or open an issue on GitHub!
