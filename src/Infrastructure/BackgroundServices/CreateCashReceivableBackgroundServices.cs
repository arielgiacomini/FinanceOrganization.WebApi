using Application.EventHandlers.CreateCashReceivableEvent;
using Domain.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.BackgroundServices
{
    public class CreateCashReceivableBackgroundServices : BackgroundService
    {
        private readonly ILogger<CreateCashReceivableBackgroundServices> _logger;
        private readonly CashReceivableOptions _options;
        private readonly ICreateCashReceivableEventHandler _cashReceivableHandler;

        public CreateCashReceivableBackgroundServices(
            ILogger<CreateCashReceivableBackgroundServices> logger,
            IOptions<CashReceivableOptions> options,
            ICreateCashReceivableEventHandler createCashReceivableEventHandler)
        {
            _logger = logger;
            _options = options.Value;
            _cashReceivableHandler = createCashReceivableEventHandler;
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
                _logger.LogInformation("[CreateCashReceivableBackgroundServices] - Rotina está desabilitada para efetuar o processo em background.");
            }
        }

        private async Task RoutineFromTimeToTime(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _cashReceivableHandler.Handle(new CreateCashReceivableEventInput() { DateExecution = DateTime.Now });

                await Task.Delay(_options.RoutineWorker.StartTime, cancellationToken);
            }
        }

        private static bool IsRoutineEnabled(CashReceivableOptions options)
        {
            return options.RoutineWorker.Enable;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[CreateCashReceivableBackgroundServices] - Iniciando o BackgoundServices responsável por criação da carteira de pagamentos.");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[CreateCashReceivableBackgroundServices] - Finalizando o BackgoundServices responsável por criação da carteira de pagamentos.");

            return base.StopAsync(cancellationToken);
        }
    }
}