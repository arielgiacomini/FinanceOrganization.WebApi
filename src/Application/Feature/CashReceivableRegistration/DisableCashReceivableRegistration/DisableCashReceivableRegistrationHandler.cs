using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.CashReceivableRegistration.DisableCashReceivableRegistration
{
    public class DisableCashReceivableRegistrationHandler : IDisableCashReceivableRegistrationHandler
    {
        private readonly ILogger<DisableCashReceivableRegistrationHandler> _logger;
        private readonly ICashReceivableRegistrationRepository _cashReceivableRegistrationRepository;

        public DisableCashReceivableRegistrationHandler(
            ILogger<DisableCashReceivableRegistrationHandler> logger,
            ICashReceivableRegistrationRepository cashReceivableRegistrationRepository)
        {
            _logger = logger;
            _cashReceivableRegistrationRepository = cashReceivableRegistrationRepository;
        }

        public async Task<DisableCashReceivableRegistrationOutput> Handle(
            DisableCashReceivableRegistrationInput input,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Desabilitar o seguinte cadastro de conta a receber de número: {Id}", input.Id);

            var validate = await DisableCashReceivableRegistrationValidator.ValidateInput(input, _cashReceivableRegistrationRepository);

            if (validate.Any())
            {
                _logger.LogWarning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);
                var outputValidator = new DisableCashReceivableRegistrationOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };
                return outputValidator;
            }

            var isSaved = await _cashReceivableRegistrationRepository.Disable(input.Id);

            DisableCashReceivableRegistrationOutput output;

            if (isSaved == 0)
            {
                output = new DisableCashReceivableRegistrationOutput
                {
                    Output = OutputBaseDetails.Validation("Não foi possível desabilitar o cadastro.", new Dictionary<string, string>())
                };
            }
            else
            {
                output = new DisableCashReceivableRegistrationOutput
                {
                    Output = OutputBaseDetails.Success($"[{isSaved}] - Desabilitado com sucesso.", new object(), 1)
                };
            }

            return output;
        }
    }
}