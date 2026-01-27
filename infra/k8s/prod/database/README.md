# Production Database Configuration

SQL Server deployment for the production environment.

## Resources

- **mssql-secret.yaml**: SA password and connection strings
- **mssql-pvc.yaml**: Persistent storage (10Gi - larger for prod)
- **mssql-deployment.yaml**: SQL Server 2019 Express pod
- **mssql-service.yaml**: Internal ClusterIP service

## Deployment

```bash
kubectl apply -f infra/k8s/prod/database/
```

## Connection Details

- **Service Name**: `mssql`
- **Port**: 1433
- **SA Password**: ProdStrong@Passw0rd123 (change for real production!)
- **Connection String**: `Server=mssql,1433;Database=DB_NAME;User Id=sa;Password=ProdStrong@Passw0rd123;TrustServerCertificate=True;`

## Storage

Uses Docker Desktop's `hostpath` StorageClass - 10Gi for production data.

## Production Recommendations

For real production environments:
- Use Azure SQL Database or AWS RDS (managed service)
- Implement automated backups
- Use sealed-secrets or Azure Key Vault for passwords
- Enable SQL Server Always On availability groups
- Configure monitoring and alerting
