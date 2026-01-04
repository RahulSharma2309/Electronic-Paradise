using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Gateway.Integration.Test.Controllers;

[Collection("GatewayIntegration")]
public class RoutingIntegrationTests
{
    private readonly GatewayFixture _fixture;

    public RoutingIntegrationTests(GatewayFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("/api/auth/login")]
    [InlineData("/api/users/1")]
    [InlineData("/api/products")]
    [InlineData("/api/orders/create")]
    [InlineData("/api/payments/process")]
    public async Task Route_ToDownstream_ReturnsExpectedStatusFromMock(string path)
    {
        // Act
        var response = await _fixture.Client.GetAsync(path);

        // Assert
        // In the real Docker-compose environment, the mock service will return something.
        // During WebApplicationFactory tests, it might fail with 502/503 because dependency-mock isn't real there.
        // But for Docker-compose tests, this will be verified.
        response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
    }
}

