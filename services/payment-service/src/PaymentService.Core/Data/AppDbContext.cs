using Microsoft.EntityFrameworkCore;
using PaymentService.Abstraction.Models;

namespace PaymentService.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<PaymentRecord> Payments { get; set; } = null!;
}
