# User Service

## Overview

The User Service manages user profiles, personal information, and wallet balance in the Electronic-Paradise e-commerce platform.

### Responsibilities

- **User Profile Management**: CRUD operations for user profiles
- **Wallet Management**: Handle wallet balance (debit/credit operations)
- **Phone Number Validation**: Ensure unique phone numbers
- **Profile Queries**: Retrieve user information by ID or UserId

---

## Architecture

**N-Tier Structure**:
```
UserService/
├── .build/                        ← Build configuration
├── docker/                        ← Standalone Docker setup
└── src/
    ├── UserService.Abstraction/   ← Models & DTOs
    ├── UserService.Core/          ← Business Logic & Repository
    └── UserService.API/           ← Controllers & Startup
```

---

## Build System

This service uses the same build system as all Electronic-Paradise microservices. See the **Build Files** section below for details.

### Build Files

All build configuration is in the `.build/` folder:

| File | Purpose |
|------|---------|
| `dependencies.props` | Centralized package version management |
| `src.props` | Source project settings (nullable, analyzers, documentation) |
| `test.props` | Test project settings (xUnit, code coverage) |
| `stylecop.json` | StyleCop analyzer configuration |
| `stylecop.ruleset` | Code analysis rules |

### How It Works

```
UserService.API.csproj
      ↓ (auto imports)
src/Directory.Build.props
      ↓ (imports)
../.build/src.props
      ↓ (imports)
../.build/dependencies.props
```

**Benefits**:
- ✅ Single source of truth for versions
- ✅ Consistent settings across all projects
- ✅ Easy to update dependencies
- ✅ Clean project files

---

## API Endpoints

### Health Check
- `GET /api/health`

### User Management
- `GET /api/users/{id}` - Get user by profile ID
- `GET /api/users/by-userid/{userId}` - Get user by auth user ID
- `GET /api/users/phone-exists/{phoneNumber}` - Check phone exists
- `POST /api/users` - Create user profile
- `PUT /api/users/{id}` - Update user profile

### Wallet Operations
- `POST /api/users/{id}/wallet/debit` - Debit from wallet
- `POST /api/users/{id}/wallet/credit` - Credit to wallet
- `POST /api/users/add-balance` - Add balance by user ID

---

## Configuration

```bash
# Database
ConnectionStrings__DefaultConnection=Server=localhost,1433;Database=userdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;
```

---

## Running

### Local
```bash
cd services/user-service/src/UserService.API
dotnet run
# Swagger: http://localhost:5005/swagger
```

### Docker Standalone
```bash
cd services/user-service/docker
docker-compose up -d
```

---

## Database Schema

**Users Table**:
- Id (Guid, PK)
- UserId (Guid, unique) - Links to Auth Service
- FirstName, LastName (string)
- Address (string)
- PhoneNumber (string, unique)
- WalletBalance (decimal)
- CreatedAt, UpdatedAt (DateTime)

---

## Dependencies

- **Ep.Platform** (1.0.2) - Infrastructure abstractions
- **SQL Server** - Database

---

## Build Configuration

To update package versions, edit `.build/dependencies.props`:

```xml
<PropertyGroup Label="Platform and Core Packages">
  <EpPlatformVersion>1.0.2</EpPlatformVersion>
</PropertyGroup>
```

All projects in `src/` automatically inherit these settings via `Directory.Build.props`.

---

## Links

- [Platform Library](../../platform/Ep.Platform/README.md)
- [Full Stack Setup](../../infra/docker-compose.yml)















