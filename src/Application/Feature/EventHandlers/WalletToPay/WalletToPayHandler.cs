using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.Feature.EventHandlers.WalletToPay
{
    public class WalletToPayHandler : IWalletToPayHandler
    {
        private readonly ILogger<WalletToPayHandler> _logger;
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;
        private readonly WalletToPayOptions _walletToPayOptions;

        public WalletToPayHandler(
            ILogger<WalletToPayHandler> logger,
            IFixedInvoiceRepository fixedInvoiceRepository,
            IOptions<WalletToPayOptions> options)
        {
            _logger = logger;
            _fixedInvoiceRepository = fixedInvoiceRepository;
            _walletToPayOptions = options.Value;
        }

        public async Task Handle(WalletToPayInput input)
        {
            var fixedInvoices = await _fixedInvoiceRepository.GetByAll();

            foreach (var fixedInvoice in fixedInvoices)
            {
                var json = JsonSerializer.Serialize(fixedInvoice);

                _logger.LogInformation($"O carregamento da informação: {json}");

                if (IsMomentRegistration(fixedInvoice))
                {
                    // CRIA A ROTINA DE INSERIR NOVA CONTA A PAGAR.
                }
            }
        }

        /// <summary>
        /// Responsável de verificar se o registro no banco de dados está elegível para cadastro agora.
        /// </summary>
        /// <param name="fixedInvoice"></param>
        /// <returns></returns>
        private bool IsMomentRegistration(FixedInvoice fixedInvoice)
        {
            if (!fixedInvoice.LastChangeDate.HasValue)
            {
                return true;
            }
            else
            {
                TimeSpan timeSpanLastChangeDate = new(
                    fixedInvoice.LastChangeDate.Value.Hour,
                    fixedInvoice.LastChangeDate.Value.Minute,
                    fixedInvoice.LastChangeDate.Value.Second,
                    fixedInvoice.LastChangeDate.Value.Millisecond);

                // TODO: Criar a lógica que verifica se está na hora de criar de acordo com a configuração no AppSettings.

                var difference = (timeSpanLastChangeDate - _walletToPayOptions.RoutineWorker.StartTime);

                return true;
            }
        }
    }
}