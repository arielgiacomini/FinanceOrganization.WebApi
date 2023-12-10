namespace Application.EventHandlers.WalletToPay
{
    public interface IWalletToPayHandler
    {
        Task Handle(WalletToPayInput input);
    }
}