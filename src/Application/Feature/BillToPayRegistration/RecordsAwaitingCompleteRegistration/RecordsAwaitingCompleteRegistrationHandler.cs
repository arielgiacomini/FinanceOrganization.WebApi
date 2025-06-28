using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.BillToPayRegistration.RecordsAwaitingCompleteRegistration
{
    public class RecordsAwaitingCompleteRegistrationHandler : IRecordsAwaitingCompleteRegistrationHandler
    {
        private readonly ILogger<RecordsAwaitingCompleteRegistrationHandler> _logger;
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;

        public RecordsAwaitingCompleteRegistrationHandler(
            ILogger<RecordsAwaitingCompleteRegistrationHandler> logger,
            IBillToPayRegistrationRepository billToPayRegistrationRepository)
        {
            _logger = logger;
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
        }

        public async Task<RecordsAwaitingCompleteRegistrationOutput> Handle(RecordsAwaitingCompleteRegistrationInput input, CancellationToken cancellationToken)
        {
            var billToPay = await _billToPayRegistrationRepository.GetBillToPayNotRegistrationPrincipal();

            if (billToPay == null)
            {
                return new RecordsAwaitingCompleteRegistrationOutput
                {
                    Output = OutputBaseDetails.Validation("Não foram encontrados registros", new Dictionary<string, string>(), 0)
                };
            }

            var output = new RecordsAwaitingCompleteRegistrationOutput();
            output = new RecordsAwaitingCompleteRegistrationOutput
            {
                Output = OutputBaseDetails.Success("Busca realizada com sucesso.", billToPay, billToPay.Count)
            };

            return output;
        }
    }
}