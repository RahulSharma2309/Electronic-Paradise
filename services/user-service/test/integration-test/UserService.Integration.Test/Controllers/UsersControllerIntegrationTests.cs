using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using UserService.Abstraction.DTOs;
using Xunit;

namespace UserService.Integration.Test.Controllers;

[Collection("UserServiceIntegration")]
public class UsersControllerIntegrationTests
{
    private readonly UserServiceFixture _fixture;

    public UsersControllerIntegrationTests(UserServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Create_ValidData_ReturnsCreatedProfile()
    {
        var userId = Guid.NewGuid();
        var phoneNumber = $"+1{Random.Shared.NextInt64(1000000000, 9999999999)}";
        var createDto = new CreateUserDto
        {
            UserId = userId,
            FirstName = "Integration",
            LastName = "User",
            Address = "123 Test St",
            PhoneNumber = phoneNumber
        };

        var response = await _fixture.Client.PostAsJsonAsync("/api/users", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var profile = await response.Content.ReadFromJsonAsync<UserProfileDto>();
        profile.Should().NotBeNull();
        profile!.UserId.Should().Be(userId);
    }
}



