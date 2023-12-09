using Application.Feature.EventHandlers.WalletToPay;
using Domain.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.BackgroundServices
{
    public class CreateWalletToPayBackgroundServices : BackgroundService
    {
        private readonly ILogger<CreateWalletToPayBackgroundServices> _logger;
        private readonly WalletToPayOptions _options;
        private readonly IWalletToPayHandler _walletToPayHandler;

        public CreateWalletToPayBackgroundServices(
            ILogger<CreateWalletToPayBackgroundServices> logger,
            IOptions<WalletToPayOptions> options,
            IWalletToPayHandler walletToPayHandler)
        {
            _logger = logger;
            _options = options.Value;
            _walletToPayHandler = walletToPayHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (IsRoutineEnabled(_options))
            {
                _ = Task.Run(() => RoutineFromTimeToTime(stoppingToken), stoppingToken);

                await Task.CompletedTask;
            }
            else
            {
                _logger.LogInformation("[CreateWalletToPayBackgroundServices] - Rotina está desabilitada para efetuar o processo em background.");
            }
        }

        private async Task RoutineFromTimeToTime(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _walletToPayHandler.Handle(new WalletToPayInput() { DateExecution = DateTime.Now });

                await Task.Delay(_options.RoutineWorker.StartTime, cancellationToken);
            }
        }

        private static bool IsRoutineEnabled(WalletToPayOptions options)
        {
            return options.RoutineWorker.Enable;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[CreateWalletToPayBackgroundServices] - Iniciando o BackgoundServices responsável por criação da carteira de pagamentos.");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[CreateWalletToPayBackgroundServices] - Finalizando o BackgoundServices responsável por criação da carteira de pagamentos.");

            return base.StopAsync(cancellationToken);
        }
    }
}