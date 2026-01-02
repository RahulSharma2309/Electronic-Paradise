# Ep.Platform

Shared infrastructure hooks extracted from the backend services. Focuses only on what is currently used: SQL Server DbContext registration, JWT bearer auth, Swagger with bearer support, typed HttpClients, and a small CORS helper. Also includes a host extension to apply migrations/seed data.

## Provided extensions
- `AddEpSqlServerDbContext<TContext>(config, connectionName = "DefaultConnection")`
- `AddEpJwtAuth(config, sectionName = "Jwt")`
- `AddEpSwaggerWithJwt(title, version = "v1")`
- `AddEpHttpClient(name, config, baseAddressKey = "ServiceUrls:{name}")` with retry policy
- `AddEpDefaultCors(policyName = "AllowLocalhost3000", origins?)`
- `host.EnsureDatabaseAsync<TContext>(applyMigrations: true)` + optional `IDbSeeder<TContext>`

## Example wiring in a service Startup
```csharp
// using Ep.Platform.DependencyInjection;
// using Ep.Platform.Hosting;

public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddEpDefaultCors();
    services.AddEpSwaggerWithJwt("Product Service");
    services.AddEpSqlServerDbContext<AppDbContext>(_config);
    services.AddEpJwtAuth(_config);
    services.AddEpHttpClient("user", _config); // pulls ServiceUrls:user
}
```

In `Program.cs`:
```csharp
var app = builder.Build();
await app.EnsureDatabaseAsync<AppDbContext>();
```

## Packing & publishing (GitHub Packages)
1) Create (or reuse) `nuget.config` at repo root:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="github" value="https://nuget.pkg.github.com/<github-org-or-user>/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="<github-username>" />
      <add key="ClearTextPassword" value="<PAT_with_read_write_packages>" />
    </github>
  </packageSourceCredentials>
</configuration>
```

2) Pack:
```
dotnet pack platform/Ep.Platform/Ep.Platform.csproj -c Release -o ./.artifacts
```

3) Push to GitHub Packages:
```
dotnet nuget push ./.artifacts/Ep.Platform.<version>.nupkg --source github --api-key <PAT>
```

4) Consume from a service:
```xml
<PackageReference Include="Ep.Platform" Version="<version>" />
```
Ensure the same `nuget.config` (or a GitHub PAT env var) is available to restore.


