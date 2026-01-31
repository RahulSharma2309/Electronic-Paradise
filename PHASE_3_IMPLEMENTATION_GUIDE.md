# 🔄 Phase 3: Mapper Services - Implementation Guide

## ✅ **What's Complete**

All mapper interfaces and implementations have been created:

### **Mappers Created (10 Files):**
1. ✅ **ProductService**
   - `ProductService.Core/Mappers/IProductMapper.cs`
   - `ProductService.Core/Mappers/ProductMapper.cs`

2. ✅ **UserService**
   - `UserService.Core/Mappers/IUserProfileMapper.cs`
   - `UserService.Core/Mappers/UserProfileMapper.cs`

3. ✅ **OrderService**
   - `OrderService.Core/Mappers/IOrderMapper.cs`
   - `OrderService.Core/Mappers/OrderMapper.cs`

4. ✅ **PaymentService**
   - `PaymentService.Core/Mappers/IPaymentMapper.cs`
   - `PaymentService.Core/Mappers/PaymentMapper.cs`

5. ✅ **AuthService**
   - `AuthService.Core/Mappers/IAuthMapper.cs`
   - `AuthService.Core/Mappers/AuthMapper.cs`

---

## 📋 **What Needs to Be Done**

### **Step 1: Register Mappers in DI Container**

For each service, add mapper registration in `Program.cs`:

#### **ProductService - Program.cs**
```csharp
using ProductService.Core.Mappers;

// Add before builder.Build()
builder.Services.AddScoped<IProductMapper, ProductMapper>();
```

#### **UserService - Program.cs**
```csharp
using UserService.Core.Mappers;

builder.Services.AddScoped<IUserProfileMapper, UserProfileMapper>();
```

#### **OrderService - Program.cs**
```csharp
using OrderService.Core.Mappers;

builder.Services.AddScoped<IOrderMapper, OrderMapper>();
```

#### **PaymentService - Program.cs**
```csharp
using PaymentService.Core.Mappers;

builder.Services.AddScoped<IPaymentMapper, PaymentMapper>();
```

#### **AuthService - Program.cs**
```csharp
using AuthService.Core.Mappers;

builder.Services.AddScoped<IAuthMapper, AuthMapper>();
```

---

### **Step 2: Update Service Implementations**

Each service implementation needs to:
1. Inject the mapper in constructor
2. Use mapper methods to convert between domain models and DTOs

#### **Example: ProductService Implementation**

**BEFORE (Old):**
```csharp
public class ProductBusinessService : IProductService
{
    private readonly AppDbContext _db;
    
    public ProductBusinessService(AppDbContext db)
    {
        _db = db;
    }
    
    // ❌ Returns domain model
    public async Task<List<Product>> ListAsync()
    {
        return await _db.Products.ToListAsync();
    }
    
    // ❌ Returns domain model
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _db.Products.FindAsync(id);
    }
    
    // ❌ Takes domain model, returns void
    public async Task CreateAsync(Product p)
    {
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
    }
}
```

**AFTER (Correct):**
```csharp
using ProductService.Core.Mappers;

public class ProductBusinessService : IProductService
{
    private readonly AppDbContext _db;
    private readonly IProductMapper _mapper;
    
    // ✅ Inject mapper
    public ProductBusinessService(AppDbContext db, IProductMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    // ✅ Returns ProductResponse DTO
    public async Task<List<ProductResponse>> ListAsync()
    {
        var products = await _db.Products.ToListAsync();
        return products.Select(_mapper.ToResponse).ToList();
    }
    
    // ✅ Returns ProductDetailResponse DTO
    public async Task<ProductDetailResponse?> GetByIdAsync(Guid id)
    {
        var product = await _db.Products
            .Include(p => p.Certification)
            .Include(p => p.Metadata)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        return product != null ? _mapper.ToDetailResponse(product) : null;
    }
    
    // ✅ Takes CreateProductRequest, returns ProductDetailResponse
    public async Task<ProductDetailResponse> CreateAsync(CreateProductRequest request)
    {
        var product = _mapper.ToEntity(request);
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return _mapper.ToDetailResponse(product);
    }
    
    // ✅ New Update method
    public async Task<ProductDetailResponse?> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return null;
        
        _mapper.UpdateEntity(product, request);
        await _db.SaveChangesAsync();
        return _mapper.ToDetailResponse(product);
    }
    
    // Keep stock operations as-is (return int)
    public async Task<int> ReserveAsync(Guid id, int quantity) { /* ... */ }
    public async Task<int> ReleaseAsync(Guid id, int quantity) { /* ... */ }
}
```

---

#### **Example: UserService Implementation**

**Key Changes:**
```csharp
using UserService.Core.Mappers;

public class UserBusinessService : IUserService
{
    private readonly AppDbContext _db;
    private readonly IUserProfileMapper _mapper;
    
    public UserBusinessService(AppDbContext db, IUserProfileMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    public async Task<UserProfileDetailResponse?> GetByIdAsync(Guid id)
    {
        var profile = await _db.UserProfiles.FindAsync(id);
        return profile != null ? _mapper.ToDetailResponse(profile) : null;
    }
    
    public async Task<UserProfileResponse?> GetByUserIdAsync(Guid userId)
    {
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        return profile != null ? _mapper.ToResponse(profile) : null;
    }
    
    public async Task<UserProfileDetailResponse> CreateAsync(CreateUserProfileRequest request)
    {
        var profile = _mapper.ToEntity(request);
        _db.UserProfiles.Add(profile);
        await _db.SaveChangesAsync();
        return _mapper.ToDetailResponse(profile);
    }
    
    public async Task<WalletBalanceResponse> AddBalanceAsync(Guid userId, AddBalanceRequest request)
    {
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) throw new KeyNotFoundException("User profile not found");
        
        profile.WalletBalance += request.Amount;
        await _db.SaveChangesAsync();
        
        return _mapper.ToWalletBalanceResponse(
            userId, 
            profile.WalletBalance, 
            $"Successfully added ${request.Amount:F2}. New balance: ${profile.WalletBalance:F2}"
        );
    }
}
```

---

#### **Example: OrderService Implementation**

**Key Changes:**
```csharp
using OrderService.Core.Mappers;

public class OrderBusinessService : IOrderService
{
    private readonly AppDbContext _db;
    private readonly IOrderMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public OrderBusinessService(
        AppDbContext db, 
        IOrderMapper mapper,
        IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _mapper = mapper;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<OrderDetailResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        var order = _mapper.ToEntity(request);
        
        // ... validate products, process payment, reserve stock ...
        
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return _mapper.ToDetailResponse(order);
    }
    
    public async Task<OrderDetailResponse?> GetOrderAsync(Guid id)
    {
        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
            
        return order != null ? _mapper.ToDetailResponse(order) : null;
    }
    
    public async Task<List<OrderResponse>> GetUserOrdersAsync(Guid userId)
    {
        var orders = await _db.Orders
            .Where(o => o.UserId == userId)
            .ToListAsync();
            
        return orders.Select(_mapper.ToResponse).ToList();
    }
}
```

---

#### **Example: PaymentService Implementation**

**Key Changes:**
```csharp
using PaymentService.Core.Mappers;

public class PaymentBusinessService : IPaymentService
{
    private readonly AppDbContext _db;
    private readonly IPaymentMapper _mapper;
    
    public PaymentBusinessService(AppDbContext db, IPaymentMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    public async Task<PaymentDetailResponse> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        // Debit user wallet via HTTP client...
        
        var payment = _mapper.ToEntity(request, "Success");
        _db.PaymentRecords.Add(payment);
        await _db.SaveChangesAsync();
        return _mapper.ToDetailResponse(payment);
    }
    
    public async Task<PaymentStatusResponse?> GetPaymentStatusAsync(Guid orderId)
    {
        var payment = await _db.PaymentRecords
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
            
        return payment != null ? _mapper.ToStatusResponse(payment) : null;
    }
}
```

---

#### **Example: AuthService Implementation**

**Key Changes:**
```csharp
using AuthService.Core.Mappers;
using Ep.Platform.Security;

public class AuthBusinessService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IAuthMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    
    public AuthBusinessService(
        AppDbContext db, 
        IAuthMapper mapper,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _db = db;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var hashedPassword = _passwordHasher.Hash(request.Password);
        var user = _mapper.ToEntity(request, hashedPassword);
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
        // Generate JWT token
        var claims = new Dictionary<string, string>
        {
            [System.Security.Claims.ClaimTypes.NameIdentifier] = user.Id.ToString(),
            [System.Security.Claims.ClaimTypes.Email] = user.Email,
            ["fullName"] = user.FullName
        };
        var token = _jwtTokenGenerator.GenerateToken(claims);
        var expiresAt = DateTime.UtcNow.AddHours(6);
        
        return _mapper.ToAuthResponse(user, token, expiresAt);
    }
    
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !_passwordHasher.Verify(request.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        
        // Generate JWT token
        var claims = new Dictionary<string, string>
        {
            [System.Security.Claims.ClaimTypes.NameIdentifier] = user.Id.ToString(),
            [System.Security.Claims.ClaimTypes.Email] = user.Email,
            ["fullName"] = user.FullName
        };
        var token = _jwtTokenGenerator.GenerateToken(claims);
        var expiresAt = DateTime.UtcNow.AddHours(6);
        
        return _mapper.ToAuthResponse(user, token, expiresAt);
    }
    
    public async Task<UserDetailResponse?> GetUserByIdAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        return user != null ? _mapper.ToDetailResponse(user) : null;
    }
}
```

---

## 📊 **Service Implementation Files to Update**

For each service, find and update these files:

1. **ProductService**
   - `services/product-service/src/ProductService.Core/Business/ProductBusinessService.cs`

2. **UserService**
   - `services/user-service/src/UserService.Core/Business/UserBusinessService.cs`

3. **OrderService**
   - `services/order-service/src/OrderService.Core/Business/OrderBusinessService.cs`

4. **PaymentService**
   - `services/payment-service/src/PaymentService.Core/Business/PaymentBusinessService.cs`

5. **AuthService**
   - `services/auth-service/src/AuthService.Core/Business/AuthBusinessService.cs`

---

## ✅ **Checklist for Each Service**

- [ ] 1. Register mapper in `Program.cs`
- [ ] 2. Find service implementation class
- [ ] 3. Inject mapper in constructor
- [ ] 4. Update each method to:
  - [ ] Use mapper to convert Request → Entity
  - [ ] Use mapper to convert Entity → Response
  - [ ] Return correct DTO type
- [ ] 5. Add `Include()` calls for related entities (EF Core)
- [ ] 6. Compile and fix any errors

---

## 🎯 **Next Steps**

After updating all service implementations:
1. **Compile** each service to check for errors
2. **Test** basic endpoints to verify mapping works
3. Move to **Phase 4: Fluent API Configuration**
4. Then **Phase 5: Update Tests**

---

**Phase 3 Mappers: ✅ Created | ⏳ Implementations need updating**
