using FinanceOrganization.UnitTests.Configs.Fixtures;
using Xunit;

namespace FinanceOrganization.UnitTests.Configs.Collections
{
    [CollectionDefinition(nameof(UnitTestCollection))]
    public class UnitTestCollection : ICollectionFixture<ModelFixture>
    {

    }
}