namespace Ep.Platform.DependencyInjection
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides DI extension methods for configuring EF Core DbContexts.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Registers a SQL Server-backed DbContext using a named connection string from configuration.
        /// Consumers can override the connection name and optionally tweak EF options without duplicating boilerplate.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="connectionName">The connection string name in configuration.</param>
        /// <param name="sqlServerOptions">Optional SQL Server provider options callback.</param>
        /// <param name="options">Optional general EF Core options callback.</param>
        /// <returns>The same service collection to allow call chaining.</returns>
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
}
