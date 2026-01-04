using Microsoft.EntityFrameworkCore;
using PaymentService.Abstraction.Models;

namespace PaymentService.Core.Data;

/// <summary>
/// Database context for the Payment service.
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
    /// Gets or sets the payments table.
    /// </summary>
    public DbSet<PaymentRecord> Payments { get; set; } = null!;
}
