// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersControllerTests.cs" company="FreshHarvest-Market">
//   © FreshHarvest-Market. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Abstraction.DTOs.Responses;
using UserService.API.Controllers;
using UserService.Core.Business;
using Xunit;

namespace UserService.API.Test.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IUserService> service = new();
    private readonly Mock<ILogger<UsersController>> logger = new();

    [Fact]
    public void GivenCtor_WhenAllSpecified_ThenInitializes()
    {
        var controller = new UsersController(this.service.Object, this.logger.Object);
        Assert.NotNull(controller);
    }

    [Fact]
    public async Task GivenMissingProfile_WhenGetById_ThenNotFound()
    {
        var id = Guid.NewGuid();
        this.service.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((UserProfileDetailResponse?)null);

        var controller = new UsersController(this.service.Object, this.logger.Object);

        var result = await controller.GetById(id);

        Assert.IsType<NotFoundResult>(result);
    }
}

