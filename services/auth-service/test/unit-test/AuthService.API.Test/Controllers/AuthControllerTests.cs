// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthControllerTests.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using AuthService.Abstraction.DTOs.Requests;
using AuthService.Abstraction.DTOs.Responses;
using AuthService.API.Controllers;
using AuthService.Core.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AuthService.API.Test.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> authService = new();
    private readonly Mock<ILogger<AuthController>> logger = new();

    [Fact]
    public void GivenCtor_WhenAllSpecified_ThenInitializes()
    {
        var controller = new AuthController(this.authService.Object, this.logger.Object);
        Assert.NotNull(controller);
    }

    [Fact]
    public void GivenCtor_WhenAuthServiceNull_ThenThrows()
    {
        Assert.Throws<ArgumentNullException>(() => new AuthController(null!, this.logger.Object));
    }

    [Fact]
    public void GivenCtor_WhenLoggerNull_ThenThrows()
    {
        Assert.Throws<ArgumentNullException>(() => new AuthController(this.authService.Object, null!));
    }

    [Fact]
    public async Task GivenValidLogin_WhenLogin_ThenReturnsOk()
    {
        var request = new LoginRequest { Email = "test@example.com", Password = "Password123!" };
        var response = new AuthResponse
        {
            Token = "token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = new UserResponse { Id = Guid.NewGuid(), Email = request.Email, FullName = "Test User", CreatedAt = DateTime.UtcNow },
        };

        this.authService.Setup(x => x.LoginAsync(request)).ReturnsAsync(response);

        var controller = new AuthController(this.authService.Object, this.logger.Object);

        var result = await controller.Login(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        Assert.NotNull(ok.Value);
    }
}

