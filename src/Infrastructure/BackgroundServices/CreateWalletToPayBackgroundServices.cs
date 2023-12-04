using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundServices
{
    public class CreateWalletToPayBackgroundServices : BackgroundService
    {
        public CreateWalletToPayBackgroundServices()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}