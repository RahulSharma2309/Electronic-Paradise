using Xunit;

namespace Gateway.Integration.Test;

[CollectionDefinition("GatewayIntegration")]
public class IntegrationCollection : ICollectionFixture<GatewayFixture>
{
}

