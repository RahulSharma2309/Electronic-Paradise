using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ProductService.Abstraction.DTOs;
using ProductService.Abstraction.Models;
using ProductService.API.Controllers;
using ProductService.Core.Business;
using Xunit;

namespace ProductService.API.Test.Controllers;

public class ProductsControllerTests
{
    [Fact]
    public async Task GetById_WhenServiceReturnsNull_ShouldReturnNotFound()
    {
        // Arrange
        var service = new Mock<IProductService>();
        service.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

        var controller = new ProductsController(service.Object, NullLogger<ProductsController>.Instance);

        // Act
        var result = await controller.GetById(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_WhenServiceThrowsArgumentException_ShouldReturnBadRequestWithError()
    {
        // Arrange
        var service = new Mock<IProductService>();
        service.Setup(s => s.CreateAsync(It.IsAny<Product>())).ThrowsAsync(new ArgumentException("Name is required"));

        var controller = new ProductsController(service.Object, NullLogger<ProductsController>.Instance);
        var dto = new CreateProductDto
        {
            Name = string.Empty,
            Price = 0,
            Stock = 0,
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.Should().NotBeNull();

        var errorProp = badRequest.Value!.GetType().GetProperty("error");
        errorProp.Should().NotBeNull();
        errorProp!.GetValue(badRequest.Value)!.ToString().Should().Contain("Name is required");
    }

    [Fact]
    public async Task Reserve_WhenQuantityIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var service = new Mock<IProductService>();
        var controller = new ProductsController(service.Object, NullLogger<ProductsController>.Instance);

        // Act
        var result = await controller.Reserve(Guid.NewGuid(), new ReleaseDto { Quantity = 0 });

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Reserve_WhenProductNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var service = new Mock<IProductService>();
        service.Setup(s => s.ReserveAsync(It.IsAny<Guid>(), It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException());

        var controller = new ProductsController(service.Object, NullLogger<ProductsController>.Instance);

        // Act
        var result = await controller.Reserve(Guid.NewGuid(), new ReleaseDto { Quantity = 1 });

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Reserve_WhenInsufficientStock_ShouldReturnConflictWithError()
    {
        // Arrange
        var service = new Mock<IProductService>();
        service.Setup(s => s.ReserveAsync(It.IsAny<Guid>(), It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException("Insufficient stock"));

        var controller = new ProductsController(service.Object, NullLogger<ProductsController>.Instance);

        // Act
        var result = await controller.Reserve(Guid.NewGuid(), new ReleaseDto { Quantity = 1 });

        // Assert
        var conflict = result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflict.Value.Should().NotBeNull();

        var errorProp = conflict.Value!.GetType().GetProperty("error");
        errorProp.Should().NotBeNull();
        errorProp!.GetValue(conflict.Value)!.ToString().Should().Contain("Insufficient stock");
    }

    [Fact]
    public async Task Release_WhenQuantityIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var service = new Mock<IProductService>();
        var controller = new ProductsController(service.Object, NullLogger<ProductsController>.Instance);

        // Act
        var result = await controller.Release(Guid.NewGuid(), new ReleaseDto { Quantity = -1 });

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Release_WhenProductNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var service = new Mock<IProductService>();
        service.Setup(s => s.ReleaseAsync(It.IsAny<Guid>(), It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException());

        var controller = new ProductsController(service.Object, NullLogger<ProductsController>.Instance);

        // Act
        var result = await controller.Release(Guid.NewGuid(), new ReleaseDto { Quantity = 1 });

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}



