using Application.Feature;
using Application.Feature.FixedInvoice.SearchFixedInvoice;
using Domain.Interfaces;
using FinanceOrganization.UnitTests.Application.Configs.Collections;
using FinanceOrganization.UnitTests.Application.Configs.Fixtures;
using Moq;
using Xunit;

namespace FinanceOrganization.UnitTests.Application.Feature.SearchFixedInvoice
{
    [Collection(nameof(UnitTestCollection))]
    public class SearchFixedInvoiceHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<IFixedInvoiceRepository> _mockFixedInvoiceRepository;

        public SearchFixedInvoiceHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _mockFixedInvoiceRepository = new Mock<IFixedInvoiceRepository>();
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