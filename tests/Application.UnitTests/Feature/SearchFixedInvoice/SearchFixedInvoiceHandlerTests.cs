using Application.Feature;
using Application.Feature.SearchFixedInvoice;
using Application.UnitTests.Configs.Collections;
using Application.UnitTests.Configs.Fixtures;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace Application.UnitTests.Feature.SearchFixedInvoice
{
    [Collection(nameof(UnitTestCollection))]
    public class SearchFixedInvoiceHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<IFixedInvoiceRepository> _mockFixedInvoiceRepository;
        private readonly Mock<OutputBaseDetails> _mockOutputBaseDetails;

        public SearchFixedInvoiceHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _mockFixedInvoiceRepository = new Mock<IFixedInvoiceRepository>();
            _mockOutputBaseDetails = new Mock<OutputBaseDetails>();
        }

        [Fact]
        public async Task Hanle_DeveExecutarBuscaDeFixedInvoice_Sucesso()
        {
            // Setup

            _mockFixedInvoiceRepository
                .Setup(repo => repo.GetByAll())
                .ReturnsAsync(_modelFixture.GetListFixedInvoice());

            // Action

            var handle =
                new SearchFixedInvoiceHandler(_mockFixedInvoiceRepository.Object);

            var result = await handle.Handle();

            Assert.Equal(OutputBaseDetails.OutputStatus.Success, result.Output.Status);
        }
    }
}