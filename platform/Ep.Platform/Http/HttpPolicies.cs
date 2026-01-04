using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace Ep.Platform.Http;

public static class HttpPolicies
{
    /// <summary>
    /// Basic retry with jitter for transient HTTP errors and 429 responses.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetDefaultRetryPolicy(int retryCount = 3, double baseDelaySeconds = 0.5)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                retryCount,
                retryAttempt =>
                {
                    var jitter = Random.Shared.NextDouble() * 0.5;
                    return TimeSpan.FromSeconds(baseDelaySeconds * Math.Pow(2, retryAttempt - 1) + jitter);
                });
    }
}

















