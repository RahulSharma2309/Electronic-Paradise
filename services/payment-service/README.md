# Payment Service

## Overview

The Payment Service handles all payment processing, refunds, and transaction recording using wallet-based payments.

### Responsibilities

- **Payment Processing**: Debit user wallet for orders
- **Refund Processing**: Credit user wallet on order cancellation
- **Transaction Recording**: Maintain payment history
- **Payment Status Queries**: Check payment status by order ID

---

## Architecture

**N-Tier Structure**:
```
PaymentService/
├── .build/                          ← Build configuration
├── docker/                          ← Standalone Docker setup
└── src/
    ├── PaymentService.Abstraction/  ← Models & DTOs
    ├── PaymentService.Core/         ← Business Logic & Repository
    └── PaymentService.API/          ← Controllers & Startup
```

---

## Build System

Uses centralized build configuration in `.build/` folder. See [Build Files Documentation](#build-files).

**Key Files**:
- `dependencies.props` - Package versions
- `src.props` - Source project settings
- `stylecop.json` - Code style rules
- `stylecop.ruleset` - Analyzer configuration

---

## API Endpoints

### Health Check
- `GET /api/health`

### Payment Operations
- `POST /api/payments/process` - Process payment (debits wallet)
- `POST /api/payments/refund` - Refund payment (credits wallet)
- `POST /api/payments/record` - Record payment transaction
- `GET /api/payments/status/{orderId}` - Get payment status

---

## Configuration

```bash
# Database
ConnectionStrings__DefaultConnection=Server=localhost,1433;Database=paymentdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;

# External Services
ServiceUrls__UserService=http://localhost:5005
```

---

## Running

### Local
```bash
cd services/payment-service/src/PaymentService.API
dotnet run
# Swagger: http://localhost:5003/swagger
```

### Docker Standalone
```bash
cd services/payment-service/docker
docker-compose up -d
```

---

## Database Schema

**Payments Table**:
- Id (Guid, PK)
- OrderId (Guid)
- UserId (Guid)
- Amount (decimal)
- Status (string) - "Paid", "Refunded"
- Timestamp (DateTime)

---

## Dependencies

### External Services
- **User Service** - For wallet operations

### Packages
- **Ep.Platform** (1.0.2) - Infrastructure abstractions

---

## Payment Flow

1. **Process Payment**:
   - Receive: OrderId, UserId, UserProfileId, Amount
   - Call User Service to debit wallet
   - Record payment with "Paid" status
   - Return payment record

2. **Refund Payment**:
   - Receive: OrderId, UserId, UserProfileId, Amount
   - Call User Service to credit wallet
   - Record payment with "Refunded" status (negative amount)
   - Return refund record

---

## Build Configuration

All build settings are inherited from `.build/` folder via `Directory.Build.props`.

To update versions:
```xml
<!-- .build/dependencies.props -->
<PropertyGroup Label="Platform and Core Packages">
  <EpPlatformVersion>1.0.2</EpPlatformVersion>
</PropertyGroup>
```

---

## Error Handling

- **Insufficient Balance**: Returns 409 Conflict
- **User Not Found**: Returns 404 Not Found
- **Service Unavailable**: Returns 503 Service Unavailable

---

## Links

- [Platform Library](../../platform/Ep.Platform/README.md)
- [User Service](../user-service/README.md)




