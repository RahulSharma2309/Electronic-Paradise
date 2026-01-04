using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Gateway.Integration.Test.Controllers;

[Collection("GatewayIntegration")]
public class HealthControllerIntegrationTests
{
    private readonly GatewayFixture _fixture;

    public HealthControllerIntegrationTests(GatewayFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Get_ReturnsOkAndHealthyStatus()
    {
        // Act
        var response = await _fixture.Client.GetAsync("/api/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("ok");
        content.Should().Contain("gateway");
    }
}

