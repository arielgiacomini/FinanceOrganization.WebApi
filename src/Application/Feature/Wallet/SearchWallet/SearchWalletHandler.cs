using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.Wallet.SearchWallet
{
    public class SearchWalletHandler : ISearchWalletHandler
    {
        private readonly ILogger<SearchWalletHandler> _logger;
        private readonly IWalletRepository _walletRepository;

        public SearchWalletHandler(ILogger<SearchWalletHandler> logger, IWalletRepository walletRepository)
        {
            _logger = logger;
            _walletRepository = walletRepository;
        }

        public async Task<SearchWalletOutput> Handle(SearchWalletInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling SearchWalletRequest for WalletKey: {WalletKey}", input.WalletKey);

            var wallets = await _walletRepository.GetAllWallets();

            var output = new SearchWalletOutput()
            {
                Output = new OutputBaseDetails()
            };

            if (wallets == null || wallets.Count == 0)
            {
                _logger.LogWarning("No wallet found for WalletKey: {WalletKey}", input.WalletKey);

                output.Output = OutputBaseDetails.Success("Não foi encontrado nenhuma carteira", wallets, wallets.Count);

                return output;
            }

            output.Output = OutputBaseDetails.Success("Carteiras encontradas", wallets, wallets.Count);

            return output;
        }
    }
}