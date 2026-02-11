using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OrderService.Abstraction.DTOs.Requests;
using OrderService.Abstraction.Models;
using OrderService.Core.Business;
using OrderService.Core.Mappers;
using OrderService.Core.Repository;
using OrderService.Core.Test.Business.Fakes;
using Xunit;

namespace OrderService.Core.Test.Business;

public class OrderServiceImplTests
{
    private static OrderServiceImpl CreateSut(Mock<IOrderRepository> repo, IReadOnlyDictionary<string, HttpClient> clients)
    {
        var mapper = new OrderMapper();
        var httpClientFactory = new StubHttpClientFactory(clients);
        return new OrderServiceImpl(repo.Object, mapper, httpClientFactory, NullLogger<OrderServiceImpl>.Instance);
    }

    [Fact]
    public async Task CreateOrderAsync_WhenItemsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var repo = new Mock<IOrderRepository>(MockBehavior.Strict);
        var sut = CreateSut(repo, new Dictionary<string, HttpClient>());

        var request = new CreateOrderRequest
        {
            UserId = Guid.NewGuid(),
            Items = new List<CreateOrderItemRequest>(),
        };

        // Act
        var act = () => sut.CreateOrderAsync(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Order must contain items*");
    }

    [Fact]
    public async Task CreateOrderAsync_WhenUserProfileNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var repo = new Mock<IOrderRepository>(MockBehavior.Strict);

        var userHttp = new HttpClient(new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.NotFound)))
        {
            BaseAddress = new Uri("http://user"),
        };

        var sut = CreateSut(repo, new Dictionary<string, HttpClient>
        {
            ["user"] = userHttp,
            ["product"] = new HttpClient(new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.OK))) { BaseAddress = new Uri("http://product") },
            ["payment"] = new HttpClient(new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.OK))) { BaseAddress = new Uri("http://payment") },
        });

        var request = new CreateOrderRequest
        {
            UserId = Guid.NewGuid(),
            Items = new List<CreateOrderItemRequest> { new() { ProductId = Guid.NewGuid(), Quantity = 1 } },
        };

        // Act
        var act = () => sut.CreateOrderAsync(request);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*User profile not found*");
    }

    [Fact]
    public async Task CreateOrderAsync_WhenProductNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var repo = new Mock<IOrderRepository>(MockBehavior.Strict);
        var userId = Guid.NewGuid();
        var profileId = Guid.NewGuid();

        var userHttp = new HttpClient(new StubHttpMessageHandler(_ =>
            StubHttpMessageHandler.Json(HttpStatusCode.OK, $$"""{"id":"{{profileId}}","userId":"{{userId}}","walletBalance":999}""")))
        {
            BaseAddress = new Uri("http://user"),
        };

        var productHttp = new HttpClient(new StubHttpMessageHandler(req =>
        {
            if (req.Method == HttpMethod.Get && req.RequestUri!.AbsolutePath.StartsWith("/api/products/", StringComparison.OrdinalIgnoreCase))
            {
                return StubHttpMessageHandler.Json(HttpStatusCode.NotFound);
            }

            return StubHttpMessageHandler.Json(HttpStatusCode.OK);
        }))
        {
            BaseAddress = new Uri("http://product"),
        };

        var sut = CreateSut(repo, new Dictionary<string, HttpClient>
        {
            ["user"] = userHttp,
            ["product"] = productHttp,
            ["payment"] = new HttpClient(new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.OK))) { BaseAddress = new Uri("http://payment") },
        });

        var request = new CreateOrderRequest
        {
            UserId = userId,
            Items = new List<CreateOrderItemRequest> { new() { ProductId = Guid.NewGuid(), Quantity = 1 } },
        };

        // Act
        var act = () => sut.CreateOrderAsync(request);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*Product not found*");
    }

    [Fact]
    public async Task CreateOrderAsync_WhenInsufficientStock_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var repo = new Mock<IOrderRepository>(MockBehavior.Strict);

        var userId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var userHttp = new HttpClient(new StubHttpMessageHandler(_ =>
            StubHttpMessageHandler.Json(HttpStatusCode.OK, $$"""{"id":"{{profileId}}","userId":"{{userId}}","walletBalance":999}""")))
        {
            BaseAddress = new Uri("http://user"),
        };

        var productHttp = new HttpClient(new StubHttpMessageHandler(req =>
        {
            if (req.Method == HttpMethod.Get && req.RequestUri!.AbsolutePath.EndsWith($"/api/products/{productId}", StringComparison.OrdinalIgnoreCase))
            {
                // stock 0 but requested 1
                return StubHttpMessageHandler.Json(HttpStatusCode.OK, $$"""{"id":"{{productId}}","name":"Organic Apples","price":100,"stock":0}""");
            }

            return StubHttpMessageHandler.Json(HttpStatusCode.OK);
        }))
        {
            BaseAddress = new Uri("http://product"),
        };

        var sut = CreateSut(repo, new Dictionary<string, HttpClient>
        {
            ["user"] = userHttp,
            ["product"] = productHttp,
            ["payment"] = new HttpClient(new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.OK))) { BaseAddress = new Uri("http://payment") },
        });

        var request = new CreateOrderRequest
        {
            UserId = userId,
            Items = new List<CreateOrderItemRequest> { new() { ProductId = productId, Quantity = 1 } },
        };

        // Act
        var act = () => sut.CreateOrderAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Insufficient stock*");
    }

    [Fact]
    public async Task CreateOrderAsync_WhenHappyPath_ShouldPersistAndReturnOrderDetail()
    {
        // Arrange
        var repo = new Mock<IOrderRepository>(MockBehavior.Strict);
        repo.Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => o);

        var userId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var userHttp = new HttpClient(new StubHttpMessageHandler(_ =>
            StubHttpMessageHandler.Json(HttpStatusCode.OK, $$"""{"id":"{{profileId}}","userId":"{{userId}}","walletBalance":999}""")))
        {
            BaseAddress = new Uri("http://user"),
        };

        var productHttp = new HttpClient(new StubHttpMessageHandler(req =>
        {
            var path = req.RequestUri!.AbsolutePath;
            if (req.Method == HttpMethod.Get && path.EndsWith($"/api/products/{productId}", StringComparison.OrdinalIgnoreCase))
            {
                return StubHttpMessageHandler.Json(HttpStatusCode.OK, $$"""{"id":"{{productId}}","name":"Organic Bananas","price":200,"stock":10}""");
            }

            if (req.Method == HttpMethod.Post && path.EndsWith($"/api/products/{productId}/reserve", StringComparison.OrdinalIgnoreCase))
            {
                return StubHttpMessageHandler.Json(HttpStatusCode.OK);
            }

            return StubHttpMessageHandler.Json(HttpStatusCode.OK);
        }))
        {
            BaseAddress = new Uri("http://product"),
        };

        var paymentHttp = new HttpClient(new StubHttpMessageHandler(req =>
        {
            if (req.Method == HttpMethod.Post && req.RequestUri!.AbsolutePath == "/api/payments/process")
            {
                return StubHttpMessageHandler.Json(HttpStatusCode.OK);
            }

            return StubHttpMessageHandler.Json(HttpStatusCode.OK);
        }))
        {
            BaseAddress = new Uri("http://payment"),
        };

        var sut = CreateSut(repo, new Dictionary<string, HttpClient>
        {
            ["user"] = userHttp,
            ["product"] = productHttp,
            ["payment"] = paymentHttp,
        });

        var request = new CreateOrderRequest
        {
            UserId = userId,
            Items = new List<CreateOrderItemRequest> { new() { ProductId = productId, Quantity = 2 } },
        };

        // Act
        var order = await sut.CreateOrderAsync(request);

        // Assert
        order.UserId.Should().Be(userId);
        order.TotalAmount.Should().Be(400m);
        order.Items.Should().HaveCount(1);
        order.Items[0].ProductId.Should().Be(productId);
        order.Items[0].Quantity.Should().Be(2);
        order.Items[0].UnitPrice.Should().Be(200m);
        order.Items[0].TotalPrice.Should().Be(400m);

        repo.Verify(r => r.AddAsync(It.Is<Order>(o =>
            o.UserId == userId &&
            o.TotalAmount == 400m &&
            o.Items.Count == 1)), Times.Once);
    }
}

