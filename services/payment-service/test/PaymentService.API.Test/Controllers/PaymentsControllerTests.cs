using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentService.Abstraction.DTOs;
using PaymentService.Abstraction.Models;
using PaymentService.API.Controllers;
using PaymentService.Core.Business;
using Xunit;

namespace PaymentService.API.Test.Controllers;

public class PaymentsControllerTests
{
    [Fact]
    public async Task ProcessPayment_WhenServiceThrowsArgumentException_ShouldReturnBadRequest()
    {
        // Arrange
        var service = new Mock<IPaymentService>();
        service.Setup(s => s.ProcessPaymentAsync(It.IsAny<ProcessPaymentDto>()))
            .ThrowsAsync(new ArgumentException("Amount must be greater than 0"));

        var controller = new PaymentsController(service.Object, NullLogger<PaymentsController>.Instance);
        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 0,
        };

        // Act
        var result = await controller.ProcessPayment(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task ProcessPayment_WhenServiceThrowsKeyNotFoundException_ShouldReturnNotFound()
    {
        // Arrange
        var service = new Mock<IPaymentService>();
        service.Setup(s => s.ProcessPaymentAsync(It.IsAny<ProcessPaymentDto>()))
            .ThrowsAsync(new KeyNotFoundException());

        var controller = new PaymentsController(service.Object, NullLogger<PaymentsController>.Instance);
        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var result = await controller.ProcessPayment(dto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task ProcessPayment_WhenServiceThrowsInvalidOperationException_ShouldReturnConflict()
    {
        // Arrange
        var service = new Mock<IPaymentService>();
        service.Setup(s => s.ProcessPaymentAsync(It.IsAny<ProcessPaymentDto>()))
            .ThrowsAsync(new InvalidOperationException("Insufficient wallet balance"));

        var controller = new PaymentsController(service.Object, NullLogger<PaymentsController>.Instance);
        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var result = await controller.ProcessPayment(dto);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
    }

    [Fact]
    public async Task ProcessPayment_WhenServiceThrowsHttpRequestException_ShouldReturn503()
    {
        // Arrange
        var service = new Mock<IPaymentService>();
        service.Setup(s => s.ProcessPaymentAsync(It.IsAny<ProcessPaymentDto>()))
            .ThrowsAsync(new HttpRequestException("User service down"));

        var controller = new PaymentsController(service.Object, NullLogger<PaymentsController>.Instance);
        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 10,
        };

        // Act
        var result = await controller.ProcessPayment(dto);

        // Assert
        var obj = result.Should().BeOfType<ObjectResult>().Subject;
        obj.StatusCode.Should().Be(503);
    }

    [Fact]
    public async Task GetPaymentStatus_WhenPaymentNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var service = new Mock<IPaymentService>();
        service.Setup(s => s.GetPaymentStatusAsync(It.IsAny<Guid>()))
            .ReturnsAsync((PaymentRecord?)null);

        var controller = new PaymentsController(service.Object, NullLogger<PaymentsController>.Instance);

        // Act
        var result = await controller.GetPaymentStatus(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}



