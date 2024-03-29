﻿using Application.EventHandlers.CreateBillToPayEvent;
using Domain.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.BackgroundServices
{
    public class CreateBillToPayBackgroundServices : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly BillToPayOptions _options;
        private readonly ICreateBillToPayEventHandler _walletToPayHandler;

        public CreateBillToPayBackgroundServices(
            ILogger logger,
            IOptions<BillToPayOptions> options,
            ICreateBillToPayEventHandler walletToPayHandler)
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
                _logger.Information("[CreateWalletToPayBackgroundServices] - Rotina está desabilitada para efetuar o processo em background.");
            }
        }

        private async Task RoutineFromTimeToTime(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _walletToPayHandler.Handle(new CreateBillToPayEventInput() { DateExecution = DateTime.Now });

                await Task.Delay(_options.RoutineWorker.StartTime, cancellationToken);
            }
        }

        private static bool IsRoutineEnabled(BillToPayOptions options)
        {
            return options.RoutineWorker.Enable;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information("[CreateWalletToPayBackgroundServices] - Iniciando o BackgoundServices responsável por criação da carteira de pagamentos.");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information("[CreateWalletToPayBackgroundServices] - Finalizando o BackgoundServices responsável por criação da carteira de pagamentos.");

            return base.StopAsync(cancellationToken);
        }
    }
}