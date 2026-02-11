# 🚀 Complete Setup Guide - Run Locally on Any Machine

This guide will walk you through setting up and running the FreshHarvest Market organic food marketplace on your local machine, step by step.

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- **Docker Desktop** (or Docker Engine + Docker Compose)
  - Download: https://www.docker.com/products/docker-desktop
  - Verify: `docker --version` and `docker-compose --version`
- **Git** (for cloning the repository)
  - Download: https://git-scm.com/downloads
  - Verify: `git --version`
- **GitHub Account** (for creating a Personal Access Token)
  - You'll need this to access the `Ep.Platform` NuGet package

---

## 📦 Step 1: Clone the Repository

Open a terminal (PowerShell on Windows, Terminal on Mac/Linux) and run:

```bash
git clone <repository-url>
cd FreshHarvest-Market
```

Replace `<repository-url>` with the actual repository URL.

**Verify:** You should see folders like `services/`, `infra/`, `gateway/`, etc.

---

## 🔑 Step 2: Create a GitHub Personal Access Token

The services need a GitHub token to access the `Ep.Platform` NuGet package during Docker builds.

### 2.1 Go to GitHub Settings

1. Open your browser and go to: https://github.com/settings/tokens
2. Click **"Generate new token"** → **"Generate new token (classic)"**

### 2.2 Configure the Token

1. **Note/Name**: Enter a name like "Docker Build Token" or "Ep.Platform Access"
2. **Expiration**: Choose your preferred expiration (or "No expiration" for convenience)
3. **Select scopes**: Check the box for **`read:packages`**
4. Click **"Generate token"** at the bottom

### 2.3 Copy the Token

⚠️ **IMPORTANT:** Copy the token immediately! You won't be able to see it again.

The token will look something like: `ghp_xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx`

---

## ⚙️ Step 3: Set Up GitHub Token (One-Time Setup)

### Option A: Using Setup Script (Recommended)

**Windows (PowerShell):**

```powershell
cd infra
.\setup-env.ps1
```

**Linux/Mac:**

```bash
cd infra
chmod +x setup-env.sh
./setup-env.sh
```

The script will:

1. Ask you to paste your GitHub token
2. Create a `.env` file in the `infra/` directory
3. Store the token securely (this file is already in `.gitignore`)

### Option B: Manual Setup

1. Copy the example file:

   ```bash
   cd infra
   cp .env.example .env
   ```

2. Open `.env` in a text editor and replace `your-github-token-here` with your actual token:

   ```
   GITHUB_TOKEN=ghp_your_actual_token_here
   ```

3. Save the file.

**Verify:** The `.env` file should exist in `infra/` directory with your token.

---

## 🐳 Step 4: Build Docker Images

This step downloads dependencies and builds all service containers. It may take 5-10 minutes the first time.

```bash
cd infra
docker-compose build
```

**What's happening:**

- Docker is building images for all services (auth, user, product, payment, order, gateway, frontend)
- It's downloading the `Ep.Platform` NuGet package using your GitHub token
- Building .NET applications inside containers

**Expected output:**

- You'll see build progress for each service
- Look for "Successfully built" messages
- If you see errors, check the troubleshooting section below

**Time:** First build: 5-10 minutes | Subsequent builds: 1-3 minutes

---

## 🚀 Step 5: Start All Services

Start all services in detached mode (runs in background):

```bash
docker-compose up -d
```

**What's happening:**

- Starting SQL Server container
- Starting all microservices (auth, user, product, payment, order)
- Starting API Gateway
- Starting Frontend

**Verify services are running:**

```bash
docker-compose ps
```

You should see all services with status "Up" or "Up (healthy)".

**Wait for services to be ready:**

- SQL Server: ~30 seconds
- Services: ~1-2 minutes
- Check logs: `docker-compose logs -f` (press Ctrl+C to exit)

---

## ✅ Step 6: Verify Everything is Working

### 6.1 Check Service Health

Open your browser and visit:

- **API Gateway**: http://localhost:5000
- **Health Check**: http://localhost:5000/api/health

### 6.2 Access Swagger Documentation

Each service has Swagger UI for testing:

- **Auth Service**: http://localhost:5001/swagger
- **User Service**: http://localhost:5005/swagger
- **Product Service**: http://localhost:5002/swagger
- **Order Service**: http://localhost:5004/swagger
- **Payment Service**: http://localhost:5003/swagger

### 6.3 Access Frontend

- **Frontend Application**: http://localhost:3000

---

## 🎯 Step 7: You're Ready!

The application is now running locally. You can:

- **Test APIs** using Swagger UIs
- **Access the frontend** at http://localhost:3000
- **View logs** with `docker-compose logs -f [service-name]`
- **Stop services** with `docker-compose down`
- **Restart services** with `docker-compose restart`

---

## 🛠️ Common Commands

### View Logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f user-service
```

### Stop Services

```bash
docker-compose down
```

### Stop and Remove Volumes (Clean Slate)

```bash
docker-compose down -v
```

### Restart Services

```bash
docker-compose restart
```

### Rebuild After Code Changes

```bash
docker-compose build
docker-compose up -d
```

---

## 🔧 Troubleshooting

### Issue: Build fails with "password cannot be empty" or "401 Unauthorized"

**Solution:**

1. Verify your `.env` file exists in `infra/` directory
2. Check that `GITHUB_TOKEN` is set correctly in `.env`
3. Verify your token has `read:packages` scope
4. Re-run the setup script: `.\setup-env.ps1` (Windows) or `./setup-env.sh` (Linux/Mac)

### Issue: Build fails with "package not found"

**Solution:**

1. Verify your GitHub token is valid and not expired
2. Check that token has `read:packages` scope
3. Try regenerating the token

### Issue: Services won't start or keep restarting

**Solution:**

1. Check logs: `docker-compose logs [service-name]`
2. Verify SQL Server is running: `docker-compose ps mssql`
3. Wait a bit longer - SQL Server takes ~30 seconds to start
4. Check port conflicts - ensure ports 5000-5005, 3000, 1433 are not in use

### Issue: Port already in use

**Solution:**

1. Find what's using the port:
   - Windows: `netstat -ano | findstr :5001`
   - Linux/Mac: `lsof -i :5001`
2. Stop the conflicting application
3. Or modify ports in `infra/docker-compose.yml`

### Issue: Docker Desktop not running

**Solution:**

1. Start Docker Desktop
2. Wait for it to fully start (whale icon in system tray)
3. Verify: `docker ps`

---

## 📚 Additional Resources

- **Project Overview**: See [README.md](README.md)
- **Architecture**: See [docs/6-architecture/SYSTEM_ARCHITECTURE.md](docs/6-architecture/SYSTEM_ARCHITECTURE.md)
- **Infrastructure Quick Reference**: See [infra/README.md](infra/README.md)

---

## 🎓 Next Steps

After setup, you might want to:

1. **Explore the codebase**: Start with [docs/START_HERE.md](docs/START_HERE.md)
2. **Understand the architecture**: Read [docs/6-architecture/SYSTEM_ARCHITECTURE.md](docs/6-architecture/SYSTEM_ARCHITECTURE.md)
3. **Test the APIs**: Use Swagger UIs to test endpoints
4. **Run services individually**: See [CONTRIBUTING.md](CONTRIBUTING.md) for local development

---

## 💡 Tips

- **First build is slow**: Subsequent builds are much faster due to Docker layer caching
- **Token persists**: Once you set up the `.env` file, you don't need to set it again
- **Use VS Code tasks**: Press `Ctrl+Shift+P` → "Tasks: Run Task" for quick Docker operations
- **Check logs first**: When something doesn't work, check logs with `docker-compose logs`

---

**Need help?** Check the troubleshooting section above or review the detailed documentation in the `docs/` folder.
