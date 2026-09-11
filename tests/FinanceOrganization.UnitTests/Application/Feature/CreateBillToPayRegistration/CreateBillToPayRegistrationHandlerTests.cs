using Application.Feature;
using Application.Feature.BillToPayRegistration.CreateBillToPayRegistration;
using Application.Feature.CashReceivable.AdjustCashReceivable;
using Application.Feature.Payment.AdjustPayament;
using Domain.Entities;
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
        private readonly Mock<IAdjustCashReceivableHandler> _mockAdjustCashReceivable;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IPaymentAdjustmentHandler> _mockPaymentAdjustmentHandler;

        public CreateBillToPayRegistrationHandlerTests(ModelFixture modelFixture)
        {
            _modelFixture = modelFixture;
            _mockBillToPayRegistrationRepository = new Mock<IBillToPayRegistrationRepository>();
            _mockBillToPayRepository = new Mock<IBillToPayRepository>();
            _mockLogger = new Mock<ILogger<CreateBillToPayRegistrationHandler>>();
            _mockAdjustCashReceivable = new Mock<IAdjustCashReceivableHandler>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockPaymentAdjustmentHandler = new Mock<IPaymentAdjustmentHandler>();
        }

        private CreateBillToPayRegistrationHandler CreateHandler()
        {
            return new CreateBillToPayRegistrationHandler(
                _mockLogger.Object,
                _mockBillToPayRegistrationRepository.Object,
                _mockBillToPayRepository.Object,
                _mockAdjustCashReceivable.Object,
                _mockAccountRepository.Object,
                _mockPaymentAdjustmentHandler.Object);
        }

        [Fact]
        public async Task Handle_DeveExecutarCriacaoDeBillToPayRegistration_Sucesso()
        {
            // Setup

            _mockBillToPayRegistrationRepository
                .Setup(repo => repo.Save(_modelFixture.GetBillToPayRegistration()))
                .ReturnsAsync(1);

            // Action

            var handle = CreateHandler();

            var input = _modelFixture.GetCreateBillToPayRegistrationInput();

            var result = await handle.Handle(input);

            // Assert

            Assert.Equal(OutputBaseDetails.OutputStatus.Success, result.Output.Status);
        }

        [Fact]
        public async Task Handle_DeveCadastrarBillToPayDiretamente_QuandoIdBillToPayRegistrationInformado()
        {
            // Setup

            const int idParentRegistration = 10;

            var parentRegistration = _modelFixture.GetBillToPayRegistration();
            parentRegistration.Id = idParentRegistration;
            parentRegistration.Enabled = true;

            var account = new Account { Id = 1, Name = "Teste2" };

            _mockBillToPayRegistrationRepository
                .Setup(repo => repo.GetById(idParentRegistration))
                .ReturnsAsync(parentRegistration);

            _mockAccountRepository
                .Setup(repo => repo.GetAccountByName(account.Name!))
                .ReturnsAsync(account);

            _mockPaymentAdjustmentHandler
                .Setup(handler => handler.ConsideredPaid(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            IList<BillToPay>? savedBillsToPay = null;

            _mockBillToPayRepository
                .Setup(repo => repo.SaveRange(It.IsAny<IList<BillToPay>>()))
                .Callback<IList<BillToPay>>(billsToPay => savedBillsToPay = billsToPay)
                .ReturnsAsync(1);

            var input = _modelFixture.GetCreateBillToPayRegistrationInput();
            input.IdBillToPayRegistration = idParentRegistration;
            input.Account = account.Name;
            input.InitialMonthYear = "Março/2024";
            input.FynallyMonthYear = "Março/2024";

            // Action

            var handle = CreateHandler();

            var result = await handle.Handle(input);

            // Assert

            Assert.Equal(OutputBaseDetails.OutputStatus.Success, result.Output.Status);

            _mockBillToPayRepository.Verify(repo => repo.SaveRange(It.IsAny<IList<BillToPay>>()), Times.Once);

            // Não deve passar por CONTA_PAGAR_CADASTRO: cadastro direto vai só para CONTA_PAGAR.
            _mockBillToPayRegistrationRepository.Verify(repo => repo.Save(It.IsAny<BillToPayRegistration>()), Times.Never);

            Assert.NotNull(savedBillsToPay);
            Assert.Single(savedBillsToPay!);
            Assert.Equal("Março/2024", savedBillsToPay![0].YearMonth);
            Assert.Equal(idParentRegistration, savedBillsToPay![0].IdBillToPayRegistration);
        }

        [Fact]
        public async Task Handle_DeveAplicarRegraDeVencimentoDeCartaoDeCredito_QuandoIdBillToPayRegistrationInformado()
        {
            // Setup

            const int idParentRegistration = 20;
            const int dueDateCreditCard = 10;

            var parentRegistration = _modelFixture.GetBillToPayRegistration();
            parentRegistration.Id = idParentRegistration;
            parentRegistration.Enabled = true;

            var creditCardAccount = new Account
            {
                Id = 2,
                Name = "Cartão de Crédito",
                CardNumber = "1234",
                DueDate = dueDateCreditCard
            };

            _mockBillToPayRegistrationRepository
                .Setup(repo => repo.GetById(idParentRegistration))
                .ReturnsAsync(parentRegistration);

            _mockAccountRepository
                .Setup(repo => repo.GetAccountByName(creditCardAccount.Name!))
                .ReturnsAsync(creditCardAccount);

            _mockPaymentAdjustmentHandler
                .Setup(handler => handler.ConsideredPaid(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            IList<BillToPay>? savedBillsToPay = null;

            _mockBillToPayRepository
                .Setup(repo => repo.SaveRange(It.IsAny<IList<BillToPay>>()))
                .Callback<IList<BillToPay>>(billsToPay => savedBillsToPay = billsToPay)
                .ReturnsAsync(1);

            var input = _modelFixture.GetCreateBillToPayRegistrationInput();
            input.IdBillToPayRegistration = idParentRegistration;
            input.Account = creditCardAccount.Name;
            input.InitialMonthYear = "Março/2024";
            input.FynallyMonthYear = "Março/2024";
            input.BestPayDay = 5; // deve ser ignorado: para cartão de crédito prevalece o DueDate da conta.

            // Action

            var handle = CreateHandler();

            var result = await handle.Handle(input);

            // Assert

            Assert.Equal(OutputBaseDetails.OutputStatus.Success, result.Output.Status);

            Assert.NotNull(savedBillsToPay);
            Assert.Single(savedBillsToPay!);

            var billToPay = savedBillsToPay![0];

            // YearMonth (mês de referência) permanece o mês informado, mas o vencimento cai no mês seguinte,
            // no dia configurado na conta de cartão de crédito — mesma regra do CreateBillToPayEventHandler.
            Assert.Equal("Março/2024", billToPay.YearMonth);
            Assert.Equal(4, billToPay.DueDate.Month);
            Assert.Equal(2024, billToPay.DueDate.Year);
            Assert.Equal(dueDateCreditCard, billToPay.DueDate.Day);
        }

        [Fact]
        public async Task Handle_DeveAcionarAjusteDePagamento_QuandoIdBillToPayRegistrationInformadoParaCompraLivre()
        {
            // Setup

            const int idParentRegistration = 30;

            var parentRegistration = _modelFixture.GetBillToPayRegistration();
            parentRegistration.Id = idParentRegistration;
            parentRegistration.Enabled = true;

            var account = new Account { Id = 3, Name = "Teste2", ConsiderPaid = false };

            _mockBillToPayRegistrationRepository
                .Setup(repo => repo.GetById(idParentRegistration))
                .ReturnsAsync(parentRegistration);

            _mockAccountRepository
                .Setup(repo => repo.GetAccountByName(account.Name!))
                .ReturnsAsync(account);

            _mockPaymentAdjustmentHandler
                .Setup(handler => handler.ConsideredPaid(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockBillToPayRepository
                .Setup(repo => repo.SaveRange(It.IsAny<IList<BillToPay>>()))
                .ReturnsAsync(1);

            var input = _modelFixture.GetCreateBillToPayRegistrationInput();
            input.IdBillToPayRegistration = idParentRegistration;
            input.Account = account.Name;
            input.InitialMonthYear = "Março/2024";
            input.FynallyMonthYear = "Março/2024";
            input.RegistrationType = "Compra Livre";
            input.Frequence = "Livre";
            input.Category = "Alimentação";
            input.Value = 50;

            // Action

            var handle = CreateHandler();

            var result = await handle.Handle(input);

            // Assert

            Assert.Equal(OutputBaseDetails.OutputStatus.Success, result.Output.Status);

            // Mesma regra da rotina em background: ao cadastrar direto uma Compra Livre, deve acionar o
            // PaymentAdjustmentHandler.Handle (que faz o desconto contra a conta fixa da mesma categoria/mês
            // quando aplicável), com os dados equivalentes ao que CreatePaymentAdjustment geraria.
            _mockPaymentAdjustmentHandler.Verify(handler => handler.Handle(
                It.Is<PaymentAdjustmentInput>(paymentInput =>
                    paymentInput.RegistrationType == "Compra Livre" &&
                    paymentInput.Frequence == "Livre" &&
                    paymentInput.QuantityMonthsAdd == 0 &&
                    paymentInput.Category == "Alimentação" &&
                    paymentInput.YearMonth == "Março/2024" &&
                    paymentInput.Value == 50),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
