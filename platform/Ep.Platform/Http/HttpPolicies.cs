namespace Ep.Platform.Http
{
    using System.Net;
    using System.Net.Http;
    using Polly;
    using Polly.Extensions.Http;

    /// <summary>
    /// Provides common Polly policies for platform HTTP clients.
    /// </summary>
    public static class HttpPolicies
    {
        /// <summary>
        /// Basic retry with jitter for transient HTTP errors and 429 responses.
        /// </summary>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="baseDelaySeconds">The base delay (in seconds) used for exponential backoff.</param>
        /// <returns>A retry policy for HttpClient calls.</returns>
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
                        var exponentialBackoff = baseDelaySeconds * Math.Pow(2, retryAttempt - 1);

                        return TimeSpan.FromSeconds(exponentialBackoff + jitter);
                    });
        }
    }
}
