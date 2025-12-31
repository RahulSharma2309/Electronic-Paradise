using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ep.Platform.DependencyInjection;

public static class DbContextExtensions
{
    /// <summary>
    /// Registers a SQL Server-backed DbContext using the conventional "DefaultConnection" string.
    /// Consumers can override the connection name and optionally tweak EF options without duplicating boilerplate.
    /// </summary>
    public static IServiceCollection AddEpSqlServerDbContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName = "DefaultConnection",
        Action<SqlServerDbContextOptionsBuilder>? sqlServerOptions = null,
        Action<DbContextOptionsBuilder>? options = null)
        where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionName)
            ?? throw new InvalidOperationException($"Connection string '{connectionName}' was not found.");

        services.AddDbContext<TContext>(builder =>
        {
            builder.UseSqlServer(connectionString, sqlServerOptions);
            options?.Invoke(builder);
        });

        return services;
    }
}

