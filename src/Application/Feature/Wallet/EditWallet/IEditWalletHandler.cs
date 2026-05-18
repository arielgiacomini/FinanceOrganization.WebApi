namespace Application.Feature.Wallet.EditWallet
{
    public interface IEditWalletHandler
    {
        Task<EditWalletOutput> Handle(EditWalletInput input, CancellationToken cancellationToken);
    }
}