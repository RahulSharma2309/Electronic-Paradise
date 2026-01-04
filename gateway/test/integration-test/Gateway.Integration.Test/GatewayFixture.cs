using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Gateway.Integration.Test;

public class GatewayFixture : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    public HttpClient Client { get; }

    public GatewayFixture()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                // Override YARP config for testing with mocks
                conf.AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ReverseProxy:Clusters:authCluster:Destinations:auth1:Address", "http://dependency-mock:3001" },
                    { "ReverseProxy:Clusters:userCluster:Destinations:user1:Address", "http://dependency-mock:3001" },
                    { "ReverseProxy:Clusters:productCluster:Destinations:product1:Address", "http://dependency-mock:3001" },
                    { "ReverseProxy:Clusters:orderCluster:Destinations:order1:Address", "http://dependency-mock:3001" },
                    { "ReverseProxy:Clusters:paymentCluster:Destinations:payment1:Address", "http://dependency-mock:3001" }
                });
            });
        });

        Client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        var response = await Client.GetAsync("/api/health");
        response.EnsureSuccessStatusCode();
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}

