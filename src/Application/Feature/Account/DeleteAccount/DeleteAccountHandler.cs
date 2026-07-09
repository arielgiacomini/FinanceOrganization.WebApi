using Domain.Interfaces;

namespace Application.Feature.Account.DeleteAccount
{
    public class DeleteAccountHandler : IDeleteAccountHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly IAccountRepository _accountRepository;

        public DeleteAccountHandler(Serilog.ILogger logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<DeleteAccountOutput> Handle(DeleteAccountInput input, CancellationToken cancellationToken = default)
        {
            _logger.Information("Está sendo deletada a conta de id: {Id}", input.Id);

            var validate = await DeleteAccountValidator.ValidateInput(input, _accountRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new DeleteAccountOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var account = await _accountRepository.GetById(input.Id);

            var isDeleted = await _accountRepository.Delete(account!);

            var output = new DeleteAccountOutput
            {
                Output = OutputBaseDetails.Success($"[{isDeleted}] - Delete realizado com sucesso.", new object(), 1)
            };

            return output;
        }
    }
}
