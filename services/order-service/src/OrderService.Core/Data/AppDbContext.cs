using Microsoft.EntityFrameworkCore;
using OrderService.Abstraction.Models;

namespace OrderService.Core.Data;

/// <summary>
/// Database context for the Order service.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the orders table.
    /// </summary>
    public DbSet<Order> Orders { get; set; } = null!;

    /// <summary>
    /// Gets or sets the order items table.
    /// </summary>
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
}
