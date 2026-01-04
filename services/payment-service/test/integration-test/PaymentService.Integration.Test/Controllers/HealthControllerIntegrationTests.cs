using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PaymentService.Integration.Test.Controllers;

[Collection("PaymentServiceIntegration")]
public class HealthControllerIntegrationTests
{
    private readonly PaymentServiceFixture _fixture;

    public HealthControllerIntegrationTests(PaymentServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Get_ReturnsOkAndHealthyStatus()
    {
        var response = await _fixture.Client.GetAsync("/api/health");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Healthy");
        content.Should().Contain("payment-service");
    }
}

