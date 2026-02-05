# Infrastructure Directory

This directory contains Docker Compose configuration and Kubernetes manifests for running all services.

## Quick Reference

> **📖 For complete setup instructions, see [SETUP_GUIDE.md](../SETUP_GUIDE.md) in the root directory.**
>
> **🗄️ For database setup (Local SQL Server vs Docker), see [DATABASE_SETUP.md](./DATABASE_SETUP.md)**

## Database Architecture

The database layer is **separated from the application** with three configurations:

| Environment | Command | Database |
|-------------|---------|----------|
| Local Development | `dotnet run` | Local SQL Server → `EP_Local_*` DBs |
| Docker (Integration Tests) | `docker-compose up` | Docker container → `authdb`, etc. |
| K8s Staging | `kubectl apply` | Local SQL Server → `EP_Staging_*` DBs |

**First-time setup:** Run `sql/setup-databases.sql` in SSMS to create all databases.

### Quick Commands

```powershell
# 1. Local Development (uses local SQL Server)
cd services/auth-service/src/AuthService.API
dotnet run

# 2. Docker with Docker DB (for integration tests)
cd infra
docker-compose up -d

# 3. Docker with Local DB (test containers against your local data)
cd infra
docker-compose -f docker-compose.yml -f docker-compose.local-db.yml up -d

# 4. Kubernetes Staging (uses local SQL Server via host.docker.internal)
kubectl apply -f k8s/staging/secrets/
kubectl apply -f k8s/staging/configmaps/
kubectl apply -f k8s/staging/deployments/
```

## Service URLs

- **Gateway**: http://localhost:5000
- **Auth Service**: http://localhost:5001
- **Product Service**: http://localhost:5002
- **Payment Service**: http://localhost:5003
- **Order Service**: http://localhost:5004
- **User Service**: http://localhost:5005
- **Frontend**: http://localhost:3000

## Files

- `docker-compose.yml` - Main orchestration (uses Docker mssql container)
- `docker-compose.local-db.yml` - Override to use local SQL Server instead
- `sql/setup-databases.sql` - Creates Local and Staging databases in SSMS
- `DATABASE_SETUP.md` - Complete database architecture guide
- `setup-env.ps1` / `setup-env.sh` - Setup scripts for GitHub token
- `k8s/` - Kubernetes manifests for staging and production

## Kubernetes Structure

```
k8s/
├── staging/
│   ├── configmaps/     # Non-sensitive config
│   ├── secrets/        # Connection strings (points to local SQL Server)
│   ├── deployments/    # Service deployments
│   ├── ingress/        # Routing rules
│   ├── database/       # Optional: containerized mssql (skip if using local SQL)
│   └── rbac/           # Service accounts
└── prod/               # Production manifests
```

## Troubleshooting

See [SETUP_GUIDE.md](../SETUP_GUIDE.md) and [DATABASE_SETUP.md](./DATABASE_SETUP.md) for detailed troubleshooting.
