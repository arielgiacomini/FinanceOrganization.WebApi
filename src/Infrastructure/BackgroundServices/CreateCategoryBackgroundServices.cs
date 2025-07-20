using Application.EventHandlers.CreateCategoryEvent;
using Domain.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.BackgroundServices
{
    public class CreateCategoryBackgroundServices : BackgroundService
    {
        private readonly ILogger<CreateCategoryBackgroundServices> _logger;
        private readonly CategoryOptions _options;
        private readonly ICreateCategoryEventHandler _createCategoryEventHandler;

        public CreateCategoryBackgroundServices(
            ILogger<CreateCategoryBackgroundServices> logger,
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
                _logger.LogInformation("Rotina automática de cadastro de categorias está habilitada.");

                _ = Task.Run(() => RoutineFromTimeToTime(stoppingToken), stoppingToken);

                await Task.CompletedTask;
            }
            else
            {
                _logger.LogInformation("Rotina automática de cadastro de categorias está desabilitada.");
            }
        }

        private async Task RoutineFromTimeToTime(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rotina automática de cadastro de categorias será configurada para executar a cada: [{StartTime}]", _options.RoutineWorker.StartTime);

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
            _logger.LogInformation("Iniciando o BackgoundServices responsável por criação de categorias.");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finalizando o BackgoundServices responsável por criação de categorias.");

            return base.StopAsync(cancellationToken);
        }
    }
}