using Domain.Interfaces;

namespace Application.Feature.Account.EditAccount
{
    public class EditAccountHandler : IEditAccountHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly IAccountRepository _accountRepository;

        public EditAccountHandler(Serilog.ILogger logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<EditAccountOutput> Handle(EditAccountInput input, CancellationToken cancellationToken = default)
        {
            _logger.Information("Está sendo editada a conta de id: {Id}", input.Id);

            var validate = await EditAccountValidator.ValidateInput(input, _accountRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new EditAccountOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var accountExists = await _accountRepository.GetById(input.Id);

            if (accountExists == null)
            {
                _logger.Warning("Conta não encontrada para o id: {Id}", input.Id);

                var outputNotFound = new EditAccountOutput
                {
                    Output = OutputBaseDetails.Error("Conta não encontrada.", new Dictionary<string, string> { { "Id", "Conta não encontrada." } })
                };

                return outputNotFound;
            }

            var isSaved = await _accountRepository.Edit(MapInputAccountToDomain(input, accountExists));

            if (input.Colors != null)
            {
                await SaveOrEditColor(input.Id, input.Colors);
            }

            var output = new EditAccountOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Alteração realizada com sucesso.", new object(), 1)
            };

            return output;
        }

        private async Task SaveOrEditColor(int accountId, AccountColorInput colorInput)
        {
            var existingColor = await _accountRepository.GetAccountColorByAccountId(accountId);

            if (existingColor == null)
            {
                await _accountRepository.SaveAccountColor(new Domain.Entities.AccountColor
                {
                    AccountId = accountId,
                    BackgroundColorHexadecimal = colorInput.BackgroundColorHexadecimal,
                    FonteColorHexadecimal = colorInput.FonteColorHexadecimal,
                    Enable = true,
                    CreationDate = DateTime.Now,
                    LastChangeDate = null
                });

                return;
            }

            existingColor.BackgroundColorHexadecimal = colorInput.BackgroundColorHexadecimal;
            existingColor.FonteColorHexadecimal = colorInput.FonteColorHexadecimal;
            existingColor.LastChangeDate = DateTime.Now;

            await _accountRepository.EditAccountColor(existingColor);
        }

        private static Domain.Entities.Account MapInputAccountToDomain(EditAccountInput input, Domain.Entities.Account accountExists)
        {
            return new Domain.Entities.Account
            {
                Id = input.Id,
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
                CreationDate = accountExists.CreationDate,
                LastChangeDate = DateTime.Now
            };
        }
    }
}
