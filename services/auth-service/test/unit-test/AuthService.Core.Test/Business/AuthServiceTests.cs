// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthServiceTests.cs" company="FreshHarvest-Market">
//   © FreshHarvest-Market. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using AuthService.Abstraction.DTOs.Requests;
using AuthService.Abstraction.Models;
using AuthService.Core.Clients;
using AuthService.Core.Clients.Models;
using AuthService.Core.Mappers;
using AuthService.Core.Repository;
using Ep.Platform.Security;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

using AuthServiceBusiness = AuthService.Core.Business.AuthService;

namespace AuthService.Core.Test.Business;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> userRepository = new();
    private readonly Mock<IUserServiceClient> userServiceClient = new();
    private readonly Mock<IPasswordHasher> passwordHasher = new();
    private readonly Mock<IJwtTokenGenerator> jwtTokenGenerator = new();
    private readonly Mock<ILogger<AuthServiceBusiness>> logger = new();

    private static IAuthMapper CreateMapper() => new AuthMapper();

    private AuthServiceBusiness CreateSut()
    {
        return new AuthServiceBusiness(
            this.userRepository.Object,
            this.userServiceClient.Object,
            CreateMapper(),
            this.passwordHasher.Object,
            this.jwtTokenGenerator.Object,
            this.logger.Object);
    }

    [Fact]
    public void GivenCtor_WhenAllSpecified_ThenInitializes()
    {
        var sut = this.CreateSut();
        Assert.NotNull(sut);
    }

    [Fact]
    public async Task GivenValidData_WhenRegisterAsync_ThenReturnsAuthResponse()
    {
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            FullName = "Test User",
            PhoneNumber = "+12345678901",
            Address = "123 Test St",
        };

        this.userRepository.Setup(x => x.EmailExistsAsync(request.Email)).ReturnsAsync(false);
        this.userServiceClient.Setup(x => x.PhoneExistsAsync(request.PhoneNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        this.passwordHasher.Setup(x => x.HashPassword(request.Password)).Returns("hashed_password");

        this.userRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        this.userServiceClient
            .Setup(x => x.CreateUserProfileAsync(It.IsAny<CreateUserProfileRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        this.jwtTokenGenerator
            .Setup(x => x.GenerateToken(It.IsAny<Dictionary<string, string>>(), It.IsAny<TimeSpan?>()))
            .Returns("token");

        var sut = this.CreateSut();

        var result = await sut.RegisterAsync(request);

        Assert.NotNull(result);
        Assert.Equal("token", result.Token);
        Assert.NotNull(result.User);
        Assert.Equal(request.Email, result.User.Email);
        Assert.Equal(request.FullName, result.User.FullName);
    }

    [Fact]
    public async Task GivenNonExistentUser_WhenLoginAsync_ThenThrowsNotFound()
    {
        var request = new LoginRequest { Email = "missing@example.com", Password = "Password123!" };

        this.userRepository.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((User?)null);

        var sut = this.CreateSut();

        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.LoginAsync(request));
    }

    [Fact]
    public async Task GivenBadPassword_WhenLoginAsync_ThenThrowsUnauthorized()
    {
        var request = new LoginRequest { Email = "test@example.com", Password = "WrongPass123!" };
        var user = new User { Email = request.Email, PasswordHash = "hashed_password", FullName = "Test User" };

        this.userRepository.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        this.passwordHasher.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(false);

        var sut = this.CreateSut();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.LoginAsync(request));
    }

    [Fact]
    public async Task GivenMissingUser_WhenResetPasswordAsync_ThenReturnsFalse()
    {
        var request = new ResetPasswordRequest
        {
            Email = "missing@example.com",
            ResetToken = "token",
            NewPassword = "Password123!",
        };

        this.userRepository.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((User?)null);

        var sut = this.CreateSut();

        var ok = await sut.ResetPasswordAsync(request);

        Assert.False(ok);
    }
}

