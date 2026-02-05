using Microsoft.EntityFrameworkCore;
using UserService.Abstraction.Models;

namespace UserService.Core.Data;

/// <summary>
/// Database context for the User service.
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
    /// Gets or sets the user profiles table.
    /// </summary>
    public DbSet<UserProfile> Users { get; set; } = null!;

    /// <summary>
    /// Configures the model for the User service database context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserProfile>()
            .HasIndex(u => u.UserId)
            .IsUnique();

        // Market-standard uniqueness: a phone number should not map to multiple profiles.
        // Allows null/empty phone values for optional scenarios.
        modelBuilder.Entity<UserProfile>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique()
            .HasFilter("[PhoneNumber] IS NOT NULL");
    }
}
