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
                await CreateDirectBillToPay(input);
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
        /// faltando, sem passar por CONTA_PAGAR_CADASTRO. É uma amarração genérica, válida para qualquer tipo
        /// de conta/frequência (não depende da lógica de contas fixas recorrentes ou de cartão de crédito).
        /// </summary>
        private async Task CreateDirectBillToPay(CreateBillToPayRegistrationInput input)
        {
            var bestPayDay = input.BestPayDay ?? input.PurchaseDate!.Value.Day;

            var initialMonthDate = DateServiceUtils.GetDateTimeByYearMonthBrazilian(input.InitialMonthYear)!.Value;
            var dayInMonth = Math.Min(bestPayDay, DateTime.DaysInMonth(initialMonthDate.Year, initialMonthDate.Month));
            var dueDate = new DateTime(initialMonthDate.Year, initialMonthDate.Month, dayInMonth, 0, 0, 0, DateTimeKind.Utc);

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
                null, transientRegistration, consideredPaid, dueDate, input.InitialMonthYear!, input.PurchaseDate);

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