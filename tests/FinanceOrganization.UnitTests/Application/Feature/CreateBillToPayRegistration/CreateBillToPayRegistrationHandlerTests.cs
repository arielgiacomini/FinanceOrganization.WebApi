using Application.Feature;
using Application.Feature.BillToPayRegistration.CreateBillToPayRegistration;
using Application.Feature.CashReceivableLogic;
using Domain.Interfaces;
using FinanceOrganization.UnitTests.Application.Configs.Collections;
using FinanceOrganization.UnitTests.Application.Configs.Fixtures;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Moq;
using Xunit;

namespace FinanceOrganization.UnitTests.Application.Feature.CreateBillToPayRegistration
{
    [Collection(nameof(UnitTestCollection))]
    public class CreateBillToPayRegistrationHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<IBillToPayRegistrationRepository> _mockBillToPayRegistrationRepository;
        private readonly Mock<IBillToPayRepository> _mockBillToPayRepository;
        private readonly Mock<ILogger<CreateBillToPayRegistrationHandler>> _mockLogger;
        private readonly Mock<IAdjustCashReceivable> _mockAdjustCashReceivable;

        public CreateBillToPayRegistrationHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _mockBillToPayRegistrationRepository = new Mock<IBillToPayRegistrationRepository>();
            _mockBillToPayRepository = new Mock<IBillToPayRepository>();
            _mockLogger = new Mock<ILogger<CreateBillToPayRegistrationHandler>>();
            _mockAdjustCashReceivable = new Mock<IAdjustCashReceivable>();
        }

        [Fact]
        public async Task Handle_DeveExecutarCriacaoDeBillToPayRegistration_Sucesso()
        {
            // Setup

            _mockBillToPayRegistrationRepository
                .Setup(repo => repo.Save(_modelFixture.GetBillToPayRegistration()))
                .ReturnsAsync(1);

            // Action

            var handle =
                new CreateBillToPayRegistrationHandler(
                    _mockLogger.Object,
                    _mockBillToPayRegistrationRepository.Object,
                    _mockBillToPayRepository.Object,
                    _mockAdjustCashReceivable.Object);

            var input = _modelFixture.GetCreateBillToPayRegistrationInput();

            var result = await handle.Handle(input);

            // Assert

            Assert.Equal(OutputBaseDetails.OutputStatus.Success, result.Output.Status);
        }
    }
}