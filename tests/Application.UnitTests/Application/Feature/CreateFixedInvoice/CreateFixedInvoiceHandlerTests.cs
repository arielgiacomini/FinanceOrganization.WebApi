using Application.Feature;
using Application.Feature.FixedInvoice.CreateFixedInvoice;
using Domain.Interfaces;
using FinanceOrganization.UnitTests.Application.Configs.Collections;
using FinanceOrganization.UnitTests.Application.Configs.Fixtures;
using Microsoft.VisualBasic;
using Moq;
using Serilog;
using Xunit;

namespace FinanceOrganization.UnitTests.Application.Feature.CreateFixedInvoice
{
    [Collection(nameof(UnitTestCollection))]
    public class CreateFixedInvoiceHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<IFixedInvoiceRepository> _mockFixedInvoiceRepository;
        private readonly Mock<ILogger> _mockLogger;

        public CreateFixedInvoiceHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _mockFixedInvoiceRepository = new Mock<IFixedInvoiceRepository>();
            _mockLogger = new Mock<ILogger>();
        }

        [Fact]
        public async Task Handle_DeveExecutarCriacaoDeFixedInvoice_Sucesso()
        {
            // Setup

            _mockFixedInvoiceRepository
                .Setup(repo => repo.Save(_modelFixture.GetFixedInvoice()))
                .ReturnsAsync(1);

            // Action

            var handle =
                new CreateFixedInvoiceHandler(_mockLogger.Object, _mockFixedInvoiceRepository.Object);

            var input = _modelFixture.GetCreateFixedInvoiceInput();

            var result = await handle.Handle(input);

            // Assert

            Assert.Equal(OutputBaseDetails.OutputStatus.Success, result.Output.Status);
        }
    }
}