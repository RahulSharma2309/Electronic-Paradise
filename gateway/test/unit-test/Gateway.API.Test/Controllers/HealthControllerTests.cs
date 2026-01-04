using FluentAssertions;
using Gateway.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Gateway.API.Test.Controllers;

public class HealthControllerTests
{
    private readonly HealthController _controller;

    public HealthControllerTests()
    {
        _controller = new HealthController();
    }

    [Fact]
    public void Get_ReturnsOkResultWithExpectedContent()
    {
        // Act
        var result = _controller.Get();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value;
        
        // Use reflection or dynamic to check properties of the anonymous object
        value.Should().NotBeNull();
        
        var statusProp = value!.GetType().GetProperty("status");
        var serviceProp = value!.GetType().GetProperty("service");

        statusProp.Should().NotBeNull();
        serviceProp.Should().NotBeNull();

        statusProp!.GetValue(value).Should().Be("ok");
        serviceProp!.GetValue(value).Should().Be("gateway");
    }
}

