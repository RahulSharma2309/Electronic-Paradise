// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserServiceImplTests.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Abstraction.DTOs.Requests;
using UserService.Abstraction.Models;
using UserService.Core.Business;
using UserService.Core.Mappers;
using UserService.Core.Repository;
using Xunit;

namespace UserService.Core.Test.Business;

public class UserServiceImplTests
{
    private readonly Mock<IUserRepository> repo = new();
    private readonly Mock<ILogger<UserServiceImpl>> logger = new();

    private static IUserProfileMapper CreateMapper() => new UserProfileMapper();

    private UserServiceImpl CreateSut()
    {
        return new UserServiceImpl(this.repo.Object, CreateMapper(), this.logger.Object);
    }

    [Fact]
    public void GivenCtor_WhenAllSpecified_ThenInitializes()
    {
        var sut = this.CreateSut();
        Assert.NotNull(sut);
    }

    [Fact]
    public async Task GivenMissingProfile_WhenGetByIdAsync_ThenReturnsNull()
    {
        var id = Guid.NewGuid();
        this.repo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((UserProfile?)null);

        var sut = this.CreateSut();
        var res = await sut.GetByIdAsync(id);

        Assert.Null(res);
    }

    [Fact]
    public async Task GivenWhitespacePhone_WhenPhoneNumberExistsAsync_ThenThrows()
    {
        var sut = this.CreateSut();

        await Assert.ThrowsAsync<ArgumentException>(() => sut.PhoneNumberExistsAsync(" "));
    }

    [Fact]
    public async Task GivenCreateRequest_WhenCreateAsync_ThenCreates()
    {
        var request = new CreateUserProfileRequest
        {
            UserId = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "+12345678901",
        };

        this.repo.Setup(x => x.GetByPhoneNumberAsync(request.PhoneNumber!)).ReturnsAsync((UserProfile?)null);
        this.repo.Setup(x => x.GetByUserIdAsync(request.UserId)).ReturnsAsync((UserProfile?)null);

        this.repo.Setup(x => x.CreateAsync(It.IsAny<UserProfile>()))
            .ReturnsAsync((UserProfile m) =>
            {
                m.Id = Guid.NewGuid();
                return m;
            });

        var sut = this.CreateSut();
        var created = await sut.CreateAsync(request);

        Assert.NotNull(created);
        Assert.Equal(request.UserId, created.UserId);
    }
}

