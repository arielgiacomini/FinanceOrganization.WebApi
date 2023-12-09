namespace Application.Feature.EventHandlers.WalletToPay
{
    public interface IWalletToPayHandler
    {
        Task Handle(WalletToPayInput input);
    }
}