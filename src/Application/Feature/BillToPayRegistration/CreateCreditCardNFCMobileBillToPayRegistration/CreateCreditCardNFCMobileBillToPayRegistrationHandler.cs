using Domain.Interfaces;
using Domain.Utils;
using Serilog;

namespace Application.Feature.BillToPayRegistration.CreateCreditCardNFCMobileBillToPayRegistration
{
    public class CreateCreditCardNFCMobileBillToPayRegistrationHandler : ICreateCreditCardNFCMobileBillToPayRegistrationHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;
        private readonly IBillToPayRepository _billToPayRepository;
        private readonly IAccountRepository _accountRepository;

        public CreateCreditCardNFCMobileBillToPayRegistrationHandler(ILogger logger,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository,
            IAccountRepository accountRepository)
        {
            _logger = logger;
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
            _billToPayRepository = billToPayRepository;
            _accountRepository = accountRepository;
        }

        public async Task<CreateCreditCardNFCMobileBillToPayRegistrationOutput> Handle(CreateCreditCardNFCMobileBillToPayRegistrationInput input,
            CancellationToken cancellationToken = default)
        {
            _logger.Information("Está sendo criado a conta a pagar de nome: {Name}", input.Name);

            var validate = await CreateCreditCardNFCMobileBillToPayRegistrationValidator
                .ValidateInput(input, _billToPayRegistrationRepository, _billToPayRepository, _accountRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new CreateCreditCardNFCMobileBillToPayRegistrationOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var accountByInput = await _accountRepository.GetAccountByName(input.Account!);

            var isSaved = await _billToPayRegistrationRepository
                .Save(MapInputBillToPayRegistrationToDomain(input, accountByInput!));

            var output = new CreateCreditCardNFCMobileBillToPayRegistrationOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
            };

            return await Task.FromResult(output);
        }

        private static Domain.Entities.BillToPayRegistration MapInputBillToPayRegistrationToDomain(
            CreateCreditCardNFCMobileBillToPayRegistrationInput input, Domain.Entities.Account account)
        {
            return new Domain.Entities.BillToPayRegistration
            {
                Name = input.Name,
                Category = input.Category,
                Account = input.Account,
                Value = input.Value,
                PurchaseDate = input.PurchaseDate,
                BestPayDay = account.DueDate == null ? DateTime.Today.ToLocalTime().Day : account.DueDate.Value,
                InitialMonthYear = DateServiceUtils.GetYearMonthPortugueseByDateTime(DateTime.Now.ToLocalTime()),
                FynallyMonthYear = DateServiceUtils.GetYearMonthPortugueseByDateTime(DateTime.Now.ToLocalTime()),
                Frequence = input.Frequence,
                RegistrationType = input.RegistrationType,
                AdditionalMessage = input.AdditionalMessage,
                CreationDate = DateTime.Now.ToLocalTime(),
                LastChangeDate = null
            };
        }
    }
}