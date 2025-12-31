using System.Net.Http;
using Ep.Platform.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Ep.Platform.DependencyInjection;

public static class HttpClientExtensions
{
    /// <summary>
    /// Registers a named HttpClient using a base address from configuration (e.g., "ServiceUrls:ProductService").
    /// A lightweight resilience policy (retry with jitter) is applied by default.
    /// </summary>
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
            throw new InvalidOperationException($"Base address not configured for HttpClient '{name}'. Expected key '{baseAddressKey}'.");
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


