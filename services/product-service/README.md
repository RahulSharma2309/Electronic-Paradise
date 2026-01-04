# Product Service

## Overview

The Product Service manages the product catalog, inventory, and stock operations for Electronic-Paradise.

### Responsibilities

- **Product Catalog**: CRUD operations for products
- **Stock Management**: Reserve and release inventory
- **Inventory Queries**: Check product availability
- **Transactional Stock Operations**: Ensure data consistency

---

## Architecture

**N-Tier Structure**:
```
ProductService/
├── .build/                           ← Build configuration
├── docker/                           ← Standalone Docker setup
└── src/
    ├── ProductService.Abstraction/   ← Models & DTOs
    ├── ProductService.Core/          ← Business Logic & Repository
    └── ProductService.API/           ← Controllers & Startup
```

---

## Build System

Centralized build configuration ensures consistency and easy maintenance.

**Build Files** (`.build/` folder):
- `dependencies.props` - Centralized package versions
- `src.props` - Source project configuration
- `test.props` - Test project configuration
- `stylecop.json` - Code style settings
- `stylecop.ruleset` - Analysis rules

All projects automatically inherit settings via `Directory.Build.props`.

---

## API Endpoints

### Health Check
- `GET /api/health`

### Product Management
- `GET /api/products` - List all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product

### Stock Operations
- `POST /api/products/{id}/reserve` - Reserve stock (atomic)
- `POST /api/products/{id}/release` - Release stock (compensation)

---

## Configuration

```bash
# Database
ConnectionStrings__DefaultConnection=Server=localhost,1433;Database=productdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;
```

---

## Running

### Local
```bash
cd services/product-service/src/ProductService.API
dotnet run
# Swagger: http://localhost:5002/swagger
```

### Docker Standalone
```bash
cd services/product-service/docker
docker-compose up -d
```

---

## Database Schema

**Products Table**:
- Id (Guid, PK)
- Name (string, required)
- Description (string, nullable)
- Price (int)
- Stock (int)
- CreatedAt (DateTime)

---

## Stock Management

### Reserve Operation
**Transactional** - Uses database transaction to ensure atomicity

```csharp
// Reserve stock for an order
POST /api/products/{id}/reserve
Body: { "quantity": 2 }

// Response: { "id": "...", "remaining": 48 }
```

**Business Rules**:
- Quantity must be > 0
- Stock must be sufficient
- Operation is atomic (all or nothing)

### Release Operation
**Compensation** - Returns stock to inventory (used for rollback)

```csharp
// Release reserved stock
POST /api/products/{id}/release
Body: { "quantity": 2 }

// Response: { "id": "...", "remaining": 50 }
```

---

## Dependencies

- **Ep.Platform** (1.0.2) - Infrastructure abstractions
- **SQL Server** - Database

---

## Build Configuration

Edit `.build/dependencies.props` to update package versions:

```xml
<PropertyGroup Label="Platform and Core Packages">
  <EpPlatformVersion>1.0.2</EpPlatformVersion>
</PropertyGroup>
```

Changes automatically apply to all projects via `Directory.Build.props` inheritance.

---

## Error Handling

- **Insufficient Stock**: Returns 409 Conflict
- **Product Not Found**: Returns 404 Not Found
- **Invalid Quantity**: Returns 400 Bad Request

---

## Links

- [Platform Library](../../platform/Ep.Platform/README.md)
- [Full Stack Setup](../../infra/docker-compose.yml)















