using Xunit;

namespace ProductService.Integration.Test;

[CollectionDefinition("ProductServiceIntegration")]
public class IntegrationCollection : ICollectionFixture<ProductServiceFixture>
{
    // This class has no code, and is used to apply [CollectionDefinition] and all the
    // ICollectionFixture interfaces to our test classes.
}



