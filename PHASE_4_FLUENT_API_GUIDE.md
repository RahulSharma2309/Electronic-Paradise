# 🏗️ Phase 4: Fluent API Configuration - Implementation Guide

## 🎯 **Goal**

Move ALL EF Core configuration from domain models (attributes) to `AppDbContext.OnModelCreating` using Fluent API.

**Why?**
- ✅ Clean domain models (no infrastructure pollution)
- ✅ Centralized configuration
- ✅ Better control over database schema
- ✅ Separation of concerns (DDD principle)

---

## 📋 **What Needs to Be Done**

For each service, update the `AppDbContext.cs` file to configure entities using Fluent API.

---

## 🔧 **Implementation Examples**

### **1. ProductService - AppDbContext.cs**

**Location:** `services/product-service/src/ProductService.Core/Data/AppDbContext.cs`

**Add this configuration:**

```csharp
using Microsoft.EntityFrameworkCore;
using ProductService.Abstraction.Models;

namespace ProductService.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductCertification> ProductCertifications { get; set; } = null!;
    public DbSet<ProductMetadata> ProductMetadatas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product entity configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
            
            entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.Stock)
                .IsRequired();
            
            entity.Property(e => e.Category)
                .HasMaxLength(100);
            
            entity.Property(e => e.Brand)
                .HasMaxLength(100);
            
            entity.Property(e => e.Sku)
                .HasMaxLength(50);
            
            entity.Property(e => e.Unit)
                .HasMaxLength(20);
            
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500);
            
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();
            
            entity.Property(e => e.UpdatedAt);
            
            // Relationships
            entity.HasOne(e => e.Certification)
                .WithOne(c => c.Product)
                .HasForeignKey<ProductCertification>(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Metadata)
                .WithOne(m => m.Product)
                .HasForeignKey<ProductMetadata>(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Indexes
            entity.HasIndex(e => e.Sku).IsUnique();
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Brand);
        });

        // ProductCertification entity configuration
        modelBuilder.Entity<ProductCertification>(entity =>
        {
            entity.ToTable("ProductCertifications");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.CertificationNumber)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.CertificationType)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Origin)
                .HasMaxLength(100);
            
            entity.Property(e => e.CertifyingAgency)
                .HasMaxLength(200);
            
            entity.Property(e => e.IsValid)
                .IsRequired()
                .HasDefaultValue(true);
            
            entity.Property(e => e.Notes)
                .HasMaxLength(500);
            
            // Indexes
            entity.HasIndex(e => e.CertificationNumber).IsUnique();
            entity.HasIndex(e => e.ProductId);
        });

        // ProductMetadata entity configuration
        modelBuilder.Entity<ProductMetadata>(entity =>
        {
            entity.ToTable("ProductMetadatas");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.AttributesJson)
                .HasColumnType("nvarchar(max)");
            
            entity.Property(e => e.Tags)
                .HasMaxLength(500);
            
            entity.Property(e => e.Slug)
                .HasMaxLength(200);
            
            entity.Property(e => e.SeoMetadataJson)
                .HasColumnType("nvarchar(max)");
            
            // Indexes
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.HasIndex(e => e.ProductId);
        });
    }
}
```

---

### **2. UserService - AppDbContext.cs**

**Location:** `services/user-service/src/UserService.Core/Data/AppDbContext.cs`

**Add this configuration:**

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // UserProfile entity configuration
    modelBuilder.Entity<UserProfile>(entity =>
    {
        entity.ToTable("UserProfiles");
        
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.UserId)
            .IsRequired();
        
        entity.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        entity.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        entity.Property(e => e.Address)
            .HasMaxLength(500);
        
        entity.Property(e => e.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);
        
        entity.Property(e => e.WalletBalance)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);
        
        entity.Property(e => e.CreatedAt)
            .IsRequired();
        
        entity.Property(e => e.UpdatedAt);
        
        // Indexes
        entity.HasIndex(e => e.UserId).IsUnique();
        entity.HasIndex(e => e.PhoneNumber).IsUnique();
    });
}
```

---

### **3. OrderService - AppDbContext.cs**

**Location:** `services/order-service/src/OrderService.Core/Data/AppDbContext.cs`

**Add this configuration:**

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Order entity configuration
    modelBuilder.Entity<Order>(entity =>
    {
        entity.ToTable("Orders");
        
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.UserId)
            .IsRequired();
        
        entity.Property(e => e.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        entity.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Pending");
        
        entity.Property(e => e.CreatedAt)
            .IsRequired();
        
        // Relationships
        entity.HasMany(e => e.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.Status);
        entity.HasIndex(e => e.CreatedAt);
    });

    // OrderItem entity configuration
    modelBuilder.Entity<OrderItem>(entity =>
    {
        entity.ToTable("OrderItems");
        
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.ProductId)
            .IsRequired();
        
        entity.Property(e => e.Quantity)
            .IsRequired();
        
        entity.Property(e => e.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        // Indexes
        entity.HasIndex(e => e.OrderId);
        entity.HasIndex(e => e.ProductId);
    });
}
```

---

### **4. PaymentService - AppDbContext.cs**

**Location:** `services/payment-service/src/PaymentService.Core/Data/AppDbContext.cs`

**Add this configuration:**

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // PaymentRecord entity configuration
    modelBuilder.Entity<PaymentRecord>(entity =>
    {
        entity.ToTable("PaymentRecords");
        
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.OrderId)
            .IsRequired();
        
        entity.Property(e => e.UserId)
            .IsRequired();
        
        entity.Property(e => e.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        entity.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Pending");
        
        entity.Property(e => e.Timestamp)
            .IsRequired();
        
        // Indexes
        entity.HasIndex(e => e.OrderId);
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.Status);
        entity.HasIndex(e => e.Timestamp);
    });
}
```

---

### **5. AuthService - AppDbContext.cs**

**Location:** `services/auth-service/src/AuthService.Core/Data/AppDbContext.cs`

**Add this configuration:**

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // User entity configuration
    modelBuilder.Entity<User>(entity =>
    {
        entity.ToTable("Users");
        
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(256);
        
        entity.Property(e => e.Password)
            .IsRequired()
            .HasMaxLength(500);
        
        entity.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(200);
        
        entity.Property(e => e.CreatedAt)
            .IsRequired();
        
        // Indexes
        entity.HasIndex(e => e.Email).IsUnique();
    });
}
```

---

## 🔄 **After Configuration: Create Migrations**

After adding Fluent API configuration, you MUST create new migrations:

### **For Each Service:**

```bash
# Navigate to service directory
cd services/product-service/src/ProductService.API

# Create migration
dotnet ef migrations add FluentApiConfiguration --project ../ProductService.Core

# Apply migration
dotnet ef database update
```

**Repeat for:**
- `services/user-service/src/UserService.API`
- `services/order-service/src/OrderService.API`
- `services/payment-service/src/PaymentService.API`
- `services/auth-service/src/AuthService.API`

---

## ✅ **Benefits of Fluent API**

### **Before (Attributes):**
```csharp
public class Product
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}
```

**Problems:**
- ❌ Domain model polluted with infrastructure
- ❌ Hard to maintain complex configurations
- ❌ Can't configure relationships easily

### **After (Fluent API):**
```csharp
// Clean domain model
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// Configuration in AppDbContext
modelBuilder.Entity<Product>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
    entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
});
```

**Benefits:**
- ✅ Clean domain models
- ✅ Centralized configuration
- ✅ Easy to manage relationships
- ✅ Better for complex scenarios

---

## 📊 **Checklist**

- [ ] 1. Update ProductService AppDbContext
- [ ] 2. Update UserService AppDbContext
- [ ] 3. Update OrderService AppDbContext
- [ ] 4. Update PaymentService AppDbContext
- [ ] 5. Update AuthService AppDbContext
- [ ] 6. Create migrations for each service
- [ ] 7. Apply migrations
- [ ] 8. Test database schema

---

**Phase 4: ⏳ Configuration examples provided | Implementation needed**
