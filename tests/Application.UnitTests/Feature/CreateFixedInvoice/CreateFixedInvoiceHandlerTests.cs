using Application.Feature;
using Application.Feature.FixedInvoice.CreateFixedInvoice;
using Application.UnitTests.Configs.Collections;
using Application.UnitTests.Configs.Fixtures;
using Domain.Interfaces;
using Microsoft.VisualBasic;
using Moq;
using Xunit;
using static Application.Feature.OutputBaseDetails;

namespace Application.UnitTests.Feature.CreateFixedInvoice
{
    [Collection(nameof(UnitTestCollection))]
    public class CreateFixedInvoiceHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<IFixedInvoiceRepository> _mockFixedInvoiceRepository;

        public CreateFixedInvoiceHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _mockFixedInvoiceRepository = new Mock<IFixedInvoiceRepository>();
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
                new CreateFixedInvoiceHandler(_mockFixedInvoiceRepository.Object);

            var input = _modelFixture.GetCreateFixedInvoiceInput();

            var result = await handle.Handle(input);

            // Assert

            Assert.Equal<OutputStatus>(result.Output.Status,
                OutputBaseDetails.OutputStatus.Success);
        }
    }
}