# Auth Service

## Overview

The Authentication Service is a core microservice in the Electronic-Paradise e-commerce platform, responsible for user authentication, registration, password management, and JWT token generation.

### Responsibilities

- **User Registration**: Create new user accounts with email/password
- **User Authentication**: Validate credentials and issue JWT tokens
- **Password Management**: Reset and update user passwords
- **Token Generation**: Issue JWT tokens for authenticated sessions
- **Integration**: Communicates with User Service for profile creation

---

## Architecture

This service follows **N-Tier Architecture** with clear separation of concerns:

```
AuthService/
├── .build/                     ← Build configuration
├── docker/                     ← Standalone Docker setup
├── src/
│   ├── AuthService.Abstraction/    ← Models & DTOs
│   ├── AuthService.Core/           ← Business Logic & Repository
│   └── AuthService.API/            ← Controllers & Startup
└── test/                       ← Unit & Integration Tests
```

### Layers

1. **Abstraction Layer** (`AuthService.Abstraction`)
   - Domain models (User)
   - DTOs (RegisterDto, LoginDto, AuthResponseDto)
   - No dependencies

2. **Core Layer** (`AuthService.Core`)
   - Business logic (IAuthService)
   - Repository pattern (IUserRepository)
   - Data access (AppDbContext)
   - Depends on: Abstraction + Ep.Platform

3. **API Layer** (`AuthService.API`)
   - Controllers (AuthController, HealthController)
   - Startup configuration
   - Program.cs entry point
   - Depends on: Core + Abstraction + Ep.Platform

---

## Build System

### Build Files Structure

The service uses **MSBuild property files** for centralized configuration:

```
.build/
├── dependencies.props      ← Package version management
├── src.props              ← Source project settings
├── test.props             ← Test project settings
├── stylecop.json          ← StyleCop configuration
└── stylecop.ruleset       ← Code analysis rules
```

### File Descriptions

#### 1. `dependencies.props`

**Purpose**: Centralized package version management

**What it does**:
- Defines all NuGet package versions in one place
- Eliminates version conflicts across projects
- Makes version updates consistent

**Key sections**:
- Framework versions (net8.0, LangVersion 12)
- Platform packages (Ep.Platform, ASP.NET Core, EF Core)
- Testing packages (xUnit, Moq, FluentAssertions)
- Code quality tools (StyleCop, JetBrains Annotations)

**Example**:
```xml
<PropertyGroup Label="Platform and Core Packages">
  <EpPlatformVersion>1.0.2</EpPlatformVersion>
  <EntityFrameworkVersion>8.0.0</EntityFrameworkVersion>
</PropertyGroup>
```

---

#### 2. `src.props`

**Purpose**: Common build settings for all source projects

**What it does**:
- Imports `dependencies.props` for version management
- Sets target framework and language version
- Enables nullable reference types
- Configures documentation generation
- Sets up code analysis rules
- Adds code quality analyzers

**Key features**:
- `TreatWarningsAsErrors`: true (enforces clean code)
- `GenerateDocumentationFile`: true (XML docs)
- `Nullable`: enable (null safety)
- Automatic StyleCop and code analyzer inclusion

**Usage**: Automatically applied to all projects in `src/` via `Directory.Build.props`

---

#### 3. `test.props`

**Purpose**: Common build settings for test projects

**What it does**:
- Imports `dependencies.props` for version management
- Configures testing frameworks (xUnit, Moq)
- Sets up code coverage (Coverlet)
- Relaxes code analysis rules for tests
- Disables XML documentation for tests

**Key features**:
- All major testing frameworks included by default
- Code coverage enabled automatically
- Less strict warnings (TreatWarningsAsErrors: false)
- Documentation headers not required

**Usage**: Automatically applied to all projects in `test/` via `Directory.Build.props`

---

#### 4. `stylecop.json`

**Purpose**: StyleCop analyzer configuration

**What it does**:
- Defines company name and copyright text
- Configures code documentation rules
- Sets header decoration style
- Controls using directive placement

**Customization**:
```json
{
  "settings": {
    "documentationRules": {
      "companyName": "Electronic-Paradise",
      "copyrightText": "© Electronic-Paradise. All rights reserved."
    }
  }
}
```

---

#### 5. `stylecop.ruleset`

**Purpose**: Code analysis rule configuration

**What it does**:
- Defines which StyleCop rules are enforced
- Sets rule severity (Error, Warning, None)
- Relaxes rules for microservices development
- Ensures consistent code style

**Relaxed rules for microservices**:
- SA1600: Element documentation (None)
- SA1633: File header (None)
- SA1118: Parameter spacing (None)

---

#### 6. `Directory.Build.props` (in src/)

**Purpose**: Automatically apply build settings to all src projects

**What it does**:
- Imports `../.build/src.props`
- Automatically applied by MSBuild to all child projects
- Sets package tags for NuGet publishing

**How it works**:
MSBuild automatically discovers and imports this file for all `.csproj` files in the directory tree.

---

#### 7. `Directory.Build.props` (in test/)

**Purpose**: Automatically apply test settings to all test projects

**What it does**:
- Imports `../.build/test.props`
- Automatically applied to all test projects

---

## How the Build System Works

### Inheritance Chain

```
Individual .csproj
      ↓ (auto imports)
Directory.Build.props (src/)
      ↓ (imports)
.build/src.props
      ↓ (imports)
.build/dependencies.props
```

### Benefits

1. **Single Source of Truth**: All versions in one place
2. **Consistency**: All projects use same settings
3. **Easy Updates**: Change version once, applies everywhere
4. **Clean Project Files**: No repeated configuration
5. **Scalability**: Add new projects without configuration

---

## Configuration

### Required Environment Variables

```bash
# Database
ConnectionStrings__DefaultConnection=Server=localhost,1433;Database=authdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;

# JWT Settings
JwtOptions__Key=your-super-secret-key-that-should-be-at-least-32-characters-long-for-security
JwtOptions__Issuer=Electronic-Paradise
JwtOptions__Audience=Electronic-Paradise-Users

# Service URLs
ServiceUrls__UserService=http://localhost:5005
```

---

## Running the Service

### Local Development

```bash
# Navigate to API project
cd services/auth-service/src/AuthService.API

# Run the service
dotnet run

# Access Swagger UI
http://localhost:5001/swagger
```

### Docker (Standalone)

```bash
# Navigate to docker folder
cd services/auth-service/docker

# Start service with dependencies
docker-compose up -d

# View logs
docker-compose logs -f auth-service

# Stop
docker-compose down
```

### Docker (Full Stack)

```bash
# From repo root
cd infra
docker-compose up -d
```

---

## Building

### Build Locally

```bash
# Build all projects
dotnet build services/auth-service/AuthService.sln

# Build specific project
dotnet build services/auth-service/src/AuthService.API/AuthService.API.csproj
```

### Build with Docker

```bash
# Build Docker image
cd services/auth-service/src
docker build -t auth-service:latest .
```

---

## Testing

```bash
# Run all tests
dotnet test services/auth-service/test/

# Run with coverage
dotnet test services/auth-service/test/ /p:CollectCoverage=true

# Coverage report location
services/auth-service/artifacts/test/coverage.opencover.xml
```

---

## API Endpoints

### Health Check
- `GET /api/health` - Service health status

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Authenticate and get JWT token
- `POST /api/auth/reset-password` - Reset user password
- `GET /api/auth/me` - Get current user info (requires JWT)

---

## Dependencies

### External Services
- **User Service** (http://localhost:5005) - For user profile creation
- **SQL Server** - Database for user credentials

### NuGet Packages
- **Ep.Platform** (1.0.2) - Platform infrastructure abstractions
- **Microsoft.EntityFrameworkCore.Design** (8.0.0) - EF Core migrations

All other dependencies are abstracted by the Platform library.

---

## Database

### Migrations

```bash
# Add new migration
cd services/auth-service/src/AuthService.API
dotnet ef migrations add MigrationName --project ../AuthService.Core

# Apply migrations
dotnet ef database update --project ../AuthService.Core
```

### Schema

**Users Table**:
- Id (Guid, PK)
- Email (string, unique)
- PasswordHash (string)
- FullName (string, nullable)
- CreatedAt (DateTime)

---

## Modifying Build Configuration

### To Update Package Versions

1. Edit `.build/dependencies.props`
2. Change version in appropriate `<PropertyGroup>`
3. Rebuild solution

```xml
<PropertyGroup Label="Platform and Core Packages">
  <EpPlatformVersion>1.0.3</EpPlatformVersion>  ← Update here
</PropertyGroup>
```

### To Add New Code Analysis Rule

1. Edit `.build/stylecop.ruleset`
2. Add rule with desired action

```xml
<Rule Id="SA1234" Action="Warning" />
```

### To Change Company Name

1. Edit `.build/stylecop.json`
2. Update `companyName` and `copyrightText`

---

## Troubleshooting

### Build Failures

**Problem**: `MSBuild can't find .build/src.props`

**Solution**: Ensure `Directory.Build.props` path is correct:
```xml
<Import Project="$(MSBuildThisFileDirectory)../.build/src.props" />
```

**Problem**: Package version conflicts

**Solution**: Check all versions are defined in `dependencies.props`

### Code Analysis Warnings

**Problem**: Too many StyleCop warnings

**Solution**: Adjust rules in `.build/stylecop.ruleset` or disable specific rules

---

## Contributing

When adding new projects:
1. Place in appropriate folder (src/ or test/)
2. No need to add build configuration - auto-inherited
3. Follow existing naming conventions

---

## Links

- [Platform Library Documentation](../../platform/Ep.Platform/README.md)
- [Full Stack docker-compose](../../infra/docker-compose.yml)
- [Project Documentation](../../docs/)
