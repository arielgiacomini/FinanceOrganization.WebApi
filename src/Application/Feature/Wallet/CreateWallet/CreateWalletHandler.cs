using Domain.Interfaces;

namespace Application.Feature.Wallet.CreateWallet
{
    public class CreateWalletHandler : ICreateWalletHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly IWalletRepository _walletRepository;

        public CreateWalletHandler(Serilog.ILogger logger, IWalletRepository walletRepository)
        {
            _logger = logger;
            _walletRepository = walletRepository;
        }

        public async Task<CreateWalletOutput> Handle(CreateWalletInput input, CancellationToken cancellationToken)
        {
            _logger.Information("Está sendo criado a carteira de nome: {Name}", input.WalletKey);

            var validate = await CreateWalletValidator.ValidateInput(input, _walletRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);
                var outputValidator = new CreateWalletOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };
                return outputValidator;
            }

            var isSaved = await _walletRepository.Save(MapInputWalletToDomain(input));

            var output = new CreateWalletOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
            };

            return await Task.FromResult(output);
        }

        private Domain.Entities.Wallet MapInputWalletToDomain(CreateWalletInput input)
        {
            return new Domain.Entities.Wallet
            {
                Id = input.Id,
                WalletKey = input.WalletKey,
                WalletValue = input.WalletValue,
                CreationDate = input.CreationDate,
                LastChangeDate = null
            };
        }
    }
}