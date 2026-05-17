namespace Application.Feature.Wallet.CreateWallet
{
    public interface ICreateWalletHandler
    {
        Task<CreateWalletOutput> Handle(CreateWalletInput input, CancellationToken cancellationToken);
    }
}