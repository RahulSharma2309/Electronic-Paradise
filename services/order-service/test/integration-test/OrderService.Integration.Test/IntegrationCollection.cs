using Xunit;

namespace OrderService.Integration.Test;

[CollectionDefinition("OrderServiceIntegration")]
public class IntegrationCollection : ICollectionFixture<OrderServiceFixture>
{
}



