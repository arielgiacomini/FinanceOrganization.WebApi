using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.BillToPayRegistration.DisableBillToPayRegistration
{
    public class DisableBillToPayRegistrationHandler : IDisableBillToPayRegistrationHandler
    {
        private readonly ILogger<DisableBillToPayRegistrationHandler> _logger;
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;

        public DisableBillToPayRegistrationHandler(
            ILogger<DisableBillToPayRegistrationHandler> logger,
            IBillToPayRegistrationRepository billToPayRegistrationRepository)
        {
            _logger = logger;
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
        }

        public async Task<DisableBillToPayRegistrationOutput> Handle(
            DisableBillToPayRegistrationInput input,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Desabilitar o seguinte cadastro de conta a pagar de número: {Id}", input.Id);

            var validate = await DisableBillToPayRegistrationValidator.ValidateInput(input, _billToPayRegistrationRepository);

            if (validate.Any())
            {
                _logger.LogWarning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new DisableBillToPayRegistrationOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var isSaved = await _billToPayRegistrationRepository.Disable(input.Id);


            DisableBillToPayRegistrationOutput output;

            if (isSaved == 0)
            {
                output = new DisableBillToPayRegistrationOutput
                {
                    Output = OutputBaseDetails.Validation("Não foi possível desabilitar o cadastro.", new Dictionary<string, string>())
                };
            }
            else
            {
                output = new DisableBillToPayRegistrationOutput
                {
                    Output = OutputBaseDetails.Success($"[{isSaved}] - Desabilitado com sucesso.", new object(), 1)
                };
            }

            return output;
        }
    }
}