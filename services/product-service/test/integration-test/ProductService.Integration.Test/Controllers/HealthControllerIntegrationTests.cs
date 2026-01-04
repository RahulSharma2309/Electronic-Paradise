using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ProductService.Integration.Test.Controllers;

[Collection("ProductServiceIntegration")]
public class HealthControllerIntegrationTests
{
    private readonly ProductServiceFixture _fixture;

    public HealthControllerIntegrationTests(ProductServiceFixture fixture)
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
        content.Should().Contain("Healthy");
        content.Should().Contain("product-service");
    }
}



