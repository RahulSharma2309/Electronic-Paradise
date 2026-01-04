using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Controllers;
using Xunit;

namespace ProductService.API.Test.Controllers;

public class HealthControllerTests
{
    /// <summary>
    /// Verifies that the health check endpoint returns OK (200) result.
    /// </summary>
    [Fact]
    public void Get_ShouldReturnOkResult()
    {
        // Arrange
        var controller = new HealthController();

        // Act
        var result = controller.Get() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
    }

    /// <summary>
    /// Verifies that the health check endpoint returns the correct service name.
    /// </summary>
    [Fact]
    public void Get_ShouldReturnCorrectServiceName()
    {
        // Arrange
        var controller = new HealthController();

        // Act
        var result = Assert.IsType<OkObjectResult>(controller.Get());
        var value = result.Value;

        // Assert
        value.Should().NotBeNull();
        var serviceProperty = value!.GetType().GetProperty("service");
        serviceProperty.Should().NotBeNull();
        var serviceName = serviceProperty!.GetValue(value);
        serviceName.Should().Be("product-service");
    }
}