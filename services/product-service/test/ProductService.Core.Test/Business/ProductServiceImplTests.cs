using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ProductService.Abstraction.Models;
using ProductService.Core.Business;
using ProductService.Core.Repository;
using Xunit;

namespace ProductService.Core.Test.Business;

public class ProductServiceImplTests
{
    [Fact]
    public async Task CreateAsync_WhenNameIsMissing_ShouldThrowArgumentException_AndNotCallRepo()
    {
        // Arrange
        var repo = new Mock<IProductRepository>(MockBehavior.Strict);
        var service = new ProductServiceImpl(repo.Object, NullLogger<ProductServiceImpl>.Instance);

        var product = new Product
        {
            Name = "   ",
            Price = 10,
            Stock = 5,
        };

        // Act
        var act = () => service.CreateAsync(product);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Name is required*");

        repo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenPriceIsNegative_ShouldThrowArgumentException_AndNotCallRepo()
    {
        // Arrange
        var repo = new Mock<IProductRepository>(MockBehavior.Strict);
        var service = new ProductServiceImpl(repo.Object, NullLogger<ProductServiceImpl>.Instance);

        var product = new Product
        {
            Name = "Keyboard",
            Price = -1,
            Stock = 5,
        };

        // Act
        var act = () => service.CreateAsync(product);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Price must be >= 0*");

        repo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenStockIsNegative_ShouldThrowArgumentException_AndNotCallRepo()
    {
        // Arrange
        var repo = new Mock<IProductRepository>(MockBehavior.Strict);
        var service = new ProductServiceImpl(repo.Object, NullLogger<ProductServiceImpl>.Instance);

        var product = new Product
        {
            Name = "Mouse",
            Price = 10,
            Stock = -1,
        };

        // Act
        var act = () => service.CreateAsync(product);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Stock must be >= 0*");

        repo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ShouldCallRepoAddAsync()
    {
        // Arrange
        var repo = new Mock<IProductRepository>(MockBehavior.Strict);
        repo.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        var service = new ProductServiceImpl(repo.Object, NullLogger<ProductServiceImpl>.Instance);

        var product = new Product
        {
            Name = "Headphones",
            Price = 50,
            Stock = 2,
        };

        // Act
        await service.CreateAsync(product);

        // Assert
        repo.Verify(r => r.AddAsync(It.Is<Product>(p =>
            p.Name == "Headphones" &&
            p.Price == 50 &&
            p.Stock == 2)), Times.Once);
    }
}




