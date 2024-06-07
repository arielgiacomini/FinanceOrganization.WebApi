using Application.EventHandlers.CreateCategoryEvent;
using Domain.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.BackgroundServices
{
    public class CreateCategoryBackgroundServices : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly CategoryOptions _options;
        private readonly ICreateCategoryEventHandler _createCategoryEventHandler;

        public CreateCategoryBackgroundServices(
            ILogger logger,
            IOptions<CategoryOptions> options,
            ICreateCategoryEventHandler createCategoryEventHandler)
        {
            _logger = logger;
            _options = options.Value;
            _createCategoryEventHandler = createCategoryEventHandler;
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
                _logger.Information("[CreateWalletToPayBackgroundServices] - Rotina está desabilitada para efetuar o processo em background.");
            }
        }

        private async Task RoutineFromTimeToTime(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _createCategoryEventHandler.Handle(new CreateCategoryEventInput());

                await Task.Delay(_options.RoutineWorker.StartTime, cancellationToken);
            }
        }

        private static bool IsRoutineEnabled(CategoryOptions options)
        {
            return options.RoutineWorker.Enable;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information("[CreateCategoryBackgroundServices] - Iniciando o BackgoundServices responsável por criação de novas categorias");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information("[CreateCategoryBackgroundServices] - Finalizando o BackgoundServices responsável por criação de novas categorias");

            return base.StopAsync(cancellationToken);
        }
    }
}