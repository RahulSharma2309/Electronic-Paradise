using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Abstraction.DTOs;
using ProductService.Abstraction.Models;
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
        var createDto = new CreateProductDto
        {
            Name = "Integration Test Product",
            Description = "A product created during integration testing",
            Price = 99,
            Stock = 10
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/products", createDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var product = await response.Content.ReadFromJsonAsync<Product>();
        product.Should().NotBeNull();
        product!.Name.Should().Be(createDto.Name);
        product.Price.Should().Be(createDto.Price);
    }

    [Fact]
    public async Task GetById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            Name = "GetById Test Product",
            Price = 50,
            Stock = 5
        };
        var createResponse = await _fixture.Client.PostAsJsonAsync("/api/products", createDto);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();

        // Act
        var response = await _fixture.Client.GetAsync($"/api/products/{createdProduct!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var product = await response.Content.ReadFromJsonAsync<Product>();
        product.Should().NotBeNull();
        product!.Id.Should().Be(createdProduct.Id);
    }

    [Fact]
    public async Task ReserveAndRelease_ValidQuantities_UpdatesStock()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            Name = "Stock Operation Product",
            Price = 10,
            Stock = 100
        };
        var createResponse = await _fixture.Client.PostAsJsonAsync("/api/products", createDto);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();

        // Act - Reserve 20
        var reserveDto = new ReleaseDto { Quantity = 20 };
        var reserveResponse = await _fixture.Client.PostAsJsonAsync($"/api/products/{createdProduct!.Id}/reserve", reserveDto);
        reserveResponse.EnsureSuccessStatusCode();
        var reserveResult = await reserveResponse.Content.ReadFromJsonAsync<dynamic>();
        ((int)reserveResult!.remaining).Should().Be(80);

        // Act - Release 10
        var releaseDto = new ReleaseDto { Quantity = 10 };
        var releaseResponse = await _fixture.Client.PostAsJsonAsync($"/api/products/{createdProduct.Id}/release", releaseDto);
        releaseResponse.EnsureSuccessStatusCode();
        var releaseResult = await releaseResponse.Content.ReadFromJsonAsync<dynamic>();
        ((int)releaseResult!.remaining).Should().Be(90);
    }
}



