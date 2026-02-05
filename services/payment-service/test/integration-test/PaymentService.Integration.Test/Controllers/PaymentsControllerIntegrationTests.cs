using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using PaymentService.Abstraction.DTOs.Requests;
using PaymentService.Abstraction.DTOs.Responses;
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
        var dto = new ProcessPaymentRequest
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 100,
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/payments/process", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaymentDetailResponse>();
        result.Should().NotBeNull();
        result!.OrderId.Should().Be(dto.OrderId);
        result.UserId.Should().Be(dto.UserId);
        result.Amount.Should().Be(dto.Amount);
        result.Status.Should().Be("Success");
    }

    [Fact]
    public async Task RefundPayment_ValidData_ReturnsOk()
    {
        // Arrange
        var process = new ProcessPaymentRequest
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 50,
        };
        var processResponse = await _fixture.Client.PostAsJsonAsync("/api/payments/process", process);
        processResponse.EnsureSuccessStatusCode();
        var createdPayment = await processResponse.Content.ReadFromJsonAsync<PaymentDetailResponse>();

        var dto = new RefundPaymentRequest
        {
            PaymentId = createdPayment!.Id,
            Reason = "Integration test refund",
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/payments/refund", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaymentDetailResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(dto.PaymentId);
        result.Status.Should().Be("Refunded");
    }

    [Fact]
    public async Task GetStatus_ExistingOrder_ReturnsStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var processDto = new ProcessPaymentRequest
        {
            OrderId = orderId,
            UserId = Guid.NewGuid(),
            Amount = 200,
        };
        await _fixture.Client.PostAsJsonAsync("/api/payments/process", processDto);

        // Act
        var response = await _fixture.Client.GetAsync($"/api/payments/status/{orderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaymentStatusResponse>();
        result.Should().NotBeNull();
        result!.OrderId.Should().Be(orderId);
        result.Status.Should().Be("Success");
    }
}



