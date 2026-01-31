using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;
using Xunit;

namespace ProductService.Integration.Test.Controllers;

[Collection("ProductServiceIntegration")]
public class ProductsControllerIntegrationTests
{
    private readonly ProductServiceFixture _fixture;

    public ProductsControllerIntegrationTests(ProductServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Create_ValidData_ReturnsCreatedProduct()
    {
        // Arrange
        var createDto = new CreateProductRequest
        {
            Name = "Integration Test Product",
            Description = "A product created during integration testing",
            Price = 99,
            Stock = 10,
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/products", createDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var product = await response.Content.ReadFromJsonAsync<ProductDetailResponse>();
        product.Should().NotBeNull();
        product!.Name.Should().Be(createDto.Name);
        product.Price.Should().Be(createDto.Price);
    }

    [Fact]
    public async Task GetById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var createDto = new CreateProductRequest
        {
            Name = "GetById Test Product",
            Price = 50,
            Stock = 5,
        };
        var createResponse = await _fixture.Client.PostAsJsonAsync("/api/products", createDto);
        createResponse.EnsureSuccessStatusCode();
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductDetailResponse>();

        // Act
        var response = await _fixture.Client.GetAsync($"/api/products/{createdProduct!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var product = await response.Content.ReadFromJsonAsync<ProductDetailResponse>();
        product.Should().NotBeNull();
        product!.Id.Should().Be(createdProduct.Id);
    }

    [Fact]
    public async Task ReserveAndRelease_ValidQuantities_UpdatesStock()
    {
        // Arrange
        var createDto = new CreateProductRequest
        {
            Name = "Stock Operation Product",
            Price = 10,
            Stock = 100,
        };
        var createResponse = await _fixture.Client.PostAsJsonAsync("/api/products", createDto);
        createResponse.EnsureSuccessStatusCode();
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductDetailResponse>();

        // Sanity check: stock should be persisted as created
        var beforeReserveResponse = await _fixture.Client.GetAsync($"/api/products/{createdProduct!.Id}");
        beforeReserveResponse.EnsureSuccessStatusCode();
        var beforeReserveProduct = await beforeReserveResponse.Content.ReadFromJsonAsync<ProductDetailResponse>();
        beforeReserveProduct!.Stock.Should().Be(100);

        // Act - Reserve 20
        var reserveDto = new ReserveStockRequest { Quantity = 20 };
        var reserveResponse = await _fixture.Client.PostAsJsonAsync($"/api/products/{createdProduct!.Id}/reserve", reserveDto);
        reserveResponse.EnsureSuccessStatusCode();
        var reserveResult = await reserveResponse.Content.ReadFromJsonAsync<JsonElement>();
        reserveResult.GetProperty("remaining").GetInt32().Should().Be(80);

        // Act - Release 10
        var releaseDto = new ReserveStockRequest { Quantity = 10 };
        var releaseResponse = await _fixture.Client.PostAsJsonAsync($"/api/products/{createdProduct.Id}/release", releaseDto);
        releaseResponse.EnsureSuccessStatusCode();
        var releaseResult = await releaseResponse.Content.ReadFromJsonAsync<JsonElement>();
        releaseResult.GetProperty("remaining").GetInt32().Should().Be(90);
    }
}



