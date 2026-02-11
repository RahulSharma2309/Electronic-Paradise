using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentService.Abstraction.DTOs.Requests;
using PaymentService.Abstraction.Models;
using PaymentService.Core.Business;
using PaymentService.Core.Mappers;
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
        var service = new PaymentServiceImpl(repo.Object, new PaymentMapper(), httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var request = new ProcessPaymentRequest
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 0,
        };

        // Act
        var act = () => service.ProcessPaymentAsync(request);

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

        var service = new PaymentServiceImpl(repo.Object, new PaymentMapper(), httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var request = new ProcessPaymentRequest
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var act = () => service.ProcessPaymentAsync(request);

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

        var service = new PaymentServiceImpl(repo.Object, new PaymentMapper(), httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var request = new ProcessPaymentRequest
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var act = () => service.ProcessPaymentAsync(request);

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

        var service = new PaymentServiceImpl(repo.Object, new PaymentMapper(), httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var request = new ProcessPaymentRequest
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var act = () => service.ProcessPaymentAsync(request);

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

        var service = new PaymentServiceImpl(repo.Object, new PaymentMapper(), httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var request = new ProcessPaymentRequest
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 25,
        };

        // Act
        var payment = await service.ProcessPaymentAsync(request);

        // Assert
        payment.OrderId.Should().Be(request.OrderId);
        payment.UserId.Should().Be(request.UserId);
        payment.Amount.Should().Be(request.Amount);
        payment.Status.Should().Be(PaymentStatus.Success.ToString());

        repo.Verify(r => r.AddAsync(It.Is<PaymentRecord>(p =>
            p.OrderId == request.OrderId &&
            p.UserId == request.UserId &&
            p.Amount == request.Amount &&
            p.Status == PaymentStatus.Success.ToString())), Times.Once);
    }

    [Fact]
    public async Task RefundPaymentAsync_WhenUserServiceSucceeds_ShouldMarkOriginalPaymentAsRefunded()
    {
        // Arrange
        var repo = new Mock<IPaymentRepository>(MockBehavior.Strict);
        var originalPayment = new PaymentRecord
        {
            Id = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 10,
            Status = PaymentStatus.Success.ToString(),
            Timestamp = DateTime.UtcNow,
        };

        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(originalPayment);
        repo.Setup(r => r.UpdateAsync(It.IsAny<PaymentRecord>()))
            .ReturnsAsync((PaymentRecord p) => p);

        var handler = new StubHttpMessageHandler(_ => StubHttpMessageHandler.Json(HttpStatusCode.OK));
        var http = new HttpClient(handler) { BaseAddress = new Uri("http://user") };

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(f => f.CreateClient("user")).Returns(http);

        var service = new PaymentServiceImpl(repo.Object, new PaymentMapper(), httpClientFactory.Object, NullLogger<PaymentServiceImpl>.Instance);

        var request = new RefundPaymentRequest
        {
            PaymentId = originalPayment.Id,
            Reason = "Test refund",
        };

        // Act
        var payment = await service.RefundPaymentAsync(request);

        // Assert
        payment.Id.Should().Be(originalPayment.Id);
        payment.Status.Should().Be(PaymentStatus.Refunded.ToString());

        repo.Verify(r => r.UpdateAsync(It.Is<PaymentRecord>(p =>
            p.Id == originalPayment.Id &&
            p.Status == PaymentStatus.Refunded.ToString())), Times.Once);
    }
}




