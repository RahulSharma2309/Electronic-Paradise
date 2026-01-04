using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using OrderService.Abstraction.DTOs;
using Xunit;

namespace OrderService.Integration.Test.Controllers;

[Collection("OrderServiceIntegration")]
public class OrdersControllerIntegrationTests
{
    private readonly OrderServiceFixture _fixture;

    public OrdersControllerIntegrationTests(OrderServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateOrder_ValidData_ReturnsCreated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var dto = new CreateOrderDto
        {
            UserId = userId,
            Items = new List<OrderItemDto>
            {
                new OrderItemDto { ProductId = productId, Quantity = 2 }
            }
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/orders/create", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        ((Guid)result!.userId).Should().Be(userId);
    }

    [Fact]
    public async Task GetOrder_ExistingOrder_ReturnsOrder()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createDto = new CreateOrderDto
        {
            UserId = userId,
            Items = new List<OrderItemDto> { new OrderItemDto { ProductId = Guid.NewGuid(), Quantity = 1 } }
        };
        var createResponse = await _fixture.Client.PostAsJsonAsync("/api/orders/create", createDto);
        var createdResult = await createResponse.Content.ReadFromJsonAsync<dynamic>();
        Guid orderId = createdResult!.id;

        // Act
        var response = await _fixture.Client.GetAsync($"/api/orders/{orderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        ((Guid)result!.id).Should().Be(orderId);
    }

    [Fact]
    public async Task GetUserOrders_ExistingUser_ReturnsOrders()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createDto = new CreateOrderDto
        {
            UserId = userId,
            Items = new List<OrderItemDto> { new OrderItemDto { ProductId = Guid.NewGuid(), Quantity = 1 } }
        };
        await _fixture.Client.PostAsJsonAsync("/api/orders/create", createDto);

        // Act
        var response = await _fixture.Client.GetAsync($"/api/orders/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var results = await response.Content.ReadFromJsonAsync<List<dynamic>>();
        results.Should().NotBeNull();
        results!.Count.Should().BeGreaterThan(0);
    }
}



