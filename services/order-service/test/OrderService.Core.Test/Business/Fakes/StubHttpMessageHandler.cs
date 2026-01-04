using System.Net;
using System.Text;

namespace OrderService.Core.Test.Business.Fakes;

public sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

    public StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_handler(request));
    }

    public static HttpResponseMessage Json(HttpStatusCode statusCode, string json = "{}")
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
    }
}



