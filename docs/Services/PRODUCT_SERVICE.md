# 📦 Product Service - Complete Documentation

**Product Catalog & Inventory Management Microservice**

---

## Table of Contents

1. [Service Overview](#1-service-overview)
2. [Architecture & Design](#2-architecture--design)
3. [Database Schema](#3-database-schema)
4. [API Endpoints](#4-api-endpoints)
5. [Business Logic Deep Dive](#5-business-logic-deep-dive)
6. [Stock Management](#6-stock-management)
7. [Service-to-Service Communication](#7-service-to-service-communication)
8. [Configuration](#8-configuration)
9. [Code Walkthrough](#9-code-walkthrough)
10. [Error Handling](#10-error-handling)
11. [Best Practices](#11-best-practices)

---

## 1. Service Overview

### Purpose

The **Product Service** is responsible for:

- Product catalog management
- Inventory (stock) management
- Stock reservation during order creation
- Stock release on order failure
- Product information retrieval
- Price management

### Key Responsibilities

| Responsibility         | Description                              |
| ---------------------- | ---------------------------------------- |
| **Catalog Management** | CRUD operations for products             |
| **Stock Management**   | Track available inventory                |
| **Stock Reservation**  | Reserve stock during checkout            |
| **Stock Release**      | Release reserved stock on order failure  |
| **Price Management**   | Store and retrieve product prices in INR |

### Technology Stack

- **.NET 8** - Framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database (productdb)
- **Repository Pattern** - Data access layer
- **Service Layer** - Business logic encapsulation

### Port & Database

- **Port**: `5002`
- **Database**: `productdb`
- **Docker Service Name**: `product-service`

---

## 2. Architecture & Design

### High-Level Architecture

```
┌──────────────────────────────────────────────┐
│         Frontend (React)                     │
│  Via Gateway → Get all products              │
│  Via Gateway → Get product by ID             │
└──────────────┬───────────────────────────────┘
               │
               ▼
┌──────────────────────────────────────────────┐
│          API Gateway (YARP)                  │
│  Routes /api/products/* → Product Service    │
└──────────────┬───────────────────────────────┘
               │
               ▼
┌──────────────────────────────────────────────┐
│        Product Service (Port 5002)           │
│                                              │
│  Controllers:                                │
│  • ProductsController - /api/products/*     │
│                                              │
│  Services:                                   │
│  • ProductServiceImpl - Business logic      │
│  • ProductValidator - Validation rules      │
│                                              │
│  Repositories:                               │
│  • ProductRepository - Data access          │
│                                              │
│  Data:                                       │
│  • AppDbContext - EF Core                   │
│  • Product entity                            │
└───────────────────┬──────────────────────────┘
                    │
                    ▼
        ┌────────────────────┐
        │   SQL Server       │
        │   productdb        │
        │                    │
        │  Products table    │
        └────────────────────┘

┌──────────────────────────────────────────────┐
│      Order Service (Port 5004)               │
│  Calls Product Service for:                  │
│  • Get product details                       │
│  • Validate stock availability               │
│  • Reserve stock during order                │
│  • Release stock on order failure            │
└──────────────────────────────────────────────┘
```

### Service Dependencies

**Depends On:**

- None (Product Service is independent)

**Depended On By:**

- **Order Service** (product info, stock reservation/release)
- **Frontend via Gateway** (product listing, details)

### Design Patterns

1. **Repository Pattern** - Data access abstraction
2. **Service Layer Pattern** - Business logic encapsulation
3. **Validator Pattern** - Domain validation logic
4. **DTO Pattern** - Data transfer objects for API contracts

---

## 3. Database Schema

### Database: `productdb`

### Table: `Products`

| Column        | Type           | Constraints                  | Description                     |
| ------------- | -------------- | ---------------------------- | ------------------------------- |
| `Id`          | Guid           | PRIMARY KEY, DEFAULT NEWID() | Product unique identifier       |
| `Name`        | nvarchar(200)  | NOT NULL                     | Product name                    |
| `Description` | nvarchar(1000) | NULL                         | Product description             |
| `Price`       | int            | NOT NULL, DEFAULT 0          | Price in paise (₹1 = 100 paise) |
| `Stock`       | int            | NOT NULL, DEFAULT 0          | Available stock quantity        |
| `CreatedAt`   | datetime2      | DEFAULT GETUTCDATE()         | Creation timestamp              |

### Entity Model

```csharp
public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    // Price in paise (cents) to avoid floating point issues
    // 1999 = ₹19.99
    public int Price { get; set; }

    // Available stock count
    public int Stock { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

### Key Characteristics

- **Integer Price**: Stored in paise (smallest unit) to avoid floating-point precision issues
  - `Price = 1999` represents `₹19.99`
  - Frontend converts: `price / 100` to display
- **Stock**: Decremented on reservation, incremented on release
- **Guid Primary Key**: Ensures unique, distributed ID generation
- **UTC Timestamps**: All timestamps in UTC

### Price Conversion

| Database (paise) | Display (INR) |
| ---------------- | ------------- |
| 1999             | ₹19.99        |
| 99900            | ₹999.00       |
| 50               | ₹0.50         |

---

## 4. API Endpoints

### Base Path: `/api/products`

### 1. **GET /api/products** - Get All Products

**Description**: Get all products in catalog

**Success Response (200 OK)**:

```json
[
  {
    "id": "product-guid-1",
    "name": "T-Shirt",
    "description": "Plain white T-Shirt",
    "price": 1999,
    "stock": 10
  },
  {
    "id": "product-guid-2",
    "name": "Coffee Mug",
    "description": "Ceramic mug",
    "price": 999,
    "stock": 5
  },
  {
    "id": "product-guid-3",
    "name": "Notebook",
    "description": "A5 ruled notebook",
    "price": 499,
    "stock": 0
  }
]
```

**Note**: Frontend converts price to INR: `price / 100` → `₹19.99`

---

### 2. **GET /api/products/{id}** - Get Product by ID

**Description**: Get product details by ID

**Parameters**:

- `id` (Guid) - Product ID

**Success Response (200 OK)**:

```json
{
  "id": "product-guid",
  "name": "T-Shirt",
  "description": "Plain white T-Shirt",
  "price": 1999,
  "stock": 10
}
```

**Error Responses**:

| Status | Description       |
| ------ | ----------------- |
| `404`  | Product not found |

---

### 3. **POST /api/products** - Create Product

**Description**: Create a new product (admin operation)

**Request Body**:

```json
{
  "name": "New Product",
  "description": "Product description",
  "price": 2999,
  "stock": 50
}
```

**Validation Rules**:

- ✅ Name: Required, not empty
- ✅ Price: Must be >= 0
- ✅ Stock: Must be >= 0

**Success Response (201 Created)**:

```json
{
  "id": "new-product-guid",
  "name": "New Product",
  "description": "Product description",
  "price": 2999,
  "stock": 50
}
```

**Error Responses**:

| Status | Error              | Description           |
| ------ | ------------------ | --------------------- |
| `400`  | Name is required   | Missing or empty name |
| `400`  | Price must be >= 0 | Negative price        |
| `400`  | Stock must be >= 0 | Negative stock        |

---

### 4. **POST /api/products/{id}/reserve** - Reserve Stock

**Description**: Reserve stock for an order (called by Order Service)

**Parameters**:

- `id` (Guid) - Product ID

**Request Body**:

```json
{
  "quantity": 2
}
```

**Success Response (200 OK)**:

```json
{
  "id": "product-guid",
  "remaining": 8
}
```

**Error Responses**:

| Status | Error                | Description                |
| ------ | -------------------- | -------------------------- |
| `400`  | Quantity must be > 0 | Invalid quantity           |
| `404`  | Product not found    | Invalid product ID         |
| `409`  | Insufficient stock   | Stock < quantity requested |

---

### 5. **POST /api/products/{id}/release** - Release Stock

**Description**: Release reserved stock (called on order failure)

**Parameters**:

- `id` (Guid) - Product ID

**Request Body**:

```json
{
  "quantity": 2
}
```

**Success Response (200 OK)**:

```json
{
  "id": "product-guid",
  "remaining": 10
}
```

**Error Responses**:

| Status | Error                | Description        |
| ------ | -------------------- | ------------------ |
| `400`  | Quantity must be > 0 | Invalid quantity   |
| `404`  | Product not found    | Invalid product ID |

---

## 5. Business Logic Deep Dive

### Product Creation Flow

```
1. Validate Input
   ├─ Name: Required, not empty
   ├─ Price: Must be >= 0
   └─ Stock: Must be >= 0

2. Create Product Entity
   ├─ Generate new Guid (Id)
   ├─ Set Name, Description
   ├─ Set Price (in paise)
   ├─ Set Stock
   └─ Set CreatedAt = UtcNow

3. Save to Database
   └─ EF Core SaveChangesAsync()

4. Return Product
   └─ Return created product with ID
```

### Code Implementation

```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var product = new Product
    {
        Name = dto.Name,
        Description = dto.Description,
        Price = dto.Price,
        Stock = dto.Stock,
        CreatedAt = DateTime.UtcNow
    };

    try
    {
        // Validate domain rules
        ProductValidator.ValidateForCreate(product);

        await _service.CreateAsync(product);

        return CreatedAtAction(nameof(Get), new { id = product.Id },
            new { product.Id, product.Name, product.Description,
                  product.Price, product.Stock });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { error = ex.Message });
    }
}
```

---

## 6. Stock Management

### Stock Reservation (Order Creation)

**Purpose**: Reserve stock when user places an order

**Flow**:

```
1. Validate Input
   └─ Quantity must be > 0

2. Get Product
   └─ By product ID (Guid)

3. Check Stock Availability
   ├─ If stock < quantity → throw InvalidOperationException
   └─ If stock >= quantity → proceed

4. Reserve Stock
   ├─ stock -= quantity
   └─ Update in database

5. Return Remaining Stock
   └─ Return int stock
```

**Code Implementation**:

```csharp
public async Task<int> ReserveAsync(Guid id, int quantity)
{
    if (quantity <= 0)
        throw new ArgumentException("Quantity must be greater than 0");

    var product = await _context.Products.FindAsync(id);
    if (product == null)
        throw new KeyNotFoundException($"Product not found: {id}");

    if (product.Stock < quantity)
        throw new InvalidOperationException(
            $"Insufficient stock. Available: {product.Stock}, Requested: {quantity}");

    product.Stock -= quantity;
    await _context.SaveChangesAsync();

    _logger.LogInformation("Reserved {Quantity} units of product {ProductId}. Remaining: {Stock}",
        quantity, id, product.Stock);

    return product.Stock;
}
```

### Stock Release (Order Failure/Rollback)

**Purpose**: Release reserved stock if order fails

**Flow**:

```
1. Validate Input
   └─ Quantity must be > 0

2. Get Product
   └─ By product ID (Guid)

3. Release Stock
   ├─ stock += quantity
   └─ Update in database

4. Return New Stock
   └─ Return int stock
```

**Code Implementation**:

```csharp
public async Task<int> ReleaseAsync(Guid id, int quantity)
{
    if (quantity <= 0)
        throw new ArgumentException("Quantity must be greater than 0");

    var product = await _context.Products.FindAsync(id);
    if (product == null)
        throw new KeyNotFoundException($"Product not found: {id}");

    product.Stock += quantity;
    await _context.SaveChangesAsync();

    _logger.LogInformation("Released {Quantity} units of product {ProductId}. New stock: {Stock}",
        quantity, id, product.Stock);

    return product.Stock;
}
```

### Stock Management Scenarios

| Scenario             | Action              | Example                       |
| -------------------- | ------------------- | ----------------------------- |
| **Order Created**    | Reserve stock       | Stock: 10 → 8 (qty: 2)        |
| **Payment Failed**   | Release stock       | Stock: 8 → 10 (qty: 2)        |
| **Order Successful** | Keep reserved       | Stock stays at 8              |
| **Low Stock**        | Prevent order       | Stock: 1, Request: 2 → 409    |
| **Out of Stock**     | Disable add to cart | Stock: 0 → UI disables button |

---

## 7. Service-to-Service Communication

### Incoming Calls

#### From Order Service

1. **Get Product Details**:

   ```
   GET /api/products/{id}
   Response: { id, name, description, price, stock }
   ```

2. **Reserve Stock**:

   ```
   POST /api/products/{id}/reserve
   Body: { quantity: 2 }
   Response: { id, remaining: 8 }
   ```

3. **Release Stock** (on order failure):
   ```
   POST /api/products/{id}/release
   Body: { quantity: 2 }
   Response: { id, remaining: 10 }
   ```

#### From Frontend (via Gateway)

1. **Get All Products**:

   ```
   GET /api/products
   Response: [ { id, name, price, stock, ... }, ... ]
   ```

2. **Get Product by ID**:
   ```
   GET /api/products/{id}
   Response: { id, name, price, stock, ... }
   ```

### No Outgoing Calls

Product Service does **not** call any other services. It's a leaf node in the dependency graph.

---

## 8. Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=productdb;User=sa;Password=YourPassword123!;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Docker Configuration

**docker-compose.yml**:

```yaml
product-service:
  build:
    context: ../services/product-service
    dockerfile: Dockerfile
  ports:
    - "5002:80"
  environment:
    - ASPNETCORE_ENVIRONMENT=Development
    - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=productdb;User=sa;Password=YourPassword123!;TrustServerCertificate=True;
  depends_on:
    - sqlserver
  networks:
    - ecommerce-network
  healthcheck:
    test: ["CMD", "curl", "-f", "http://localhost/health"]
    interval: 30s
    timeout: 10s
    retries: 3
```

### Seed Data

**Program.cs** - Seeds sample products for development:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product {
                Name = "T-Shirt",
                Description = "Plain white T-Shirt",
                Price = 1999,  // ₹19.99
                Stock = 10
            },
            new Product {
                Name = "Coffee Mug",
                Description = "Ceramic mug",
                Price = 999,  // ₹9.99
                Stock = 5
            },
            new Product {
                Name = "Notebook",
                Description = "A5 ruled notebook",
                Price = 499,  // ₹4.99
                Stock = 0
            }
        );
        db.SaveChanges();
    }
}
```

---

## 9. Code Walkthrough

### Project Structure

```
product-service/
├── Controllers/
│   └── ProductsController.cs       # API endpoints
├── Models/
│   └── Product.cs                  # Entity model
├── Services/
│   ├── IProductService.cs          # Service interface
│   ├── ProductServiceImpl.cs       # Service implementation
│   └── ProductValidator.cs         # Validation logic
├── Repositories/
│   ├── IProductRepository.cs       # Repository interface
│   └── ProductRepository.cs        # Repository implementation
├── Dtos/
│   ├── CreateProductDto.cs         # Create request DTO
│   ├── ReserveDto.cs               # Reserve stock DTO
│   └── ReleaseDto.cs               # Release stock DTO
├── Data/
│   └── AppDbContext.cs             # EF Core context
├── Startup.cs                      # Service configuration
├── Program.cs                      # Application entry + seed data
└── product-service.csproj          # Project file
```

### Product Validator

```csharp
public static class ProductValidator
{
    public static void ValidateForCreate(Product p)
    {
        if (string.IsNullOrWhiteSpace(p.Name))
            throw new ArgumentException("Name is required");

        if (p.Price < 0)
            throw new ArgumentException("Price must be >= 0");

        if (p.Stock < 0)
            throw new ArgumentException("Stock must be >= 0");
    }
}
```

### Repository Implementation

```csharp
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task<int> ReserveAsync(Guid id, int quantity)
    {
        // Implementation shown in section 6
    }

    public async Task<int> ReleaseAsync(Guid id, int quantity)
    {
        // Implementation shown in section 6
    }
}
```

---

## 10. Error Handling

### Error Response Format

```json
{
  "error": "Human-readable error message"
}
```

### Common Errors

| Scenario           | Status Code | Response                              |
| ------------------ | ----------- | ------------------------------------- |
| Product not found  | 404         | `NotFound()`                          |
| Invalid quantity   | 400         | `{ "error": "Quantity must be > 0" }` |
| Insufficient stock | 409         | `{ "error": "Insufficient stock" }`   |
| Invalid name       | 400         | `{ "error": "Name is required" }`     |
| Negative price     | 400         | `{ "error": "Price must be >= 0" }`   |
| Negative stock     | 400         | `{ "error": "Stock must be >= 0" }`   |

### Exception Handling

```csharp
[HttpPost("{id}/reserve")]
public async Task<IActionResult> Reserve(Guid id, ReserveDto dto)
{
    if (dto.Quantity <= 0)
        return BadRequest(new { error = "Quantity must be > 0" });

    try
    {
        var remaining = await _service.ReserveAsync(id, dto.Quantity);
        return Ok(new { id, remaining });
    }
    catch (KeyNotFoundException)
    {
        return NotFound();
    }
    catch (InvalidOperationException ex)
    {
        return Conflict(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Stock reservation failed for product {ProductId}", id);
        return StatusCode(500, new { error = "Stock reservation failed" });
    }
}
```

---

## 11. Best Practices

### ✅ Price Management

1. **Integer Storage**: Store prices in smallest unit (paise)
2. **No Floating Point**: Avoid `float` or `double` for currency
3. **Frontend Conversion**: Convert to display format in UI layer
4. **Consistency**: All prices in same unit across system

### ✅ Stock Management

1. **Atomic Operations**: Use database transactions
2. **Pessimistic Locking**: Consider row locking for high concurrency
3. **Reserve-Release Pattern**: Always release on failure
4. **Logging**: Log all stock operations for audit

### ✅ Validation

1. **Domain Validation**: Use validator pattern
2. **Early Returns**: Validate and fail fast
3. **Clear Errors**: Provide specific error messages
4. **Positive Values**: Ensure price and stock are non-negative

### ✅ Repository Pattern

1. **Separation of Concerns**: Data access logic isolated
2. **Testability**: Easy to mock repositories
3. **Maintainability**: Change data access without changing business logic

---

## 12. Future Enhancements

### Suggested Improvements

1. **Product Categories**

   - Add `CategoryId` field
   - Create `Categories` table
   - Support filtering by category

2. **Product Images**

   - Add `ImageUrl` field
   - Support multiple images
   - Use blob storage (Azure, S3)

3. **Product Variants**

   - Size, color, etc.
   - Each variant has own stock
   - SKU management

4. **Low Stock Alerts**

   - Email admin when stock < threshold
   - Automated reorder suggestions

5. **Soft Delete**

   - Add `IsDeleted` flag
   - Archive instead of hard delete
   - Maintain product history

6. **Price History**

   - Track price changes over time
   - Support discounts and promotions
   - Price effective dates

7. **Search & Filtering**

   - Full-text search on name/description
   - Filter by price range, stock availability
   - Pagination for large catalogs

8. **Stock Reservation Timeout**
   - Release reserved stock after N minutes if order not completed
   - Prevent indefinite locks

---

**Document Version:** 1.0  
**Last Updated:** December 24, 2025  
**Service Port:** 5002  
**Database:** productdb  
**Author:** MVP E-Commerce Team
