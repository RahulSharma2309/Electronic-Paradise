# 🧪 Phase 5: Update Tests - Implementation Guide

## 🎯 **Goal**

Update all existing tests to use the new Request/Response DTOs and verify the refactored architecture works correctly.

---

## 📋 **What Needs to Be Done**

### **1. Update Existing Tests**
- Controller tests
- Service tests
- Integration tests

### **2. Add New Tests**
- DTO validation tests
- Mapper tests
- Fluent API configuration tests

---

## 🔧 **Test Updates by Service**

### **1. ProductService Tests**

**Location:** `services/product-service/tests/ProductService.Tests/`

#### **Controller Tests - Before/After**

**BEFORE (Old):**
```csharp
[Fact]
public async Task GetAll_ReturnsOkWithProducts()
{
    // Arrange
    var products = new List<Product>
    {
        new Product { Id = Guid.NewGuid(), Name = "Test Product", Price = 100 }
    };
    _mockService.Setup(s => s.ListAsync()).ReturnsAsync(products);
    
    // Act
    var result = await _controller.GetAll();
    
    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    // ... assertions on anonymous object (hard to test)
}
```

**AFTER (Correct):**
```csharp
[Fact]
public async Task GetAll_ReturnsOkWithProductResponses()
{
    // Arrange
    var products = new List<ProductResponse>
    {
        new ProductResponse 
        { 
            Id = Guid.NewGuid(), 
            Name = "Test Product", 
            Price = 100,
            Stock = 10 
        }
    };
    _mockService.Setup(s => s.ListAsync()).ReturnsAsync(products);
    
    // Act
    var result = await _controller.GetAll();
    
    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnedProducts = Assert.IsAssignableFrom<List<ProductResponse>>(okResult.Value);
    Assert.Single(returnedProducts);
    Assert.Equal("Test Product", returnedProducts[0].Name);
}
```

#### **Service Tests - Before/After**

**BEFORE (Old):**
```csharp
[Fact]
public async Task CreateAsync_AddsProductToDatabase()
{
    // Arrange
    var product = new Product { Name = "Test", Price = 100 };
    
    // Act
    await _service.CreateAsync(product);
    
    // Assert
    _mockDb.Verify(db => db.Products.Add(It.IsAny<Product>()), Times.Once);
}
```

**AFTER (Correct):**
```csharp
[Fact]
public async Task CreateAsync_AddsProductToDatabase_ReturnsProductDetailResponse()
{
    // Arrange
    var request = new CreateProductRequest 
    { 
        Name = "Test", 
        Price = 100,
        Stock = 10
    };
    
    // Act
    var result = await _service.CreateAsync(request);
    
    // Assert
    Assert.NotNull(result);
    Assert.IsType<ProductDetailResponse>(result);
    Assert.Equal("Test", result.Name);
    Assert.Equal(100, result.Price);
    _mockDb.Verify(db => db.Products.Add(It.IsAny<Product>()), Times.Once);
}
```

---

### **2. New Tests to Add**

#### **DTO Validation Tests**

**File:** `tests/ProductService.Tests/DTOs/RequestValidationTests.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using ProductService.Abstraction.DTOs.Requests;
using Xunit;

namespace ProductService.Tests.DTOs;

public class RequestValidationTests
{
    [Fact]
    public void CreateProductRequest_WithInvalidName_FailsValidation()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "", // Invalid: required
            Price = 100,
            Stock = 10
        };
        
        // Act
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(request);
        var isValid = Validator.TryValidateObject(request, context, validationResults, true);
        
        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Name"));
    }
    
    [Fact]
    public void CreateProductRequest_WithNegativePrice_FailsValidation()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            Price = -100, // Invalid: must be >= 0
            Stock = 10
        };
        
        // Act
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(request);
        var isValid = Validator.TryValidateObject(request, context, validationResults, true);
        
        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Price"));
    }
    
    [Fact]
    public void CreateProductRequest_WithValidData_PassesValidation()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            Price = 100,
            Stock = 10
        };
        
        // Act
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(request);
        var isValid = Validator.TryValidateObject(request, context, validationResults, true);
        
        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }
}
```

---

#### **Mapper Tests**

**File:** `tests/ProductService.Tests/Mappers/ProductMapperTests.cs`

```csharp
using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;
using ProductService.Abstraction.Models;
using ProductService.Core.Mappers;
using Xunit;

namespace ProductService.Tests.Mappers;

public class ProductMapperTests
{
    private readonly ProductMapper _mapper;
    
    public ProductMapperTests()
    {
        _mapper = new ProductMapper();
    }
    
    [Fact]
    public void ToResponse_MapsProductToProductResponse()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Price = 100,
            Stock = 10,
            Category = "Test Category"
        };
        
        // Act
        var response = _mapper.ToResponse(product);
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(product.Id, response.Id);
        Assert.Equal(product.Name, response.Name);
        Assert.Equal(product.Price, response.Price);
        Assert.Equal(product.Stock, response.Stock);
    }
    
    [Fact]
    public void ToEntity_MapsCreateRequestToProduct()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            Price = 100,
            Stock = 10
        };
        
        // Act
        var product = _mapper.ToEntity(request);
        
        // Assert
        Assert.NotNull(product);
        Assert.Equal(request.Name, product.Name);
        Assert.Equal(request.Price, product.Price);
        Assert.Equal(request.Stock, product.Stock);
        Assert.NotEqual(Guid.Empty, product.Id);
    }
    
    [Fact]
    public void UpdateEntity_UpdatesOnlyProvidedFields()
    {
        // Arrange
        var product = new Product
        {
            Name = "Original Name",
            Price = 100,
            Stock = 10
        };
        
        var request = new UpdateProductRequest
        {
            Name = "Updated Name",
            Price = null, // Don't update price
            Stock = 20
        };
        
        // Act
        _mapper.UpdateEntity(product, request);
        
        // Assert
        Assert.Equal("Updated Name", product.Name);
        Assert.Equal(100, product.Price); // Unchanged
        Assert.Equal(20, product.Stock);
    }
}
```

---

### **3. Integration Tests**

**File:** `tests/ProductService.IntegrationTests/ProductsControllerIntegrationTests.cs`

```csharp
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;
using Xunit;

namespace ProductService.IntegrationTests;

public class ProductsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public ProductsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAll_ReturnsProductResponses()
    {
        // Act
        var response = await _client.GetAsync("/api/products");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
        Assert.NotNull(products);
    }
    
    [Fact]
    public async Task Create_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Integration Test Product",
            Price = 100,
            Stock = 10
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/products", request);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var product = await response.Content.ReadFromJsonAsync<ProductDetailResponse>();
        Assert.NotNull(product);
        Assert.Equal(request.Name, product.Name);
    }
    
    [Fact]
    public async Task Create_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "", // Invalid
            Price = -100, // Invalid
            Stock = 10
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/products", request);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
```

---

## 📊 **Test Files to Update**

### **For Each Service:**

1. **Controller Tests**
   - Update all test methods to use new DTOs
   - Update assertions to check DTO types

2. **Service Tests**
   - Update method signatures
   - Update mocks to return DTOs
   - Update assertions

3. **Integration Tests**
   - Update request/response types
   - Verify end-to-end flow

### **New Tests to Add:**

1. **DTO Validation Tests**
   - Test `[Required]` attributes
   - Test `[Range]` attributes
   - Test `[StringLength]` attributes

2. **Mapper Tests**
   - Test all mapping methods
   - Test edge cases (null values, etc.)

3. **Fluent API Tests** (Optional)
   - Verify database schema matches configuration

---

## ✅ **Testing Checklist**

- [ ] 1. Update ProductService tests
- [ ] 2. Update UserService tests
- [ ] 3. Update OrderService tests
- [ ] 4. Update PaymentService tests
- [ ] 5. Update AuthService tests
- [ ] 6. Add DTO validation tests for all services
- [ ] 7. Add mapper tests for all services
- [ ] 8. Add/update integration tests
- [ ] 9. Run all tests and verify they pass
- [ ] 10. Check code coverage

---

## 🎯 **Running Tests**

```bash
# Run all tests
dotnet test

# Run specific service tests
dotnet test services/product-service/tests/ProductService.Tests

# Run with coverage
dotnet test /p:CollectCoverage=true
```

---

**Phase 5: ⏳ Test patterns provided | Implementation needed**
