using Xunit;

namespace PaymentService.Integration.Test;

[CollectionDefinition("PaymentServiceIntegration")]
public class IntegrationCollection : ICollectionFixture<PaymentServiceFixture>
{
}



