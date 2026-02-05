# Database Architecture & Setup Guide

This document explains the database architecture for FreshHarvest Market across different environments.

## Architecture Overview

The database layer is **separated from the application infrastructure** with three distinct configurations:

| Environment | Database Location | Database Names | Use Case |
|-------------|-------------------|----------------|----------|
| **Local Development** | LocalDB | `EP_Local_*` | Day-to-day development via `dotnet run` |
| **Local Staging** | LocalDB | `EP_Staging_*` | Testing with staging data locally |
| **Docker Integration Tests** | Docker container (`mssql`) | `authdb`, `productdb`, etc. | CI/CD, integration tests |
| **K8s Staging** | SQL Server Express (TCP/IP) | `EP_Staging_*` | K8s deployments (requires SQL Server Express) |

### Why This Separation?

1. **Local Dev databases** (`EP_Local_*`) - Fast iteration, your playground data
2. **Staging databases** (`EP_Staging_*`) - Mimics production, more controlled data
3. **Docker DB** - Isolated, disposable, perfect for automated tests

---

## Prerequisites

- **SQL Server LocalDB** - Comes with Visual Studio or can be installed separately
- **SQL Server Management Studio (SSMS)** - For database management
- **Docker Desktop** - For integration tests and K8s

---

## Quick Start (LocalDB)

### Step 1: Create All Databases

Run in PowerShell:

```powershell
cd c:\Users\Lenovo\source\repos\FreshHarvest-Market\infra\sql
.\create-localdb-databases.ps1
```

Or manually:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "CREATE DATABASE EP_Local_AuthDb; CREATE DATABASE EP_Local_UserDb; CREATE DATABASE EP_Local_ProductDb; CREATE DATABASE EP_Local_OrderDb; CREATE DATABASE EP_Local_PaymentDb; CREATE DATABASE EP_Staging_AuthDb; CREATE DATABASE EP_Staging_UserDb; CREATE DATABASE EP_Staging_ProductDb; CREATE DATABASE EP_Staging_OrderDb; CREATE DATABASE EP_Staging_PaymentDb;"
```

### Step 2: Verify in SSMS

1. Open SSMS
2. Connect to: `(localdb)\MSSQLLocalDB`
3. You should see all 10 databases

---

## Running Each Environment

### 1. Local Development (EP_Local_* databases)

```powershell
cd services/auth-service/src/AuthService.API
dotnet run
```

Default `ASPNETCORE_ENVIRONMENT=Development` uses `appsettings.Development.json` → `EP_Local_*` databases

### 2. Local Staging (EP_Staging_* databases)

```powershell
cd services/auth-service/src/AuthService.API
dotnet run --environment Staging
```

`ASPNETCORE_ENVIRONMENT=Staging` uses `appsettings.Staging.json` → `EP_Staging_*` databases

### 3. Docker Integration Tests

```powershell
cd infra
docker-compose up -d
```

Uses containerized `mssql` with `authdb`, `productdb`, etc. - completely isolated.

---

## Configuration Files

Each service has environment-specific settings:

```
services/auth-service/src/AuthService.API/
├── appsettings.json              # Base settings (fallback)
├── appsettings.Development.json  # EP_Local_AuthDb
└── appsettings.Staging.json      # EP_Staging_AuthDb
```

### Switching Environments

| Command | Environment | Database |
|---------|-------------|----------|
| `dotnet run` | Development | `EP_Local_*` |
| `dotnet run --environment Staging` | Staging | `EP_Staging_*` |
| `$env:ASPNETCORE_ENVIRONMENT="Staging"; dotnet run` | Staging | `EP_Staging_*` |

---

## Environment Matrix

```
┌──────────────────────────────────────────────────────────────────────┐
│                     DATABASE ARCHITECTURE                             │
├──────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  ┌─────────────────────┐     ┌────────────────────────────────────┐  │
│  │   Your Machine      │     │         LocalDB Instance           │  │
│  │                     │     │      (localdb)\MSSQLLocalDB        │  │
│  │  dotnet run         │────▶│  ┌────────────┐ ┌────────────────┐ │  │
│  │  (Development)      │     │  │EP_Local_*  │ │ EP_Staging_*   │ │  │
│  │                     │     │  │ databases  │ │  databases     │ │  │
│  │  dotnet run         │────▶│  └────────────┘ └────────────────┘ │  │
│  │  --environment      │     │        ▲               ▲           │  │
│  │  Staging            │     └────────┼───────────────┼───────────┘  │
│  └─────────────────────┘              │               │              │
│                                       │               │              │
│  ┌─────────────────────┐              │               │              │
│  │  Docker Compose     │     ┌────────┴───────────────┴───────────┐  │
│  │  (Integration Tests)│     │      mssql Container               │  │
│  │                     │────▶│   authdb, productdb, orderdb...    │  │
│  │  docker-compose up  │     │      (isolated, disposable)        │  │
│  └─────────────────────┘     └────────────────────────────────────┘  │
│                                                                       │
└──────────────────────────────────────────────────────────────────────┘
```

---

## Troubleshooting

### "Cannot open database" Error

The database doesn't exist. Run the setup script:
```powershell
.\infra\sql\create-localdb-databases.ps1
```

### LocalDB Not Starting

```powershell
# Check LocalDB status
sqllocaldb info MSSQLLocalDB

# Start LocalDB if stopped
sqllocaldb start MSSQLLocalDB
```

### Reset All Databases

```sql
-- Connect to (localdb)\MSSQLLocalDB and run:
DROP DATABASE IF EXISTS EP_Local_AuthDb;
DROP DATABASE IF EXISTS EP_Local_UserDb;
DROP DATABASE IF EXISTS EP_Local_ProductDb;
DROP DATABASE IF EXISTS EP_Local_OrderDb;
DROP DATABASE IF EXISTS EP_Local_PaymentDb;
DROP DATABASE IF EXISTS EP_Staging_AuthDb;
DROP DATABASE IF EXISTS EP_Staging_UserDb;
DROP DATABASE IF EXISTS EP_Staging_ProductDb;
DROP DATABASE IF EXISTS EP_Staging_OrderDb;
DROP DATABASE IF EXISTS EP_Staging_PaymentDb;
```

Then re-run `create-localdb-databases.ps1`.

---

## Note: K8s/Docker Limitations

**LocalDB does NOT support network connections.** Docker containers and K8s pods cannot connect to LocalDB.

For K8s staging deployments, you have two options:
1. **Use Docker mssql container** (current setup in `docker-compose.yml`)
2. **Install SQL Server Express** which supports TCP/IP on port 1433

The K8s secrets in `infra/k8s/staging/secrets/secrets.yaml` are configured for SQL Server Express (`host.docker.internal:1433`). If you only have LocalDB, use Docker Compose for containerized testing.
