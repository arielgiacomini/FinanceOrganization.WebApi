using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.Account.CreateAccount
{
    public class CreateAccountHandler : ICreateAccountHandler
    {
        private readonly ILogger<CreateAccountHandler> _logger;
        private readonly IAccountRepository _accountRepository;

        public CreateAccountHandler(ILogger<CreateAccountHandler> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<CreateAccountOutput> Handle(CreateAccountInput input,
           CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Está sendo criado a conta a pagar de nome: {Name}", "");

            var validate = await CreateAccountValidator.ValidateInput(input, _accountRepository);

            if (validate.Any())
            {
                _logger.LogWarning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new CreateAccountOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var isSaved = await _accountRepository.Save(MapAccountInputToAccountDomain(input));

            var output = new CreateAccountOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
            };

            return output;
        }

        private static Domain.Entities.Account MapAccountInputToAccountDomain(CreateAccountInput input)
        {
            return new Domain.Entities.Account
            {
                Name = input.Name,
                DueDate = input.DueDate,
                ClosingDay = input.ClosingDay,
                ConsiderPaid = input.ConsiderPaid,
                AccountAgency = input.AccountAgency,
                AccountNumber = input.AccountNumber,
                AccountDigit = input.AccountDigit,
                CardNumber = input.CardNumber,
                CommissionPercentage = input.CommissionPercentage,
                Enable = input.Enable,
                CreationDate = DateTime.Now,
                LastChangeDate = null
            };
        }
    }
}