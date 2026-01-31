using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ProductService.Abstraction.DTOs.Requests;
using ProductService.Abstraction.DTOs.Responses;
using ProductService.Abstraction.Models;
using ProductService.Core.Business;
using ProductService.Core.Mappers;
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
        var mapper = new Mock<IProductMapper>(MockBehavior.Strict);
        var service = new ProductServiceImpl(repo.Object, mapper.Object, NullLogger<ProductServiceImpl>.Instance);

        var request = new CreateProductRequest
        {
            Name = "   ",
            Price = 10,
            Stock = 5,
        };

        mapper.Setup(m => m.ToEntity(It.IsAny<CreateProductRequest>())).Returns(new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
        });

        // Act
        var act = () => service.CreateAsync(request);

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
        var mapper = new Mock<IProductMapper>(MockBehavior.Strict);
        var service = new ProductServiceImpl(repo.Object, mapper.Object, NullLogger<ProductServiceImpl>.Instance);

        var request = new CreateProductRequest
        {
            Name = "Keyboard",
            Price = -1,
            Stock = 5,
        };

        mapper.Setup(m => m.ToEntity(It.IsAny<CreateProductRequest>())).Returns(new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
        });

        // Act
        var act = () => service.CreateAsync(request);

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
        var mapper = new Mock<IProductMapper>(MockBehavior.Strict);
        var service = new ProductServiceImpl(repo.Object, mapper.Object, NullLogger<ProductServiceImpl>.Instance);

        var request = new CreateProductRequest
        {
            Name = "Mouse",
            Price = 10,
            Stock = -1,
        };

        mapper.Setup(m => m.ToEntity(It.IsAny<CreateProductRequest>())).Returns(new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
        });

        // Act
        var act = () => service.CreateAsync(request);

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

        var mapper = new Mock<IProductMapper>(MockBehavior.Strict);
        var service = new ProductServiceImpl(repo.Object, mapper.Object, NullLogger<ProductServiceImpl>.Instance);

        var request = new CreateProductRequest
        {
            Name = "Headphones",
            Price = 50,
            Stock = 2,
        };

        mapper.Setup(m => m.ToEntity(It.IsAny<CreateProductRequest>())).Returns(new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
        });

        mapper.Setup(m => m.ToDetailResponse(It.IsAny<Product>())).Returns(new ProductDetailResponse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = null,
            Price = request.Price,
            Stock = request.Stock,
            Category = null,
            Brand = null,
            Sku = null,
            Unit = null,
            ImageUrl = null,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            Certification = null,
            Metadata = null,
        });

        // Act
        await service.CreateAsync(request);

        // Assert
        repo.Verify(r => r.AddAsync(It.Is<Product>(p =>
            p.Name == "Headphones" &&
            p.Price == 50 &&
            p.Stock == 2)), Times.Once);
    }
}




