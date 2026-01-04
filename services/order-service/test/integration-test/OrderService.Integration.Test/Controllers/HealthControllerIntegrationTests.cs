using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace OrderService.Integration.Test.Controllers;

[Collection("OrderServiceIntegration")]
public class HealthControllerIntegrationTests
{
    private readonly OrderServiceFixture _fixture;

    public HealthControllerIntegrationTests(OrderServiceFixture fixture)
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
        content.Should().Contain("order-service");
    }
}

