using Application.Feature.CashReceivable.AdjustCashReceivable;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.BillToPayRegistration.CreateBillToPayRegistration
{
    public class CreateBillToPayRegistrationHandler : ICreateBillToPayRegistrationHandler
    {
        private readonly ILogger<CreateBillToPayRegistrationHandler> _logger;
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;
        private readonly IBillToPayRepository _billToPayRepository;
        private readonly IAdjustCashReceivableHandler _adjustCashReceivable;

        public CreateBillToPayRegistrationHandler(ILogger<CreateBillToPayRegistrationHandler> logger,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository,
            IAdjustCashReceivableHandler adjustCashReceivable)
        {
            _logger = logger;
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
            _billToPayRepository = billToPayRepository;
            _adjustCashReceivable = adjustCashReceivable;
        }

        public async Task<CreateBillToPayRegistrationOutput> Handle(CreateBillToPayRegistrationInput input,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Está sendo criado a conta a pagar de nome: {Name}", input.Name);

            var validate = await CreateBillToPayRegistrationValidator.ValidateInput(input, _billToPayRegistrationRepository, _billToPayRepository);

            if (validate.Any())
            {
                _logger.LogWarning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new CreateBillToPayRegistrationOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var isSaved = await _billToPayRegistrationRepository.Save(MapInputBillToPayRegistrationToDomain(input));

            var output = new CreateBillToPayRegistrationOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
            };

            return output;
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
                LastChangeDate = input.LastChangeDate
            };
        }
    }
}