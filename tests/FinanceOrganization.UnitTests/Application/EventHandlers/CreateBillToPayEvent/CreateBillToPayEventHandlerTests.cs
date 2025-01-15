using Application.EventHandlers.CreateBillToPayEvent;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using FinanceOrganization.UnitTests.Application.Configs.Fixtures;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using Xunit;

namespace FinanceOrganization.UnitTests.Application.EventHandlers.CreateBillToPayEvent
{
    public class CreateBillToPayEventHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<ILogger> _dummyLogger;
        private readonly Mock<IOptions<BillToPayOptions>> _mockOptions;
        private readonly Mock<IBillToPayRegistrationRepository> _mockBillToPayRegistrationRepository;
        private readonly Mock<IBillToPayRepository> _mockWalletToPayRepository;
        private readonly Mock<IAccountRepository> _mockAccountRepository;

        public CreateBillToPayEventHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _dummyLogger = new Mock<ILogger>();
            _mockOptions = new Mock<IOptions<BillToPayOptions>>();
            _mockBillToPayRegistrationRepository = new Mock<IBillToPayRegistrationRepository>();
            _mockWalletToPayRepository = new Mock<IBillToPayRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();

            _mockOptions
                .Setup(options => options.Value)
                .Returns(_modelFixture.GetBillToPayOptions());
        }

        [Fact]
        public void Handle_DeveExecutarHandleComBillToPayPreenchido_Sucesso()
        {
            // Setup
            _mockBillToPayRegistrationRepository
                .Setup(billToPayRegistration => billToPayRegistration.GetAll())
                .ReturnsAsync(_modelFixture.GetListBillToPayRegistration());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.GetBillToPayByBillToPayRegistrationId(It.IsAny<int>()))
                .ReturnsAsync(_modelFixture.GetListBillToPay());

            _mockBillToPayRegistrationRepository
                .Setup(billToPayRegistration => billToPayRegistration
                .GetOnlyOldRecordsAndParticipants(-1, "Conta/Fatura Fixa"))
                .ReturnsAsync(_modelFixture.GetListBillToPayRegistration());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.SaveRange(It.IsAny<IList<BillToPay>>()))
                .ReturnsAsync(It.IsAny<int>());

            // Action

            var handler = new CreateBillToPayEventHandler(
                _dummyLogger.Object,
                _mockOptions.Object,
                _mockBillToPayRegistrationRepository.Object,
                _mockWalletToPayRepository.Object,
                _mockAccountRepository.Object);

            var input = new CreateBillToPayEventInput
            {
                DateExecution = new DateTime(2023, 1, 1, 0, 0, 0, kind: DateTimeKind.Local)
            };

            _ = handler.Handle(input);

            // Assert

            _mockWalletToPayRepository
                .Verify(wallet => wallet.SaveRange(It.IsAny<IList<BillToPay>>()), Times.Once());
        }

        [Fact]
        public void Handle_DeveRetornarQuantidadeMesesAddMenorIgualZeroBillToPay_Sucesso()
        {
            // Setup
            _mockBillToPayRegistrationRepository
                .Setup(billToPayRegistration => billToPayRegistration.GetAll())
                .ReturnsAsync(_modelFixture.GetListBillToPayRegistration());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.GetBillToPayByBillToPayRegistrationId(It.IsAny<int>()))
                .ReturnsAsync(_modelFixture.GetListBillToPay());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.SaveRange(It.IsAny<IList<BillToPay>>()))
                .ReturnsAsync(It.IsAny<int>());

            var options = _modelFixture.GetBillToPayOptions();
            options.HowManyMonthForward = 0;

            _mockOptions
                .Setup(options => options.Value)
                .Returns(options);

            // Action

            var handler = new CreateBillToPayEventHandler(
                _dummyLogger.Object,
                _mockOptions.Object,
                _mockBillToPayRegistrationRepository.Object,
                _mockWalletToPayRepository.Object, _mockAccountRepository.Object);

            var input = new CreateBillToPayEventInput
            {
                DateExecution = new DateTime(2023, 1, 1, 0, 0, 0, kind: DateTimeKind.Local)
            };

            _ = handler.Handle(input);

            // Assert

            _mockWalletToPayRepository
                .Verify(wallet => wallet.SaveRange(It.IsAny<IList<BillToPay>>()), Times.Never());
        }

        [Theory]
        [InlineData(25, 24, 0)]
        [InlineData(24, 24, 0)]
        [InlineData(20, 24, 4)]
        public void Handle_DeveValidarQuantidadeMesesAdicionar_Sucesso(
            int totalMonths, int howManyMonthForward, int expected)
        {
            var result = CreateBillToPayEventHandler
                .GetMonthsAdd(totalMonths, howManyMonthForward);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Handle_DeveExecutarHandleComBillToPayRegistrationPreenchido_Sucesso()
        {
            // Setup
            _mockBillToPayRegistrationRepository
                .Setup(billToPayRegistration => billToPayRegistration.GetAll())
                .ReturnsAsync(_modelFixture.GetListBillToPayRegistration());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.GetBillToPayByBillToPayRegistrationId(It.IsAny<int>()))
                .ReturnsAsync(() => null!);

            _mockBillToPayRegistrationRepository
                .Setup(billToPayRegistration => billToPayRegistration
                .GetOnlyOldRecordsAndParticipants(-1, "Conta/Fatura Fixa"))
                .ReturnsAsync(_modelFixture.GetListBillToPayRegistration());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.SaveRange(It.IsAny<IList<BillToPay>>()))
                .ReturnsAsync(It.IsAny<int>());

            // Action

            var handler = new CreateBillToPayEventHandler(
                _dummyLogger.Object,
                _mockOptions.Object,
                _mockBillToPayRegistrationRepository.Object,
                _mockWalletToPayRepository.Object,
                _mockAccountRepository.Object);

            var input = new CreateBillToPayEventInput
            {
                DateExecution = new DateTime(2023, 1, 1, 0, 0, 0, kind: DateTimeKind.Local)
            };

            _ = handler.Handle(input);

            // Assert

            _mockWalletToPayRepository
                .Verify(wallet => wallet.SaveRange(It.IsAny<IList<BillToPay>>()), Times.Once());
        }

        [Fact]
        public void Handle_DeveRetornarQuantidadeMesesAddMenorIgualZeroBillToPayRegistration_Sucesso()
        {
            // Setup
            _mockBillToPayRegistrationRepository
                .Setup(billToPayRegistration => billToPayRegistration.GetAll())
                .ReturnsAsync(_modelFixture.GetListBillToPayRegistration());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.GetBillToPayByBillToPayRegistrationId(It.IsAny<int>()))
                .ReturnsAsync(() => null!);

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.SaveRange(It.IsAny<IList<BillToPay>>()))
                .ReturnsAsync(It.IsAny<int>());

            var options = _modelFixture.GetBillToPayOptions();
            options.HowManyMonthForward = 0;

            _mockOptions
                .Setup(options => options.Value)
                .Returns(options);

            // Action

            var handler = new CreateBillToPayEventHandler(
                _dummyLogger.Object,
                _mockOptions.Object,
                _mockBillToPayRegistrationRepository.Object,
                _mockWalletToPayRepository.Object, _mockAccountRepository.Object);

            var input = new CreateBillToPayEventInput
            {
                DateExecution = new DateTime(2023, 1, 1, 0, 0, 0, kind: DateTimeKind.Local)
            };

            _ = handler.Handle(input);

            // Assert

            _mockWalletToPayRepository
                .Verify(wallet => wallet.SaveRange(It.IsAny<IList<BillToPay>>()), Times.Never());
        }
    }
}