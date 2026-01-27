# Staging Database Configuration

SQL Server deployment for the staging environment.

## Resources

- **mssql-secret.yaml**: SA password and connection strings
- **mssql-pvc.yaml**: Persistent storage (5Gi)
- **mssql-deployment.yaml**: SQL Server 2019 Express pod
- **mssql-service.yaml**: Internal ClusterIP service

## Deployment

```bash
kubectl apply -f infra/k8s/staging/database/
```

## Connection Details

- **Service Name**: `mssql`
- **Port**: 1433
- **SA Password**: YourStrong@Passw0rd (for local dev only!)
- **Connection String**: `Server=mssql,1433;Database=DB_NAME;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;`

## Storage

Uses Docker Desktop's `hostpath` StorageClass - data persists on your local machine.

## Production Notes

For production:
- Use stronger passwords (via sealed secrets or external secrets)
- Consider managed databases (Azure SQL, AWS RDS)
- Enable backup policies
- Configure high availability
