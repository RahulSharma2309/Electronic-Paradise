namespace OrderService.Core.Test.Business.Fakes;

public sealed class StubHttpClientFactory : IHttpClientFactory
{
    private readonly IReadOnlyDictionary<string, HttpClient> _clients;

    public StubHttpClientFactory(IReadOnlyDictionary<string, HttpClient> clients)
    {
        _clients = clients ?? throw new ArgumentNullException(nameof(clients));
    }

    public HttpClient CreateClient(string name)
    {
        if (!_clients.TryGetValue(name, out var client))
        {
            throw new InvalidOperationException($"No HttpClient registered for '{name}'.");
        }

        return client;
    }
}



