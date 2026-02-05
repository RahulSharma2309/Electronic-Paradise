using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OrderService.Abstraction.DTOs.Requests;
using OrderService.Abstraction.DTOs.Responses;
using OrderService.API.Controllers;
using OrderService.Core.Business;
using Xunit;

namespace OrderService.API.Test.Controllers;

public class OrdersControllerTests
{
    [Fact]
    public async Task CreateOrder_WhenServiceThrowsArgumentException_ShouldReturnBadRequest()
    {
        // Arrange
        var service = new Mock<IOrderService>(MockBehavior.Strict);
        service.Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderRequest>()))
            .ThrowsAsync(new ArgumentException("Order must contain items"));

        var controller = new OrdersController(service.Object, NullLogger<OrdersController>.Instance);

        var request = new CreateOrderRequest
        {
            UserId = Guid.NewGuid(),
            Items = new List<CreateOrderItemRequest>(),
        };

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateOrder_WhenServiceThrowsKeyNotFoundException_ShouldReturnNotFound()
    {
        // Arrange
        var service = new Mock<IOrderService>(MockBehavior.Strict);
        service.Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderRequest>()))
            .ThrowsAsync(new KeyNotFoundException("User profile not found"));

        var controller = new OrdersController(service.Object, NullLogger<OrdersController>.Instance);

        var request = new CreateOrderRequest
        {
            UserId = Guid.NewGuid(),
            Items = new List<CreateOrderItemRequest> { new() { ProductId = Guid.NewGuid(), Quantity = 1 } },
        };

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CreateOrder_WhenServiceThrowsInvalidOperationException_ShouldReturnConflict()
    {
        // Arrange
        var service = new Mock<IOrderService>(MockBehavior.Strict);
        service.Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderRequest>()))
            .ThrowsAsync(new InvalidOperationException("Insufficient stock"));

        var controller = new OrdersController(service.Object, NullLogger<OrdersController>.Instance);

        var request = new CreateOrderRequest
        {
            UserId = Guid.NewGuid(),
            Items = new List<CreateOrderItemRequest> { new() { ProductId = Guid.NewGuid(), Quantity = 1 } },
        };

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
    }

    [Fact]
    public async Task CreateOrder_WhenServiceThrowsHttpRequestException_ShouldReturn503()
    {
        // Arrange
        var service = new Mock<IOrderService>(MockBehavior.Strict);
        service.Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderRequest>()))
            .ThrowsAsync(new HttpRequestException("Service unavailable"));

        var controller = new OrdersController(service.Object, NullLogger<OrdersController>.Instance);

        var request = new CreateOrderRequest
        {
            UserId = Guid.NewGuid(),
            Items = new List<CreateOrderItemRequest> { new() { ProductId = Guid.NewGuid(), Quantity = 1 } },
        };

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        var obj = result.Should().BeOfType<ObjectResult>().Subject;
        obj.StatusCode.Should().Be(503);
    }

    [Fact]
    public async Task GetOrder_WhenServiceReturnsNull_ShouldReturnNotFound()
    {
        // Arrange
        var service = new Mock<IOrderService>(MockBehavior.Strict);
        service.Setup(s => s.GetOrderAsync(It.IsAny<Guid>()))
            .ReturnsAsync((OrderDetailResponse?)null);

        var controller = new OrdersController(service.Object, NullLogger<OrdersController>.Instance);

        // Act
        var result = await controller.GetOrder(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}

