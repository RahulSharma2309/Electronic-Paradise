// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HealthControllerTests.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Net;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Controllers;
using Xunit;

namespace UserService.API.Test.Controllers
{
    public class HealthControllerTests
    {
        /// <summary>
        /// Verifies that the health check endpoint returns OK (200) status.
        /// </summary>
        [Fact]
        public void GivenHealthCheck_WhenGet_ThenReturnsOk()
        {
            // arrange
            var controller = new HealthController();

            // act
            var result = controller.Get();

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        /// <summary>
        /// Verifies that the health check endpoint returns the correct status and service name.
        /// </summary>
        [Fact]
        public void GivenHealthCheck_WhenGet_ThenReturnsHealthyStatus()
        {
            // arrange
            var controller = new HealthController();

            // act
            var result = controller.Get();

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;
            Assert.NotNull(value);

            var statusProperty = value.GetType().GetProperty("status");
            var serviceProperty = value.GetType().GetProperty("service");

            Assert.NotNull(statusProperty);
            Assert.NotNull(serviceProperty);

            Assert.Equal("Healthy", statusProperty.GetValue(value));
            Assert.Equal("user-service", serviceProperty.GetValue(value));
        }
    }
}