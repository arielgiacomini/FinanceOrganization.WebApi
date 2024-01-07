using FinanceOrganization.UnitTests.Application.Configs.Fixtures;
using Xunit;

namespace FinanceOrganization.UnitTests.Application.Configs.Collections
{
    [CollectionDefinition(nameof(UnitTestCollection))]
    public class UnitTestCollection : ICollectionFixture<ModelFixture>
    {

    }
}