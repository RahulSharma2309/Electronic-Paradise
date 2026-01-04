using Xunit;

namespace AuthService.Integration.Test;

[CollectionDefinition("AuthServiceIntegration")]
public class IntegrationCollection : ICollectionFixture<AuthServiceFixture>
{
}



