namespace Ep.Platform.DependencyInjection
{
    using Ep.Platform.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides DI extension methods for configuring HttpClient instances.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Registers a named HttpClient using a base address from configuration (e.g., "ServiceUrls:ProductService").
        /// A lightweight resilience policy (retry with jitter) is applied by default.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="name">The logical name of the client to register.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="baseAddressKey">Optional configuration key holding the base address (defaults to <c>ServiceUrls:{name}</c>).</param>
        /// <param name="configure">Optional callback to further customize the client builder.</param>
        /// <returns>The client builder for further configuration.</returns>
        public static IHttpClientBuilder AddEpHttpClient(
            this IServiceCollection services,
            string name,
            IConfiguration configuration,
            string? baseAddressKey = null,
            Action<IHttpClientBuilder>? configure = null)
        {
            baseAddressKey ??= $"ServiceUrls:{name}";
            var baseAddress = configuration[baseAddressKey];
            if (string.IsNullOrWhiteSpace(baseAddress))
            {
                throw new InvalidOperationException(
                    $"Base address not configured for HttpClient '{name}'. Expected key '{baseAddressKey}'.");
            }

            var builder = services.AddHttpClient(name, client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });

            builder.AddPolicyHandler(HttpPolicies.GetDefaultRetryPolicy());
            configure?.Invoke(builder);

            return builder;
        }
    }
}
