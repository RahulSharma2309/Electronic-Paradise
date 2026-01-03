using AuthService.Abstraction.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}





