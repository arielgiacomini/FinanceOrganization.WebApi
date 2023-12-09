namespace Application.Feature.EventHandlers.WalletToPay
{
    public class WalletToPayHandler : IWalletToPayHandler
    {
        public WalletToPayHandler()
        {

        }

        public async Task Handle(WalletToPayInput input)
        {
            await Task.CompletedTask;
        }
    }
}