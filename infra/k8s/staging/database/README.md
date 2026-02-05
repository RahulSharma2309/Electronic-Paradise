# Staging Database Configuration

This folder contains SQL Server deployment for running a containerized database in K8s.

## Database Options for Staging

You have **two options** for the staging database:

### Option 1: Local SQL Server (Recommended)

Connect K8s services to your local SQL Server installation (SSMS).

**Advantages:**
- Persistent data across cluster restarts
- Direct access to data via SSMS
- Faster startup (no container to spin up)
- Less resource usage

**Setup:**
1. Run `infra/sql/setup-databases.sql` in SSMS to create `EP_Staging_*` databases
2. **DO NOT** deploy this folder - skip `kubectl apply -f infra/k8s/staging/database/`
3. The service secrets in `infra/k8s/staging/secrets/secrets.yaml` are already configured to use `host.docker.internal` to connect to your local SQL Server

**Connection:**
- Services connect via `host.docker.internal,1433` → `EP_Staging_*` databases

---

### Option 2: Containerized SQL Server

Deploy SQL Server as a container inside K8s (using files in this folder).

**Advantages:**
- Fully isolated environment
- Easy to reset/destroy
- Good for CI/CD pipelines

**Setup:**
```bash
kubectl apply -f infra/k8s/staging/database/
```

**If using this option**, update `infra/k8s/staging/secrets/secrets.yaml`:
```yaml
# Change host.docker.internal back to mssql
ConnectionStrings__DefaultConnection: "Server=mssql,1433;Database=authdb;..."
```

---

## Files in This Folder

- **mssql-secret.yaml**: SA password
- **mssql-pvc.yaml**: Persistent storage (5Gi)
- **mssql-deployment.yaml**: SQL Server 2019 Express pod
- **mssql-service.yaml**: Internal ClusterIP service

## Connection Details (Containerized)

- **Service Name**: `mssql`
- **Port**: 1433
- **SA Password**: YourStrong@Passw0rd (local dev only!)
- **Connection String**: `Server=mssql,1433;Database=DB_NAME;...`

## Storage

Uses Docker Desktop's `hostpath` StorageClass - data persists on your local machine.

## Production Notes

For production:
- Use stronger passwords (via sealed secrets or external secrets)
- Consider managed databases (Azure SQL, AWS RDS)
- Enable backup policies
- Configure high availability
