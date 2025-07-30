using Application.EventHandlers.CreateBillToPayEvent;
using Application.EventHandlers.CreateCashReceivableEvent;
using Application.EventHandlers.CreateCategoryEvent;
using Domain.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.BackgroundServices
{
    public class GenericBackgroundServices : BackgroundService
    {
        private readonly ILogger<GenericBackgroundServices> _logger;
        private readonly GenericBackgroundServiceOptions _options;
        private readonly ICreateCategoryEventHandler _createCategoryEventHandler;
        private readonly ICreateBillToPayEventHandler _createBillToPayEventHandler;
        private readonly ICreateCashReceivableEventHandler _cashReceivableHandler;

        public GenericBackgroundServices(
            ILogger<GenericBackgroundServices> logger,
            IOptions<GenericBackgroundServiceOptions> options,
            ICreateCategoryEventHandler createCategoryEventHandler,
            ICreateBillToPayEventHandler createBillToPayEventHandler,
            ICreateCashReceivableEventHandler createCashReceivableEventHandler)
        {
            _logger = logger;
            _options = options.Value;
            _createCategoryEventHandler = createCategoryEventHandler;
            _createBillToPayEventHandler = createBillToPayEventHandler;
            _cashReceivableHandler = createCashReceivableEventHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (IsRoutineEnabled(_options))
            {
                _logger.LogInformation("Rotina que executa em BackgroundServices de forma genérica está habilitada.");

                _ = Task.Run(() => RoutineFromTimeToTime(stoppingToken), stoppingToken);

                await Task.CompletedTask;
            }
            else
            {
                _logger.LogInformation("Rotina automática de forma genérica está desabilitada.");
            }
        }

        private async Task RoutineFromTimeToTime(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rotina automática de forma genérica será configurada para executar a cada: [{StartTime}]", _options.RoutineWorker.StartTime);

            while (!cancellationToken.IsCancellationRequested)
            {
                await _createCategoryEventHandler.Handle(new CreateCategoryEventInput());

                await _createBillToPayEventHandler.Handle(new CreateBillToPayEventInput() { DateExecution = DateTime.Now });

                await _cashReceivableHandler.Handle(new CreateCashReceivableEventInput() { DateExecution = DateTime.Now });

                await Task.Delay(_options.RoutineWorker.StartTime, cancellationToken);
            }
        }

        private static bool IsRoutineEnabled(GenericBackgroundServiceOptions options)
        {
            return options.RoutineWorker.Enable;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando o BackgoundServices responsável de fazer rotinas de forma genérica.");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finalizando o BackgoundServices responsável de fazer rotinas de forma genérica.");

            return base.StopAsync(cancellationToken);
        }
    }
}