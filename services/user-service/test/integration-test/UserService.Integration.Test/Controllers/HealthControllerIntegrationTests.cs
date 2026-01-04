using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace UserService.Integration.Test.Controllers;

[Collection("UserServiceIntegration")]
public class HealthControllerIntegrationTests
{
    private readonly UserServiceFixture _fixture;

    public HealthControllerIntegrationTests(UserServiceFixture fixture)
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
        content.Should().Contain("user-service");
    }
}

