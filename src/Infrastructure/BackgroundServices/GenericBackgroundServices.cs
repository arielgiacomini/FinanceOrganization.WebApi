using Application.EventHandlers.CreateBillToPayEvent;
using Application.EventHandlers.CreateCashReceivableEvent;
using Application.EventHandlers.CreateCategoryEvent;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.BackgroundServices
{
    public class GenericBackgroundServices : BackgroundService
    {
        private readonly ILogger<GenericBackgroundServices> _logger;
        private readonly GenericBackgroundServiceOptions _options;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GenericBackgroundServices(
            ILogger<GenericBackgroundServices> logger,
            IOptions<GenericBackgroundServiceOptions> options,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _options = options.Value;
            _serviceScopeFactory = serviceScopeFactory;
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
                await RunForEveryUser(cancellationToken);

                await Task.Delay(_options.RoutineWorker.StartTime, cancellationToken);
            }
        }

        /// <summary>
        /// Não existe "o" usuário fora de uma requisição HTTP (sem token, sem HttpContext) — e os dados
        /// agora são isolados por usuário. Por isso a rotina processa cada usuário separadamente, cada um
        /// num escopo de DI próprio, com o ICurrentUserService daquele escopo apontado manualmente pra ele.
        /// Isso também evita reaproveitar os handlers (Scoped) direto no construtor do BackgroundService
        /// (Singleton) — o que os prenderia (captive dependency) resolvidos uma única vez, sem usuário
        /// nenhum, pro resto da vida do processo.
        /// </summary>
        private async Task RunForEveryUser(CancellationToken cancellationToken)
        {
            IList<Domain.Entities.User> users;

            using (var lookupScope = _serviceScopeFactory.CreateScope())
            {
                var userRepository = lookupScope.ServiceProvider.GetRequiredService<IUserRepository>();
                users = await userRepository.GetAll();
            }

            foreach (var user in users)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    using var userScope = _serviceScopeFactory.CreateScope();

                    userScope.ServiceProvider.GetRequiredService<ICurrentUserSetter>().SetUserId(user.Id);

                    var createCategoryEventHandler = userScope.ServiceProvider.GetRequiredService<ICreateCategoryEventHandler>();
                    var createBillToPayEventHandler = userScope.ServiceProvider.GetRequiredService<ICreateBillToPayEventHandler>();
                    var createCashReceivableEventHandler = userScope.ServiceProvider.GetRequiredService<ICreateCashReceivableEventHandler>();

                    await createCategoryEventHandler.Handle(new Application.EventHandlers.CreateCategoryEvent.CreateCategoryEventInput());

                    await createBillToPayEventHandler.Handle(new CreateBillToPayEventInput { DateExecution = DateTime.Now });

                    await createCashReceivableEventHandler.Handle(new Application.EventHandlers.CreateCashReceivableEvent.CreateCashReceivableEventInput { DateExecution = DateTime.Now });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro na rotina automática ao processar o usuário {UserId}. Erro: {Message}", user.Id, ex.Message);
                }
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
