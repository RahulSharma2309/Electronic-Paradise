using Xunit;

namespace UserService.Integration.Test;

[CollectionDefinition("UserServiceIntegration")]
public class IntegrationCollection : ICollectionFixture<UserServiceFixture>
{
}



