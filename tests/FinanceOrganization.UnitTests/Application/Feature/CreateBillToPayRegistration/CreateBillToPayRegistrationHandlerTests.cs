using Application.Feature;
using Application.Feature.BillToPayRegistration.CreateBillToPayRegistration;
using Domain.Interfaces;
using FinanceOrganization.UnitTests.Application.Configs.Collections;
using FinanceOrganization.UnitTests.Application.Configs.Fixtures;
using Microsoft.VisualBasic;
using Moq;
using Serilog;
using Xunit;

namespace FinanceOrganization.UnitTests.Application.Feature.CreateBillToPayRegistration
{
    [Collection(nameof(UnitTestCollection))]
    public class CreateBillToPayRegistrationHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<IBillToPayRegistrationRepository> _mockBillToPayRegistrationRepository;
        private readonly Mock<IBillToPayRepository> _mockBillToPayRepository;
        private readonly Mock<ILogger> _mockLogger;

        public CreateBillToPayRegistrationHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _mockBillToPayRegistrationRepository = new Mock<IBillToPayRegistrationRepository>();
            _mockBillToPayRepository = new Mock<IBillToPayRepository>();
            _mockLogger = new Mock<ILogger>();
        }

        [Fact]
        public async Task Handle_DeveExecutarCriacaoDeFixedInvoice_Sucesso()
        {
            // Setup

            _mockBillToPayRegistrationRepository
                .Setup(repo => repo.Save(_modelFixture.GetFixedInvoice()))
                .ReturnsAsync(1);

            // Action

            var handle =
                new CreateBillToPayRegistrationHandler(_mockLogger.Object, _mockBillToPayRegistrationRepository.Object, _mockBillToPayRepository.Object);

            var input = _modelFixture.GetCreateFixedInvoiceInput();

            var result = await handle.Handle(input);

            // Assert

            Assert.Equal(OutputBaseDetails.OutputStatus.Success, result.Output.Status);
        }
    }
}