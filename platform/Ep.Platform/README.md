# Ep.Platform

Shared infrastructure library for FreshHarvest Market microservices.

## Quick Reference

**Ep.Platform** provides infrastructure abstractions (Entity Framework, JWT, Swagger, HTTP clients, CORS) so services don't need direct dependencies on third-party libraries.

### Available Extensions

- `AddEpSqlServerDbContext<TContext>()` - SQL Server DbContext setup
- `AddEpJwtAuth()` - JWT Bearer authentication
- `AddEpSwaggerWithJwt()` - Swagger with JWT support
- `AddEpHttpClient()` - Typed HTTP clients with retry policy
- `AddEpDefaultCors()` - CORS configuration
- `EnsureDatabaseAsync<TContext>()` - Database initialization

### Quick Example

```csharp
// Startup.cs
services.AddEpSqlServerDbContext<AppDbContext>(Configuration);
services.AddEpJwtAuth(Configuration);
services.AddEpSwaggerWithJwt("My Service");
services.AddEpHttpClient("user", Configuration);

// Program.cs
await app.EnsureDatabaseAsync<AppDbContext>();
```

## 📚 Documentation

For complete documentation, see:
- **[Platform Guide](../../docs/8-platform/PLATFORM_GUIDE.md)** - Comprehensive usage guide
- **[Platform Architecture](../../docs/6-architecture/PLATFORM_ARCHITECTURE.md)** - Design and architecture
- **[Release Guide](RELEASE_GUIDE.md)** - How to publish new versions

## 📦 Publishing

To release a new version, see **[RELEASE_GUIDE.md](RELEASE_GUIDE.md)** for step-by-step instructions.





