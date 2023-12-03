using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundServices
{
    public class CreateWalletToPayBackgroundServices : BackgroundService
    {
        public CreateWalletToPayBackgroundServices()
        {

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.FromResult<bool>(true);
        }
    }
}