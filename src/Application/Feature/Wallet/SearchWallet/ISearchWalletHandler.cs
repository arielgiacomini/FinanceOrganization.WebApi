namespace Application.Feature.Wallet.SearchWallet
{
    public interface ISearchWalletHandler
    {
        Task<SearchWalletOutput> Handle(SearchWalletInput input, CancellationToken cancellationToken);
    }
}