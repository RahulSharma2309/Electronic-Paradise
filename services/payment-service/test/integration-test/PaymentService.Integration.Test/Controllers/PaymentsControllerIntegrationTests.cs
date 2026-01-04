using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using PaymentService.Abstraction.DTOs;
using Xunit;

namespace PaymentService.Integration.Test.Controllers;

[Collection("PaymentServiceIntegration")]
public class PaymentsControllerIntegrationTests
{
    private readonly PaymentServiceFixture _fixture;

    public PaymentsControllerIntegrationTests(PaymentServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ProcessPayment_ValidData_ReturnsOk()
    {
        // Arrange
        var dto = new ProcessPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 100
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/payments/process", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        ((string)result!.status).Should().Be("Paid");
    }

    [Fact]
    public async Task RefundPayment_ValidData_ReturnsOk()
    {
        // Arrange
        var dto = new RefundPaymentDto
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 50
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/payments/refund", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        ((string)result!.status).Should().Be("Refunded");
    }

    [Fact]
    public async Task GetStatus_ExistingOrder_ReturnsStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var processDto = new ProcessPaymentDto
        {
            OrderId = orderId,
            UserId = Guid.NewGuid(),
            UserProfileId = Guid.NewGuid(),
            Amount = 200
        };
        await _fixture.Client.PostAsJsonAsync("/api/payments/process", processDto);

        // Act
        var response = await _fixture.Client.GetAsync($"/api/payments/status/{orderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        ((string)result!.status).Should().Be("Paid");
    }
}



