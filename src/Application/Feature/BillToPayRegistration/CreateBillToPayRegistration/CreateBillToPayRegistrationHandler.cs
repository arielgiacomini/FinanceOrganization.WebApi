using Application.EventHandlers.CreateBillToPayEvent;
using Application.Feature.CashReceivable.AdjustCashReceivable;
using Application.Feature.Payment.AdjustPayament;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Utils;
using Microsoft.Extensions.Logging;

namespace Application.Feature.BillToPayRegistration.CreateBillToPayRegistration
{
    public class CreateBillToPayRegistrationHandler : ICreateBillToPayRegistrationHandler
    {
        private readonly ILogger<CreateBillToPayRegistrationHandler> _logger;
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;
        private readonly IBillToPayRepository _billToPayRepository;
        private readonly IAdjustCashReceivableHandler _adjustCashReceivable;
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentAdjustmentHandler _paymentAdjustmentHandler;

        public CreateBillToPayRegistrationHandler(ILogger<CreateBillToPayRegistrationHandler> logger,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository,
            IAdjustCashReceivableHandler adjustCashReceivable,
            IAccountRepository accountRepository,
            IPaymentAdjustmentHandler paymentAdjustmentHandler)
        {
            _logger = logger;
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
            _billToPayRepository = billToPayRepository;
            _adjustCashReceivable = adjustCashReceivable;
            _accountRepository = accountRepository;
            _paymentAdjustmentHandler = paymentAdjustmentHandler;
        }

        public async Task<CreateBillToPayRegistrationOutput> Handle(CreateBillToPayRegistrationInput input,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Está sendo criado a conta a pagar de nome: {Name}", input.Name);

            var validate = await CreateBillToPayRegistrationValidator.ValidateInput(input, _billToPayRegistrationRepository, _billToPayRepository, _accountRepository);

            if (validate.Any())
            {
                _logger.LogWarning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new CreateBillToPayRegistrationOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            int isSaved;

            if (input.IdBillToPayRegistration.HasValue)
            {
                // Associação a uma conta a pagar já existente: não passa por CONTA_PAGAR_CADASTRO nem pela
                // rotina em background CreateBillToPayEventHandler — cadastra direto na tabela oficial CONTA_PAGAR.
                await CreateDirectBillToPay(input, cancellationToken);
                isSaved = 1;
            }
            else
            {
                isSaved = await _billToPayRegistrationRepository.Save(MapInputBillToPayRegistrationToDomain(input));
            }

            var output = new CreateBillToPayRegistrationOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
            };

            return output;
        }

        /// <summary>
        /// Cadastra diretamente o BillToPay (CONTA_PAGAR) referente ao mês informado, associado a uma conta a
        /// pagar já existente via IdBillToPayRegistration — usada para preencher rapidamente um mês que ficou
        /// faltando, sem passar por CONTA_PAGAR_CADASTRO. É uma amarração genérica (não depende da lógica de
        /// contas fixas recorrentes), mas mantém as mesmas regras da rotina em background
        /// (CreateBillToPayEventHandler): vencimento de Cartão de Crédito (dia da conta, mês seguinte ao de
        /// referência) e o ajuste/desconto de Compra Livre contra a conta fixa correspondente, quando aplicável.
        /// </summary>
        private async Task CreateDirectBillToPay(CreateBillToPayRegistrationInput input, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountByName(input.Account!);
            var isCreditCard = account!.IsCreditCard;

            var bestPayDay = isCreditCard && account.DueDate.HasValue
                ? account.DueDate.Value
                : input.BestPayDay ?? input.PurchaseDate!.Value.Day;

            var initialMonthDate = DateServiceUtils.GetDateTimeByYearMonthBrazilian(input.InitialMonthYear)!.Value;
            var dayInMonth = Math.Min(bestPayDay, DateTime.DaysInMonth(initialMonthDate.Year, initialMonthDate.Month));
            var targetDate = new DateTime(initialMonthDate.Year, initialMonthDate.Month, dayInMonth, 0, 0, 0, DateTimeKind.Utc);

            // addFirstNextMonth desloca o vencimento para o mês seguinte ao de referência, mesma regra da
            // rotina em background para Cartão de Crédito (o YearMonth/mês de referência não é deslocado).
            var nextMonthYearToRegister = DateServiceUtils.GetNextYearMonthAndDateTime(
                targetDate, 0, null, currentMonth: true, addFirstNextMonth: isCreditCard);

            var singleMonth = nextMonthYearToRegister.First();

            var consideredPaid = await _paymentAdjustmentHandler
                .ConsideredPaid(input.Account!, input.RegistrationType!);

            var transientRegistration = new Domain.Entities.BillToPayRegistration
            {
                Id = input.IdBillToPayRegistration!.Value,
                Name = input.Name,
                Account = input.Account,
                Category = input.Category,
                Value = input.Value,
                Frequence = input.Frequence,
                RegistrationType = input.RegistrationType,
                AdditionalMessage = input.AdditionalMessage,
                Country = input.Country
            };

            var newBillToPay = CreateBillToPayEventHandler.MapBillToPay(
                null, transientRegistration, consideredPaid, singleMonth.Value, singleMonth.Key, input.PurchaseDate);

            // Se for uma Compra Livre, desconta o valor da conta fixa correspondente na mesma categoria/mês
            // (mesma regra aplicada pela rotina em background via PaymentAdjustmentHandler.Handle).
            var paymentAdjustment = CreateBillToPayEventHandler.CreatePaymentAdjustment(
                transientRegistration, 0, account, nextMonthYearToRegister);

            await _paymentAdjustmentHandler.Handle(paymentAdjustment, cancellationToken);

            await _billToPayRepository.SaveRange(new List<Domain.Entities.BillToPay> { newBillToPay });
        }

        private static Domain.Entities.BillToPayRegistration MapInputBillToPayRegistrationToDomain(CreateBillToPayRegistrationInput input)
        {
            return new Domain.Entities.BillToPayRegistration
            {
                Name = input.Name,
                Category = input.Category,
                Account = input.Account,
                Value = input.Value,
                PurchaseDate = input.PurchaseDate,
                BestPayDay = input.BestPayDay ?? input.PurchaseDate!.Value.Day,
                InitialMonthYear = input.InitialMonthYear,
                FynallyMonthYear = input.FynallyMonthYear,
                Frequence = input.Frequence,
                RegistrationType = input.RegistrationType,
                AdditionalMessage = input.AdditionalMessage,
                CreationDate = input.CreationDate,
                LastChangeDate = input.LastChangeDate,
                Country = input.Country
            };
        }
    }
}