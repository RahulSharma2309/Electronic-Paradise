using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentService.Abstraction.DTOs;
using PaymentService.Abstraction.Models;
using PaymentService.Core.Business;
using PaymentService.Core.Repository;
using PaymentService.Core.Test.Business.Fakes;
using Xunit;

namespace PaymentService.Core.Test.Business;

public class PaymentServiceImplTests
{
    [Fact]
    public async Task ProcessPaymentAsync_WhenAmountIsNotPositive_ShouldThrowArgumentException()
    {
        // Arrange
        var repo = new Mock<IPaymentRepository>(MockBehavior.Strict);
        var httpClientFactory = new Mock<IHttpClientFactory>(MockBehavior.Strict);
        var service = new PaymentServiceImpl(repo.Object, httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 0,
        };

        // Act
        var act = () => service.ProcessPaymentAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Amount must be greater than 0*");
    }

    [Fact]
    public async Task ProcessPaymentAsync_WhenUserServiceReturnsNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var repo = new Mock<IPaymentRepository>(MockBehavior.Strict);

        var handler = new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.NotFound));
        var http = new HttpClient(handler) { BaseAddress = new Uri("http://user") };

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(f => f.CreateClient("user")).Returns(http);

        var service = new PaymentServiceImpl(repo.Object, httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var act = () => service.ProcessPaymentAsync(dto);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*User not found*");
    }

    [Fact]
    public async Task ProcessPaymentAsync_WhenUserServiceReturnsConflict_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var repo = new Mock<IPaymentRepository>(MockBehavior.Strict);

        var handler = new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.Conflict));
        var http = new HttpClient(handler) { BaseAddress = new Uri("http://user") };

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(f => f.CreateClient("user")).Returns(http);

        var service = new PaymentServiceImpl(repo.Object, httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var act = () => service.ProcessPaymentAsync(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Insufficient wallet balance*");
    }

    [Fact]
    public async Task ProcessPaymentAsync_WhenUserServiceReturnsNonSuccess_ShouldThrowHttpRequestException()
    {
        // Arrange
        var repo = new Mock<IPaymentRepository>(MockBehavior.Strict);

        var handler = new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.InternalServerError));
        var http = new HttpClient(handler) { BaseAddress = new Uri("http://user") };

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(f => f.CreateClient("user")).Returns(http);

        var service = new PaymentServiceImpl(repo.Object, httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var act = () => service.ProcessPaymentAsync(dto);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*Wallet debit failed*");
    }

    [Fact]
    public async Task ProcessPaymentAsync_WhenUserServiceSucceeds_ShouldPersistPaidRecord()
    {
        // Arrange
        var repo = new Mock<IPaymentRepository>(MockBehavior.Strict);
        repo.Setup(r => r.AddAsync(It.IsAny<PaymentRecord>()))
            .ReturnsAsync((PaymentRecord p) => p);

        var handler = new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.OK));
        var http = new HttpClient(handler) { BaseAddress = new Uri("http://user") };

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(f => f.CreateClient("user")).Returns(http);

        var service = new PaymentServiceImpl(repo.Object, httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 25,
        };

        // Act
        var payment = await service.ProcessPaymentAsync(dto);

        // Assert
        payment.OrderId.Should().Be(dto.OrderId);
        payment.UserId.Should().Be(dto.UserId);
        payment.Amount.Should().Be(dto.Amount);
        payment.Status.Should().Be("Paid");

        repo.Verify(r => r.AddAsync(It.Is<PaymentRecord>(p =>
            p.OrderId == dto.OrderId &&
            p.UserId == dto.UserId &&
            p.Amount == dto.Amount &&
            p.Status == "Paid")), Times.Once);
    }

    [Fact]
    public async Task RefundPaymentAsync_WhenUserServiceSucceeds_ShouldPersistRefundedRecordWithNegativeAmount()
    {
        // Arrange
        var repo = new Mock<IPaymentRepository>(MockBehavior.Strict);
        repo.Setup(r => r.AddAsync(It.IsAny<PaymentRecord>()))
            .ReturnsAsync((PaymentRecord p) => p);

        var handler = new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.OK));
        var http = new HttpClient(handler) { BaseAddress = new Uri("http://user") };

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(f => f.CreateClient("user")).Returns(http);

        var service = new PaymentServiceImpl(repo.Object, httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var dto = new RefundPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var payment = await service.RefundPaymentAsync(dto);

        // Assert
        payment.OrderId.Should().Be(dto.OrderId);
        payment.UserId.Should().Be(dto.UserId);
        payment.Amount.Should().Be(-dto.Amount);
        payment.Status.Should().Be("Refunded");

        repo.Verify(r => r.AddAsync(It.Is<PaymentRecord>(p =>
            p.OrderId == dto.OrderId &&
            p.UserId == dto.UserId &&
            p.Amount == -dto.Amount &&
            p.Status == "Refunded")), Times.Once);
    }
}



