using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using ProductService.Abstraction.Models;
using ProductService.Core.Data;
using ProductService.Core.Repository;
using Xunit;

namespace ProductService.Core.Test.Repository;

public class ProductRepositoryTests
{
    private static AppDbContext CreateDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            // ProductRepository uses explicit transactions; EF InMemory doesn't support them and emits
            // TransactionIgnoredWarning. Ignore it so we can test repository behavior.
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task ReserveAsync_WhenQuantityIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        await using var db = CreateDbContext(Guid.NewGuid().ToString("N"));
        var repo = new ProductRepository(db, NullLogger<ProductRepository>.Instance);

        // Act
        var act = () => repo.ReserveAsync(Guid.NewGuid(), 0);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Quantity must be > 0*");
    }

    [Fact]
    public async Task ReserveAsync_WhenProductNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        await using var db = CreateDbContext(Guid.NewGuid().ToString("N"));
        var repo = new ProductRepository(db, NullLogger<ProductRepository>.Instance);

        // Act
        var act = () => repo.ReserveAsync(Guid.NewGuid(), 1);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*Product not found*");
    }

    [Fact]
    public async Task ReserveAsync_WhenInsufficientStock_ShouldThrowInvalidOperationException()
    {
        // Arrange
        await using var db = CreateDbContext(Guid.NewGuid().ToString("N"));
        var repo = new ProductRepository(db, NullLogger<ProductRepository>.Instance);

        var product = new Product { Id = Guid.NewGuid(), Name = "SSD", Price = 100, Stock = 2 };
        db.Products.Add(product);
        await db.SaveChangesAsync();

        // Act
        var act = () => repo.ReserveAsync(product.Id, 3);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Insufficient stock*");
    }

    [Fact]
    public async Task ReserveAsync_WhenValid_ShouldDecreaseStock()
    {
        // Arrange
        await using var db = CreateDbContext(Guid.NewGuid().ToString("N"));
        var repo = new ProductRepository(db, NullLogger<ProductRepository>.Instance);

        var product = new Product { Id = Guid.NewGuid(), Name = "GPU", Price = 500, Stock = 10 };
        db.Products.Add(product);
        await db.SaveChangesAsync();

        // Act
        var newStock = await repo.ReserveAsync(product.Id, 4);

        // Assert
        newStock.Should().Be(6);
        (await db.Products.FindAsync(product.Id))!.Stock.Should().Be(6);
    }

    [Fact]
    public async Task ReleaseAsync_WhenQuantityIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        await using var db = CreateDbContext(Guid.NewGuid().ToString("N"));
        var repo = new ProductRepository(db, NullLogger<ProductRepository>.Instance);

        // Act
        var act = () => repo.ReleaseAsync(Guid.NewGuid(), -1);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Quantity must be > 0*");
    }

    [Fact]
    public async Task ReleaseAsync_WhenProductNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        await using var db = CreateDbContext(Guid.NewGuid().ToString("N"));
        var repo = new ProductRepository(db, NullLogger<ProductRepository>.Instance);

        // Act
        var act = () => repo.ReleaseAsync(Guid.NewGuid(), 1);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*Product not found*");
    }

    [Fact]
    public async Task ReleaseAsync_WhenValid_ShouldIncreaseStock()
    {
        // Arrange
        await using var db = CreateDbContext(Guid.NewGuid().ToString("N"));
        var repo = new ProductRepository(db, NullLogger<ProductRepository>.Instance);

        var product = new Product { Id = Guid.NewGuid(), Name = "RAM", Price = 40, Stock = 1 };
        db.Products.Add(product);
        await db.SaveChangesAsync();

        // Act
        var newStock = await repo.ReleaseAsync(product.Id, 3);

        // Assert
        newStock.Should().Be(4);
        (await db.Products.FindAsync(product.Id))!.Stock.Should().Be(4);
    }
}


