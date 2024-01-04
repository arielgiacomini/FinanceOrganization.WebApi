using Application.EventHandlers.CreateBillToPayEvent;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using FinanceOrganization.UnitTests.Application.Configs.Collections;
using FinanceOrganization.UnitTests.Application.Configs.Fixtures;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using Xunit;

namespace FinanceOrganization.UnitTests.Application.EventHandlers.CreateBillToPayEvent
{
    [Collection(nameof(UnitTestCollection))]
    public class CreateBillToPayEventHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<ILogger> _dummyLogger;
        private readonly Mock<IOptions<BillToPayOptions>> _mockOptions;
        private readonly Mock<IFixedInvoiceRepository> _mockFixedInvoiceRepository;
        private readonly Mock<IBillToPayRepository> _mockWalletToPayRepository;

        public CreateBillToPayEventHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _dummyLogger = new Mock<ILogger>();
            _mockOptions = new Mock<IOptions<BillToPayOptions>>();
            _mockFixedInvoiceRepository = new Mock<IFixedInvoiceRepository>();
            _mockWalletToPayRepository = new Mock<IBillToPayRepository>();

            _mockOptions
                .Setup(options => options.Value)
                .Returns(_modelFixture.GetBillToPayOptions());
        }

        [Fact]
        public void Handle_DeveExecutarHandleComBillToPayPreenchido_Sucesso()
        {
            // Setup
            _mockFixedInvoiceRepository
                .Setup(fixedInvoice => fixedInvoice.GetAll())
                .ReturnsAsync(_modelFixture.GetListFixedInvoice());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.GetBillToPayByFixedInvoiceId(It.IsAny<int>()))
                .ReturnsAsync(_modelFixture.GetListBillToPay());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.Save(It.IsAny<IList<BillToPay>>()))
                .ReturnsAsync(It.IsAny<int>());

            // Action

            var handler = new CreateBillToPayEventHandler(
                _dummyLogger.Object,
                _mockOptions.Object,
                _mockFixedInvoiceRepository.Object,
                _mockWalletToPayRepository.Object);

            var input = new CreateBillToPayEventInput
            {
                DateExecution = new DateTime(2023, 1, 1, 0, 0, 0, kind: DateTimeKind.Local)
            };

            _ = handler.Handle(input);

            // Assert

            _mockWalletToPayRepository
                .Verify(wallet => wallet.Save(It.IsAny<IList<BillToPay>>()), Times.Once());
        }

        [Fact]
        public void Handle_DeveRetornarQuantidadeMesesAddMenorIgualZeroBillToPay_Sucesso()
        {
            // Setup
            _mockFixedInvoiceRepository
                .Setup(fixedInvoice => fixedInvoice.GetAll())
                .ReturnsAsync(_modelFixture.GetListFixedInvoice());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.GetBillToPayByFixedInvoiceId(It.IsAny<int>()))
                .ReturnsAsync(_modelFixture.GetListBillToPay());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.Save(It.IsAny<IList<BillToPay>>()))
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
                _mockFixedInvoiceRepository.Object,
                _mockWalletToPayRepository.Object);

            var input = new CreateBillToPayEventInput
            {
                DateExecution = new DateTime(2023, 1, 1, 0, 0, 0, kind: DateTimeKind.Local)
            };

            _ = handler.Handle(input);

            // Assert

            _mockWalletToPayRepository
                .Verify(wallet => wallet.Save(It.IsAny<IList<BillToPay>>()), Times.Never());
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
        public void Handle_DeveExecutarHandleComFixedInvoicePreenchido_Sucesso()
        {
            // Setup
            _mockFixedInvoiceRepository
                .Setup(fixedInvoice => fixedInvoice.GetAll())
                .ReturnsAsync(_modelFixture.GetListFixedInvoice());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.GetBillToPayByFixedInvoiceId(It.IsAny<int>()))
                .ReturnsAsync(() => null!);

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.Save(It.IsAny<IList<BillToPay>>()))
                .ReturnsAsync(It.IsAny<int>());

            // Action

            var handler = new CreateBillToPayEventHandler(
                _dummyLogger.Object,
                _mockOptions.Object,
                _mockFixedInvoiceRepository.Object,
                _mockWalletToPayRepository.Object);

            var input = new CreateBillToPayEventInput
            {
                DateExecution = new DateTime(2023, 1, 1, 0, 0, 0, kind: DateTimeKind.Local)
            };

            _ = handler.Handle(input);

            // Assert

            _mockWalletToPayRepository
                .Verify(wallet => wallet.Save(It.IsAny<IList<BillToPay>>()), Times.Once());
        }

        [Fact]
        public void Handle_DeveRetornarQuantidadeMesesAddMenorIgualZeroFixedInvoice_Sucesso()
        {
            // Setup
            _mockFixedInvoiceRepository
                .Setup(fixedInvoice => fixedInvoice.GetAll())
                .ReturnsAsync(_modelFixture.GetListFixedInvoice());

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.GetBillToPayByFixedInvoiceId(It.IsAny<int>()))
                .ReturnsAsync(() => null!);

            _mockWalletToPayRepository
                .Setup(walletToPay => walletToPay.Save(It.IsAny<IList<BillToPay>>()))
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
                _mockFixedInvoiceRepository.Object,
                _mockWalletToPayRepository.Object);

            var input = new CreateBillToPayEventInput
            {
                DateExecution = new DateTime(2023, 1, 1, 0, 0, 0, kind: DateTimeKind.Local)
            };

            _ = handler.Handle(input);

            // Assert

            _mockWalletToPayRepository
                .Verify(wallet => wallet.Save(It.IsAny<IList<BillToPay>>()), Times.Never());
        }
    }
}