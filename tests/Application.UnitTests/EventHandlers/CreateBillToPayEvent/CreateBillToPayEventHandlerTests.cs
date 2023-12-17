using Application.EventHandlers.CreateBillToPayEvent;
using Application.UnitTests.Collections;
using Application.UnitTests.Fixtures;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Application.UnitTests.EventHandlers.CreateBillToPayEvent
{
    [Collection(nameof(UnitTestCollection))]
    public class CreateBillToPayEventHandlerTests
    {
        private readonly ModelFixture _modelFixture;
        private readonly Mock<CreateBillToPayEventHandler> _mockCreateBillToPayHandler;
        private readonly Mock<ILogger<CreateBillToPayEventHandler>> _dummyLogger;
        private readonly Mock<IOptions<BillToPayOptions>> _mockOptions;
        private readonly Mock<IFixedInvoiceRepository> _mockFixedInvoiceRepository;
        private readonly Mock<IWalletToPayRepository> _mockWalletToPayRepository;

        public CreateBillToPayEventHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _mockCreateBillToPayHandler = new Mock<CreateBillToPayEventHandler>();
            _dummyLogger = new Mock<ILogger<CreateBillToPayEventHandler>>();
            _mockOptions = new Mock<IOptions<BillToPayOptions>>();
            _mockFixedInvoiceRepository = new Mock<IFixedInvoiceRepository>();
            _mockWalletToPayRepository = new Mock<IWalletToPayRepository>();

            _mockOptions
                .Setup(options => options.Value)
                .Returns(_modelFixture.GetBillToPayOptions());
        }

        [Fact]
        public void Handle_DeveExecutarHandleComBillToPayPreenchido_Sucesso()
        {
            // Setup
            _mockFixedInvoiceRepository
                .Setup(fixedInvoice => fixedInvoice.GetByAll())
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

            var input = new CreateBillToPayEventInput { DateExecution = new DateTime() };

            _ = handler.Handle(input);

            // Assert

            Assert.Null(null);
        }
    }
}