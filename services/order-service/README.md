# Order Service

## Overview

The Order Service is the **orchestration layer** that coordinates User, Product, and Payment services to complete order transactions in Electronic-Paradise.

### Responsibilities

- **Order Creation**: Orchestrate multi-service order workflow
- **Order Queries**: Retrieve order details and history
- **Distributed Transaction Management**: Handle cross-service operations
- **Compensation Logic**: Automatic rollback on failures

---

## Architecture

**N-Tier Structure**:
```
OrderService/
├── .build/                         ← Build configuration
├── docker/                         ← Standalone Docker setup
└── src/
    ├── OrderService.Abstraction/   ← Models & DTOs
    ├── OrderService.Core/          ← Business Logic & Orchestration
    └── OrderService.API/           ← Controllers & Startup
```

---

## Build System

Centralized build configuration in `.build/` folder:

| File | Purpose |
|------|---------|
| `dependencies.props` | Package version management |
| `src.props` | Source project settings |
| `test.props` | Test project settings |
| `stylecop.json` | Code style configuration |
| `stylecop.ruleset` | Analysis rules |

**Inheritance Chain**:
```
OrderService.API.csproj
    ↓
src/Directory.Build.props
    ↓
../.build/src.props
    ↓
../.build/dependencies.props
```

---

## API Endpoints

### Health Check
- `GET /api/health`

### Order Operations
- `POST /api/orders/create` - Create new order (orchestrates all services)
- `GET /api/orders/{id}` - Get order details
- `GET /api/orders/user/{userId}` - Get user's order history

---

## Order Creation Workflow

The Order Service orchestrates a **multi-step distributed transaction**:

```
1. GET User Profile (User Service)
   ↓
2. Validate Products & Check Stock (Product Service)
   ↓
3. Process Payment - Debit Wallet (Payment Service)
   ↓
4. Reserve Stock (Product Service)
   ↓
5. Create Order (Local Database)
   ↓
6. Return Order Details

On ANY failure:
   ↓
Automatic Compensation:
- Refund Payment (if processed)
- Release Stock (if reserved)
```

### Compensation Logic

**Automatic Rollback** ensures data consistency:

```csharp
try {
    // Process payment
    await _paymentService.ProcessPaymentAsync(...)
    
    // Reserve stock
    await _productService.ReserveStockAsync(...)
    
    // Create order
    await _orderRepository.CreateAsync(...)
}
catch (Exception) {
    // Automatic compensation
    if (paymentProcessed) {
        await _paymentService.RefundAsync(...)
    }
    if (stockReserved) {
        await _productService.ReleaseStockAsync(...)
    }
    throw;
}
```

---

## Configuration

```bash
# Database
ConnectionStrings__DefaultConnection=Server=localhost,1433;Database=orderdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;

# External Services
ServiceUrls__UserService=http://localhost:5005
ServiceUrls__ProductService=http://localhost:5002
ServiceUrls__PaymentService=http://localhost:5003
```

---

## Running

### Local
```bash
cd services/order-service/src/OrderService.API
dotnet run
# Swagger: http://localhost:5004/swagger
```

### Docker Standalone
```bash
cd services/order-service/docker
docker-compose up -d
# This starts Order, User, Product, and Payment services
```

---

## Database Schema

**Orders Table**:
- Id (Guid, PK)
- UserId (Guid)
- TotalAmount (int)
- CreatedAt (DateTime)

**OrderItems Table**:
- Id (Guid, PK)
- OrderId (Guid, FK)
- ProductId (Guid)
- Quantity (int)
- UnitPrice (int)

---

## Dependencies

### External Services (All Required)
- **User Service** - Get user profile
- **Product Service** - Validate products, reserve stock
- **Payment Service** - Process payments

### Packages
- **Ep.Platform** (1.0.2) - Infrastructure abstractions, HttpClient with retry policy

---

## Order Creation Request

```json
POST /api/orders/create
{
  "userId": "guid-here",
  "items": [
    {
      "productId": "guid-here",
      "quantity": 2
    }
  ]
}
```

**Response** (Success):
```json
{
  "id": "order-guid",
  "userId": "user-guid",
  "totalAmount": 2000,
  "createdAt": "2026-01-02T10:00:00Z"
}
```

**Response** (Failure):
- 400 Bad Request - Invalid request data
- 404 Not Found - User or product not found
- 409 Conflict - Insufficient stock or balance
- 503 Service Unavailable - Dependent service unavailable

---

## Error Handling & Resilience

### Retry Policy
The Order Service uses **Polly retry policy** via Platform library:
- Automatic retries on transient failures
- Exponential backoff
- Circuit breaker pattern

### Compensation
All operations are **compensated on failure**:
1. Payment refunded if order creation fails
2. Stock released if payment fails
3. Atomic operations where possible

---

## Build Configuration

Update package versions in `.build/dependencies.props`:

```xml
<PropertyGroup Label="Platform and Core Packages">
  <EpPlatformVersion>1.0.2</EpPlatformVersion>
</PropertyGroup>
```

All projects automatically inherit via `Directory.Build.props`.

---

## Testing Order Creation

### Prerequisites
1. User Service running with created user
2. Product Service running with products in stock
3. User has sufficient wallet balance

### Example Flow
```bash
# 1. Create user (Auth Service)
POST /api/auth/register

# 2. Create user profile (User Service)
POST /api/users

# 3. Add wallet balance (User Service)
POST /api/users/add-balance

# 4. Create products (Product Service)
POST /api/products

# 5. Create order (Order Service)
POST /api/orders/create
```

---

## Complexity Level

**HIGH** - This service:
- Orchestrates 3 external services
- Implements distributed transaction patterns
- Handles complex error scenarios
- Requires compensation logic
- Most complex in the platform

---

## Links

- [Platform Library](../../platform/Ep.Platform/README.md)
- [User Service](../user-service/README.md)
- [Product Service](../product-service/README.md)
- [Payment Service](../payment-service/README.md)




