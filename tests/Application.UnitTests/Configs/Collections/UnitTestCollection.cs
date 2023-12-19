using Application.UnitTests.Configs.Fixtures;
using Xunit;

namespace Application.UnitTests.Configs.Collections
{
    [CollectionDefinition(nameof(UnitTestCollection))]
    public class UnitTestCollection : ICollectionFixture<ModelFixture>
    {

    }
}