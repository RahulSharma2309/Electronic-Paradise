using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AuthService.Abstraction.DTOs.Requests;
using AuthService.Abstraction.DTOs.Responses;
using FluentAssertions;
using Xunit;

namespace AuthService.Integration.Test.Controllers;

[Collection("AuthServiceIntegration")]
public class AuthControllerIntegrationTests
{
    private readonly AuthServiceFixture _fixture;

    public AuthControllerIntegrationTests(AuthServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Register_ValidData_ReturnsCreated()
    {
        var email = $"it-{Guid.NewGuid():N}@example.com";
        var password = "Password123!";
        var phoneNumber = $"+1{Random.Shared.NextInt64(1000000000, 9999999999)}";

        var registerDto = new RegisterRequest
        {
            FullName = "Integration Test User",
            Email = email,
            Password = password,
            ConfirmPassword = password,
            PhoneNumber = phoneNumber,
            Address = "123 Integration St",
        };

        var response = await _fixture.Client.PostAsJsonAsync("/api/auth/register", registerDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadFromJsonAsync<AuthResponse>();
        content.Should().NotBeNull();
        content!.User.Email.Should().Be(email);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        var email = $"it-{Guid.NewGuid():N}@example.com";
        var password = "Password123!";
        var phoneNumber = $"+1{Random.Shared.NextInt64(1000000000, 9999999999)}";
        await _fixture.RegisterAndLoginUser(email, password, "Login Test User", phoneNumber, "456 Login Ave");

        var loginDto = new LoginRequest { Email = email, Password = password };
        var response = await _fixture.Client.PostAsJsonAsync("/api/auth/login", loginDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        authResponse.Should().NotBeNull();
        authResponse!.Token.Should().NotBeEmpty();
    }
}



