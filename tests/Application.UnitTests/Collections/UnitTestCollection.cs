using Application.UnitTests.Fixtures;
using Xunit;

namespace Application.UnitTests.Collections
{
    [CollectionDefinition(nameof(UnitTestCollection))]
    public class UnitTestCollection : ICollectionFixture<ModelFixture>
    {

    }
}