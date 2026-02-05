# Product Service - Complete Documentation

**Product Catalog & Inventory Management Microservice**

---

## Table of Contents

1. [Service Overview](#1-service-overview)
2. [Database Schema](#2-database-schema)
3. [Entity Relationships](#3-entity-relationships)
4. [Tables Deep Dive](#4-tables-deep-dive)
5. [Architecture & Layers](#5-architecture--layers)
6. [API Endpoints](#6-api-endpoints)
7. [Business Logic](#7-business-logic)
8. [Configuration](#8-configuration)
9. [Code Structure](#9-code-structure)

---

## 1. Service Overview

### Purpose

The **Product Service** is the central catalog management system for FreshHarvest Market. It handles:

- **Product Catalog** - Store and manage all product information
- **Inventory Management** - Track stock levels and availability
- **Stock Reservation** - Reserve inventory during checkout
- **Category Taxonomy** - Organize products hierarchically
- **Product Discovery** - Tags, attributes, and SEO metadata
- **Certification Tracking** - Organic/quality certifications (for fresh products)

### Technology Stack

| Component | Technology |
|-----------|------------|
| Framework | .NET 10 |
| ORM | Entity Framework Core |
| Database | SQL Server (LocalDB) |
| Pattern | Repository + Service Layer |
| API | RESTful with Swagger |

### Service Info

| Property | Value |
|----------|-------|
| Port | `5002` |
| Database | `EP_Local_ProductDb` (Local) / `EP_Staging_ProductDb` (Staging) |
| Health Check | `GET /api/health` |
| Swagger | `http://localhost:5002/swagger` |

---

## 2. Database Schema

### Entity-Relationship Diagram

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                         PRODUCT SERVICE DATABASE SCHEMA                          │
├─────────────────────────────────────────────────────────────────────────────────┤
│                                                                                  │
│  ┌──────────────────┐         ┌──────────────────────────────────────────────┐  │
│  │    Categories    │         │                   Products                    │  │
│  ├──────────────────┤         ├──────────────────────────────────────────────┤  │
│  │ Id (PK)          │◄───────┐│ Id (PK)                                      │  │
│  │ Name             │        ││ Name                                         │  │
│  │ Slug (Unique)    │        ││ Description                                  │  │
│  │ Description      │        ││ Price (int - in paise)                       │  │
│  │ ParentId (FK)────┼──┐     ││ Stock                                        │  │
│  │ IsActive         │  │     ││ CategoryId (FK)─────────────────────────────►│  │
│  │ CreatedAt        │  │     ││ Brand                                        │  │
│  │ UpdatedAt        │  │     ││ Sku                                          │  │
│  └────────┬─────────┘  │     ││ Unit                                         │  │
│           │            │     ││ IsActive                                     │  │
│           └────────────┘     ││ CreatedAt                                    │  │
│      (Self-referencing       ││ UpdatedAt                                    │  │
│       for hierarchy)         │└──────────────┬───────────────────────────────┘  │
│                              │               │                                   │
│                              │               │ 1:N                               │
│                              │               ▼                                   │
│  ┌───────────────────────────┼───────────────┼───────────────────────────────┐  │
│  │                           │               │                               │  │
│  │  ┌────────────────────┐   │  ┌────────────┴───────────┐                   │  │
│  │  │   ProductImages    │   │  │   ProductAttributes    │                   │  │
│  │  ├────────────────────┤   │  ├────────────────────────┤                   │  │
│  │  │ Id (PK)            │   │  │ Id (PK)                │                   │  │
│  │  │ ProductId (FK)─────┼───┤  │ ProductId (FK)─────────┼───────────────┐   │  │
│  │  │ Url                │   │  │ Key                    │               │   │  │
│  │  │ AltText            │   │  │ Group                  │               │   │  │
│  │  │ SortOrder          │   │  │ Unit                   │               │   │  │
│  │  │ IsPrimary          │   │  │ ValueString            │               │   │  │
│  │  │ CreatedAt          │   │  │ ValueNumber            │               │   │  │
│  │  └────────────────────┘   │  │ ValueBoolean           │               │   │  │
│  │                           │  │ CreatedAt              │               │   │  │
│  │                           │  └────────────────────────┘               │   │  │
│  │                           │                                           │   │  │
│  │  ┌────────────────────┐   │  ┌────────────────────────┐               │   │  │
│  │  │ProductCertification│   │  │   ProductMetadata      │               │   │  │
│  │  ├────────────────────┤   │  ├────────────────────────┤               │   │  │
│  │  │ Id (PK)            │   │  │ Id (PK)                │               │   │  │
│  │  │ ProductId (FK,Uniq)┼───┤  │ ProductId (FK, Unique)─┼───────────────┤   │  │
│  │  │ CertificationNumber│   │  │ Slug (Unique)          │               │   │  │
│  │  │ CertificationType  │   │  │ SeoMetadataJson        │               │   │  │
│  │  │ Origin             │   │  │ CreatedAt              │               │   │  │
│  │  │ CertifyingAgency   │   │  │ UpdatedAt              │               │   │  │
│  │  │ IssuedDate         │   │  └────────────────────────┘               │   │  │
│  │  │ ExpiryDate         │   │                                           │   │  │
│  │  │ IsValid            │   │                                           │   │  │
│  │  │ ProductExpiryDate  │   │                                           │   │  │
│  │  │ Notes              │   │                                           │   │  │
│  │  │ CreatedAt          │   │                                           │   │  │
│  │  │ UpdatedAt          │   │                                           │   │  │
│  │  └────────────────────┘   │                                           │   │  │
│  │                           │                                           │   │  │
│  └───────────────────────────┼───────────────────────────────────────────┘   │  │
│                              │                                               │  │
│                              │                                               │  │
│  ┌───────────────────────────┼───────────────────────────────────────────┐   │  │
│  │       MANY-TO-MANY: Products ←→ Tags                                  │   │  │
│  │                              │                                        │   │  │
│  │  ┌──────────────┐    ┌──────┴───────┐    ┌──────────────┐            │   │  │
│  │  │    Tags      │    │  ProductTags │    │   Products   │            │   │  │
│  │  ├──────────────┤    ├──────────────┤    │   (above)    │            │   │  │
│  │  │ Id (PK)      │◄───┤ TagId (PK,FK)│    │              │            │   │  │
│  │  │ Name         │    │ ProductId(PK)├───►│              │            │   │  │
│  │  │ Slug (Unique)│    └──────────────┘    └──────────────┘            │   │  │
│  │  └──────────────┘     (Composite PK)                                  │   │  │
│  │                                                                       │   │  │
│  └───────────────────────────────────────────────────────────────────────┘   │  │
│                                                                              │  │
└──────────────────────────────────────────────────────────────────────────────┘
```

### Tables Summary

| Table | Purpose | Relationship to Product |
|-------|---------|-------------------------|
| `Products` | Core product data | - |
| `Categories` | Product taxonomy/hierarchy | Many Products → One Category |
| `ProductImages` | Product image gallery | One Product → Many Images |
| `ProductAttributes` | Flexible key-value specs | One Product → Many Attributes |
| `ProductCertifications` | Organic/quality certs | One Product → One Certification |
| `ProductMetadata` | SEO and slugs | One Product → One Metadata |
| `Tags` | Reusable labels | - |
| `ProductTags` | Product-Tag junction | Many-to-Many |

---

## 3. Entity Relationships

### Relationship Types

```
┌─────────────────────────────────────────────────────────────────┐
│                    RELATIONSHIP SUMMARY                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ONE-TO-MANY (1:N)                                              │
│  ────────────────                                               │
│  • Category → Products     (One category has many products)     │
│  • Product → Images        (One product has many images)        │
│  • Product → Attributes    (One product has many attributes)    │
│  • Category → Categories   (Self-ref: parent → children)        │
│                                                                  │
│  ONE-TO-ONE (1:1)                                               │
│  ────────────────                                               │
│  • Product → Certification (One product, one cert record)       │
│  • Product → Metadata      (One product, one metadata record)   │
│                                                                  │
│  MANY-TO-MANY (N:M)                                             │
│  ─────────────────                                              │
│  • Products ↔ Tags         (Via ProductTags junction table)     │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Delete Behaviors

| Relationship | On Delete | Meaning |
|--------------|-----------|---------|
| Category → Products | SET NULL | Products keep existing, CategoryId becomes null |
| Category → Parent | RESTRICT | Cannot delete parent with children |
| Product → Images | CASCADE | Delete product = delete all its images |
| Product → Attributes | CASCADE | Delete product = delete all its attributes |
| Product → Certification | CASCADE | Delete product = delete its certification |
| Product → Metadata | CASCADE | Delete product = delete its metadata |
| Product ↔ Tags | CASCADE | Delete product = remove from ProductTags |

---

## 4. Tables Deep Dive

### 4.1 Products (Core Table)

**Purpose:** Stores the main product information.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | GUID | PK, Default NewGuid | Unique identifier |
| `Name` | nvarchar(200) | Required | Product name |
| `Description` | nvarchar(2000) | Nullable | Detailed description |
| `Price` | int | Default 0 | Price in **paise** (₹1 = 100 paise) |
| `Stock` | int | Default 0 | Available inventory count |
| `CategoryId` | GUID | FK, Nullable | Link to Categories table |
| `Brand` | nvarchar(100) | Nullable | Manufacturer/brand name |
| `Sku` | nvarchar(100) | Nullable | Stock Keeping Unit code |
| `Unit` | nvarchar(50) | Nullable | Unit of measure (kg, piece, bunch) |
| `IsActive` | bit | Default true | Visibility flag |
| `CreatedAt` | datetime2 | Default UtcNow | Creation timestamp |
| `UpdatedAt` | datetime2 | Nullable | Last modification timestamp |

**Price Storage:** Prices are stored in **paise** (smallest currency unit) to avoid floating-point precision issues.

```
Database: 1999 paise → Display: ₹19.99
Database: 99900 paise → Display: ₹999.00
```

---

### 4.2 Categories (Taxonomy)

**Purpose:** Hierarchical product categorization (e.g., Electronics → Phones → Smartphones).

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | GUID | PK | Unique identifier |
| `Name` | nvarchar(200) | Required | Display name |
| `Slug` | nvarchar(200) | Required, Unique | URL-friendly identifier |
| `Description` | nvarchar(max) | Nullable | Category description |
| `ParentId` | GUID | FK (self), Nullable | Parent category for hierarchy |
| `IsActive` | bit | Default true | Visibility flag |
| `CreatedAt` | datetime2 | Default UtcNow | Creation timestamp |
| `UpdatedAt` | datetime2 | Nullable | Last modification timestamp |

**Hierarchy Example:**
```
Electronics (ParentId: null)
├── Phones (ParentId: Electronics.Id)
│   ├── Smartphones (ParentId: Phones.Id)
│   └── Feature Phones (ParentId: Phones.Id)
└── Laptops (ParentId: Electronics.Id)
```

---

### 4.3 ProductImages (Gallery)

**Purpose:** Store multiple images per product with ordering and primary flag.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | GUID | PK | Unique identifier |
| `ProductId` | GUID | FK, Required | Link to product |
| `Url` | nvarchar(500) | Required | Image URL (CDN/storage) |
| `AltText` | nvarchar(250) | Nullable | Accessibility text |
| `SortOrder` | int | Default 0 | Display ordering |
| `IsPrimary` | bit | Default false | Primary/thumbnail flag |
| `CreatedAt` | datetime2 | Default UtcNow | Creation timestamp |

**Constraint:** Only one image per product can have `IsPrimary = true` (unique filtered index).

---

### 4.4 ProductAttributes (EAV Pattern)

**Purpose:** Flexible key-value storage for product specifications. Uses Entity-Attribute-Value pattern for extensibility.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | GUID | PK | Unique identifier |
| `ProductId` | GUID | FK, Required | Link to product |
| `Key` | nvarchar(200) | Required | Attribute name |
| `Group` | nvarchar(100) | Nullable | UI grouping (e.g., "Technical", "Physical") |
| `Unit` | nvarchar(50) | Nullable | Unit of measure |
| `ValueString` | nvarchar(4000) | Nullable | Text value |
| `ValueNumber` | decimal(18,4) | Nullable | Numeric value |
| `ValueBoolean` | bit | Nullable | Boolean value |
| `CreatedAt` | datetime2 | Default UtcNow | Creation timestamp |

**Example Data:**
| Key | Group | ValueString | ValueNumber | ValueBoolean |
|-----|-------|-------------|-------------|--------------|
| screenSize | Display | null | 6.5 | null |
| color | Appearance | Space Gray | null | null |
| isWaterproof | Features | null | null | true |
| weight | Physical | null | 180 | null |

---

### 4.5 ProductCertification (Quality/Organic)

**Purpose:** Store certification details for verified/organic products.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | GUID | PK | Unique identifier |
| `ProductId` | GUID | FK, Unique | One cert per product |
| `CertificationNumber` | nvarchar(100) | Required | Cert ID (e.g., "IN-ORG-123") |
| `CertificationType` | nvarchar(100) | Required | Type (e.g., "USDA Organic") |
| `Origin` | nvarchar(200) | Nullable | Product source location |
| `CertifyingAgency` | nvarchar(200) | Nullable | Issuing authority |
| `IssuedDate` | datetime2 | Nullable | Cert issue date |
| `ExpiryDate` | datetime2 | Nullable | Cert expiry date |
| `IsValid` | bit | Default true | Current validity status |
| `ProductExpirationDate` | datetime2 | Nullable | Product expiry (perishables) |
| `Notes` | nvarchar(1000) | Nullable | Additional notes |

---

### 4.6 ProductMetadata (SEO)

**Purpose:** SEO-friendly slugs and metadata for product pages.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | GUID | PK | Unique identifier |
| `ProductId` | GUID | FK, Unique | One metadata per product |
| `Slug` | nvarchar(200) | Unique, Nullable | URL slug (e.g., "iphone-15-pro-256gb") |
| `SeoMetadataJson` | nvarchar(4000) | Nullable | JSON with title, description, keywords |

**SeoMetadataJson Structure:**
```json
{
  "title": "iPhone 15 Pro - Buy Online",
  "description": "Latest iPhone 15 Pro with A17 chip...",
  "keywords": ["iphone", "apple", "smartphone"],
  "canonicalUrl": "https://example.com/products/iphone-15-pro"
}
```

---

### 4.7 Tags & ProductTags (Discovery)

**Purpose:** Reusable labels for product discovery and filtering.

**Tags Table:**
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | GUID | PK | Unique identifier |
| `Name` | nvarchar(100) | Required | Display name |
| `Slug` | nvarchar(120) | Required, Unique | URL-friendly identifier |

**ProductTags Table (Junction):**
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `ProductId` | GUID | PK, FK | Link to product |
| `TagId` | GUID | PK, FK | Link to tag |

**Example:**
```
Tags: [organic, bestseller, new-arrival, eco-friendly]

Product "Organic Apples" → Tags: [organic, eco-friendly]
Product "iPhone 15" → Tags: [bestseller, new-arrival]
```

---

## 5. Architecture & Layers

### Layer Diagram

```
┌────────────────────────────────────────────────────────────────┐
│                        API LAYER                                │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │              ProductsController.cs                        │  │
│  │  • HTTP endpoints (GET, POST)                            │  │
│  │  • Request validation                                     │  │
│  │  • Response formatting                                    │  │
│  └──────────────────────────────────────────────────────────┘  │
│                              │                                  │
│                              ▼                                  │
├────────────────────────────────────────────────────────────────┤
│                      SERVICE LAYER                              │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │              ProductServiceImpl.cs                        │  │
│  │  • Business logic                                         │  │
│  │  • Validation rules                                       │  │
│  │  • Orchestration                                          │  │
│  └──────────────────────────────────────────────────────────┘  │
│                              │                                  │
│                              ▼                                  │
├────────────────────────────────────────────────────────────────┤
│                     REPOSITORY LAYER                            │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │              ProductRepository.cs                         │  │
│  │  • Data access                                            │  │
│  │  • CRUD operations                                        │  │
│  │  • Stock management                                       │  │
│  └──────────────────────────────────────────────────────────┘  │
│                              │                                  │
│                              ▼                                  │
├────────────────────────────────────────────────────────────────┤
│                       DATA LAYER                                │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │              AppDbContext.cs                              │  │
│  │  • EF Core DbContext                                      │  │
│  │  • Entity configurations                                  │  │
│  │  • Migrations                                             │  │
│  └──────────────────────────────────────────────────────────┘  │
│                              │                                  │
│                              ▼                                  │
│                    ┌──────────────────┐                         │
│                    │   SQL Server     │                         │
│                    │  EP_Local_*Db    │                         │
│                    └──────────────────┘                         │
└────────────────────────────────────────────────────────────────┘
```

### Design Patterns Used

| Pattern | Implementation | Purpose |
|---------|----------------|---------|
| **Repository** | `IProductRepository` / `ProductRepository` | Abstract data access |
| **Service Layer** | `IProductService` / `ProductServiceImpl` | Business logic encapsulation |
| **DTO** | Request/Response classes | API contracts, hide internals |
| **Mapper** | `IProductMapper` / `ProductMapper` | Entity ↔ DTO conversion |
| **EAV** | `ProductAttributes` table | Flexible attribute storage |

---

## 6. API Endpoints

### Base URL: `/api/products`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | List all products |
| GET | `/api/products/{id}` | Get product details |
| POST | `/api/products` | Create new product |
| POST | `/api/products/{id}/reserve` | Reserve stock |
| POST | `/api/products/{id}/release` | Release stock |

### GET /api/products - List Products

**Response (200 OK):**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "iPhone 15 Pro",
    "price": 129900,
    "stock": 50,
    "brand": "Apple",
    "categoryName": "Smartphones",
    "primaryImageUrl": "https://cdn.example.com/iphone15.jpg",
    "isActive": true
  }
]
```

### GET /api/products/{id} - Product Details

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "iPhone 15 Pro",
  "description": "Latest Apple smartphone with A17 chip",
  "price": 129900,
  "stock": 50,
  "brand": "Apple",
  "sku": "APPL-IP15P-256-BLK",
  "unit": "piece",
  "categoryId": "...",
  "categoryName": "Smartphones",
  "isActive": true,
  "createdAt": "2026-01-31T10:00:00Z",
  "images": [
    {
      "url": "https://cdn.example.com/iphone15.jpg",
      "altText": "iPhone 15 Pro front view",
      "isPrimary": true
    }
  ],
  "attributes": [
    { "key": "screenSize", "group": "Display", "valueNumber": 6.1 },
    { "key": "storage", "group": "Memory", "valueString": "256GB" }
  ],
  "tags": ["bestseller", "new-arrival"],
  "certification": {
    "certificationNumber": "CE-12345",
    "certificationType": "CE Mark",
    "isValid": true
  },
  "metadata": {
    "slug": "iphone-15-pro-256gb",
    "seo": {
      "title": "Buy iPhone 15 Pro Online",
      "description": "...",
      "keywords": ["iphone", "apple"]
    }
  }
}
```

### POST /api/products/{id}/reserve - Reserve Stock

**Request:**
```json
{ "quantity": 2 }
```

**Response (200 OK):**
```json
{ "id": "...", "remaining": 48 }
```

**Error (409 Conflict):**
```json
{ "error": "Insufficient stock. Available: 1, Requested: 2" }
```

---

## 7. Business Logic

### Stock Management Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                    STOCK MANAGEMENT FLOW                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  USER ADDS TO CART                                              │
│         │                                                        │
│         ▼                                                        │
│  ┌─────────────────┐                                            │
│  │ Check Stock > 0 │  (Frontend checks before add to cart)     │
│  └────────┬────────┘                                            │
│           │                                                      │
│           ▼                                                      │
│  USER PROCEEDS TO CHECKOUT                                       │
│           │                                                      │
│           ▼                                                      │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │ Order Service calls: POST /api/products/{id}/reserve    │    │
│  │ Body: { "quantity": N }                                  │    │
│  └────────┬────────────────────────────────────────────────┘    │
│           │                                                      │
│           ▼                                                      │
│  ┌─────────────────┐     ┌──────────────────────────────┐       │
│  │ Stock >= N ?    │─NO─►│ Return 409 Conflict          │       │
│  └────────┬────────┘     │ "Insufficient stock"         │       │
│           │YES           └──────────────────────────────┘       │
│           ▼                                                      │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │ Stock = Stock - N                                        │    │
│  │ Save to database                                         │    │
│  │ Return 200 OK { remaining: Stock }                       │    │
│  └────────┬────────────────────────────────────────────────┘    │
│           │                                                      │
│           ▼                                                      │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │               PAYMENT PROCESSING                         │    │
│  └────────┬─────────────────────────┬──────────────────────┘    │
│           │                         │                            │
│      SUCCESS                      FAILURE                        │
│           │                         │                            │
│           ▼                         ▼                            │
│  ┌─────────────────┐     ┌─────────────────────────────────┐    │
│  │ Order Complete  │     │ POST /api/products/{id}/release │    │
│  │ Stock stays     │     │ Body: { "quantity": N }         │    │
│  │ reduced         │     │ Stock = Stock + N               │    │
│  └─────────────────┘     └─────────────────────────────────┘    │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Validation Rules

| Field | Rule | Error |
|-------|------|-------|
| Name | Required, not empty | "Name is required" |
| Price | Must be >= 0 | "Price must be >= 0" |
| Stock | Must be >= 0 | "Stock must be >= 0" |
| Reserve Quantity | Must be > 0 | "Quantity must be > 0" |
| Reserve Quantity | Must be <= Stock | "Insufficient stock" |

---

## 8. Configuration

### Connection Strings

**Local Development** (`appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EP_Local_ProductDb;Integrated Security=true;TrustServerCertificate=True;"
  }
}
```

**Staging** (`appsettings.Staging.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB-Staging;Database=EP_Staging_ProductDb;Integrated Security=true;TrustServerCertificate=True;"
  }
}
```

### Running the Service

```powershell
# Local Development (EP_Local_ProductDb)
cd services/product-service/src/ProductService.API
dotnet run

# Staging (EP_Staging_ProductDb)
dotnet run --environment Staging
```

---

## 9. Code Structure

```
product-service/
├── src/
│   ├── ProductService.API/                    # API Layer
│   │   ├── Controllers/
│   │   │   ├── ProductsController.cs          # Main API endpoints
│   │   │   └── HealthController.cs            # Health check
│   │   ├── Program.cs                         # Entry point
│   │   ├── Startup.cs                         # DI configuration
│   │   ├── appsettings.json                   # Base config
│   │   ├── appsettings.Development.json       # Local DB config
│   │   └── appsettings.Staging.json           # Staging DB config
│   │
│   ├── ProductService.Core/                   # Business Layer
│   │   ├── Business/
│   │   │   ├── IProductService.cs             # Service interface
│   │   │   └── ProductServiceImpl.cs          # Service implementation
│   │   ├── Repository/
│   │   │   ├── IProductRepository.cs          # Repository interface
│   │   │   └── ProductRepository.cs           # Data access
│   │   ├── Mappers/
│   │   │   ├── IProductMapper.cs              # Mapper interface
│   │   │   └── ProductMapper.cs               # Entity ↔ DTO mapping
│   │   └── Data/
│   │       ├── AppDbContext.cs                # EF Core context
│   │       └── Migrations/                    # DB migrations
│   │
│   └── ProductService.Abstraction/            # Contracts Layer
│       ├── Models/                            # Entity classes
│       │   ├── Product.cs
│       │   ├── Category.cs
│       │   ├── ProductImage.cs
│       │   ├── ProductAttribute.cs
│       │   ├── ProductCertification.cs
│       │   ├── ProductMetadata.cs
│       │   ├── SeoMetadata.cs
│       │   ├── Tag.cs
│       │   └── ProductTag.cs
│       └── DTOs/                              # Data Transfer Objects
│           ├── Requests/
│           │   ├── CreateProductRequest.cs
│           │   ├── UpdateProductRequest.cs
│           │   └── ReserveStockRequest.cs
│           └── Responses/
│               ├── ProductResponse.cs
│               └── ProductDetailResponse.cs
│
└── test/                                      # Tests
    ├── unit-test/
    │   ├── ProductService.API.Test/
    │   └── ProductService.Core.Test/
    └── integration-test/
        └── ProductService.Integration.Test/
```

---

## Summary

The Product Service is a comprehensive catalog management system with:

- **8 database tables** working together
- **Normalized schema** following best practices
- **Flexible extensibility** via EAV attributes and JSON metadata
- **Stock management** with reserve/release operations
- **Hierarchical categories** for product taxonomy
- **Many-to-many tags** for product discovery
- **Clean architecture** with separation of concerns

**Database:** `EP_Local_ProductDb` (Local) | `EP_Staging_ProductDb` (Staging)  
**Port:** 5002  
**Swagger:** http://localhost:5002/swagger

---

*Document Version: 2.0*  
*Last Updated: January 31, 2026*
