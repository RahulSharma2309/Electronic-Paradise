# Auth Service - N-Tier Architecture

This service handles authentication and user management using a clean n-tier architecture.

## Project Structure

```
auth-service/
├── src/
│   ├── AuthService.Abstraction/     # Models, DTOs, Constants
│   │   ├── Models/                  # Domain entities (User)
│   │   ├── DTOs/                    # Data Transfer Objects
│   │   └── Constants/               # Application constants
│   ├── AuthService.Core/            # Business Logic & Data Access
│   │   ├── Business/                # Business logic layer
│   │   │   ├── IJwtService.cs      # JWT token generation interface
│   │   │   ├── JwtService.cs       # JWT token implementation
│   │   │   ├── IAuthService.cs     # Auth business logic interface
│   │   │   └── AuthService.cs      # Auth business logic implementation
│   │   ├── Repository/              # Data access layer
│   │   │   ├── IUserRepository.cs  # User repository interface
│   │   │   └── UserRepository.cs   # User repository implementation
│   │   └── Data/                    # Database context
│   │       └── AppDbContext.cs     # EF Core DbContext
│   └── AuthService.API/             # Web API Layer
│       ├── Controllers/             # API controllers
│       │   ├── AuthController.cs   # Authentication endpoints
│       │   └── HealthController.cs # Health check endpoint
│       ├── Program.cs              # Application entry point
│       ├── Startup.cs              # Service & middleware configuration
│       └── appsettings.json        # Configuration
├── test/                            # Test projects (future)
└── AuthService.sln                  # Solution file

```

## Dependencies

### Ep.Platform NuGet Package

The service uses the `Ep.Platform` package (private NuGet package from GitHub Packages) which provides:

- **DbContext Extensions**: Simplified SQL Server DbContext configuration
- **JWT Authentication**: Standardized JWT authentication setup
- **Swagger Extensions**: Pre-configured Swagger with JWT bearer support
- **CORS Extensions**: Default CORS policy for local development
- **HttpClient Extensions**: Resilient HttpClient configuration with retry policies

### External Packages

- `Microsoft.EntityFrameworkCore.SqlServer` - SQL Server database provider
- `BCrypt.Net-Next` - Password hashing
- `System.IdentityModel.Tokens.Jwt` - JWT token generation
- `Swashbuckle.AspNetCore` - API documentation

## Architecture Layers

### 1. Abstraction Layer
Contains all models, DTOs, and constants that are shared across layers. No dependencies on other layers.

### 2. Core Layer
Contains business logic and data access:
- **Business**: Service classes implementing business rules and orchestration
- **Repository**: Data access interfaces and implementations using EF Core
- **Data**: DbContext and database configuration

### 3. API Layer
ASP.NET Core Web API exposing HTTP endpoints:
- **Controllers**: Handle HTTP requests/responses
- **Program.cs**: Minimal entry point that uses Startup class
- **Startup.cs**: ConfigureServices and Configure methods for DI and middleware
- Uses Ep.Platform extensions for common configurations

## Dependency Injection

Services are registered in `Startup.cs` using the traditional pattern:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Database
    services.AddEpSqlServerDbContext<AppDbContext>(Configuration);

    // JWT Authentication
    services.AddEpJwtAuth(Configuration);

    // Business Services
    services.AddSingleton<IJwtService>(new JwtService(...));
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IAuthService, AuthService>();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Middleware pipeline configuration
    app.UseRouting();
    app.UseCors("AllowLocalhost3000");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => endpoints.MapControllers());
}
```

## Configuration

Configure the service in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthDb;..."
  },
  "Jwt": {
    "Key": "YourSecretKey",
    "Issuer": "auth-service",
    "Audience": "auth-clients"
  },
  "ServiceUrls": {
    "UserService": "http://localhost:5005"
  }
}
```

## Running the Service

```bash
# Restore packages (ensure GITHUB_TOKEN is set for Ep.Platform)
dotnet restore

# Build
dotnet build

# Run
dotnet run --project src/AuthService.API
```

## API Endpoints

- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token
- `POST /api/auth/reset-password` - Reset user password
- `GET /api/auth/me` - Get current user info (requires auth)
- `GET /api/health` - Health check

## Development Notes

- The service communicates with the User Service to create user profiles during registration
- Phone number uniqueness is validated via User Service API
- Passwords are hashed using BCrypt before storage
- JWT tokens are valid for 6 hours by default
- Database is created automatically in development mode using `EnsureDatabaseAsync`

## Future Enhancements

- Add unit and integration tests in `test/` folder
- Implement email verification
- Add refresh token support
- Add rate limiting for authentication endpoints
- Implement password reset via email

