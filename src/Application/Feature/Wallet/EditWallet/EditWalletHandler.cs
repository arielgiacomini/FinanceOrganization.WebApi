using Domain.Interfaces;

namespace Application.Feature.Wallet.EditWallet
{
    public class EditWalletHandler : IEditWalletHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly IWalletRepository _walletRepository;

        public EditWalletHandler(Serilog.ILogger logger, IWalletRepository walletRepository)
        {
            _logger = logger;
            _walletRepository = walletRepository;
        }

        public async Task<EditWalletOutput> Handle(EditWalletInput input, CancellationToken cancellationToken)
        {
            _logger.Information("Está sendo editada a carteira de nome: {Name}", input.WalletKey);

            var validate = await EditWalletValidator.ValidateInput(input, _walletRepository);
            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);
                var outputValidator = new EditWalletOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };
                return outputValidator;
            }

            var walletExists = await _walletRepository.GetById(input.Id);

            if (walletExists == null)
            {
                _logger.Warning("Carteira não encontrada para o id: {Id}", input.Id);
                var outputNotFound = new EditWalletOutput
                {
                    Output = OutputBaseDetails.Error("Carteira não encontrada.", new Dictionary<string, string> { { "Id", "Carteira não encontrada." } })
                };
                return outputNotFound;
            }

            var isSaved = await _walletRepository.Edit(MapInputWalletToDomain(input, walletExists));

            var output = new EditWalletOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
            };

            return await Task.FromResult(output);
        }

        private Domain.Entities.Wallet MapInputWalletToDomain(EditWalletInput input, Domain.Entities.Wallet walletExists)
        {
            return new Domain.Entities.Wallet
            {
                Id = input.Id,
                WalletKey = input.WalletKey,
                WalletValue = input.WalletValue,
                CreationDate = walletExists.CreationDate,
                LastChangeDate = DateTime.Now
            };
        }
    }
}