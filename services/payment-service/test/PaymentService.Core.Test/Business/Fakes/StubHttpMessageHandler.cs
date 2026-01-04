using System.Net;
using System.Net.Http;

namespace PaymentService.Core.Test.Business.Fakes;

public sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

    public StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = _handler(request);
        return Task.FromResult(response);
    }

    public static HttpResponseMessage Json(HttpStatusCode code)
    {
        return new HttpResponseMessage(code)
        {
            Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json"),
        };
    }
}



