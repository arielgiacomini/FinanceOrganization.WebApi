using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.CashReceivable.EditCashReceivable
{
    public class EditCashReceivableHandler : IEditCashReceivableHandler
    {
        private readonly ILogger<EditCashReceivableHandler> _logger;
        private readonly ICashReceivableRepository _cashReceivableRepository;
        private readonly ICashReceivableRegistrationRepository _cashReceivableRegistrationRepository;

        public EditCashReceivableHandler(
            ILogger<EditCashReceivableHandler> logger,
            ICashReceivableRepository cashReceivableRepository,
            ICashReceivableRegistrationRepository cashReceivableRegistrationRepository)
        {
            _logger = logger;
            _cashReceivableRepository = cashReceivableRepository;
            _cashReceivableRegistrationRepository = cashReceivableRegistrationRepository;
        }

        public async Task<EditCashReceivableOutput> Handle(EditCashReceivableInput input, CancellationToken cancellationToken)
        {
            var validate = await EditCashReceivableValidator.ValidateInput(input, _cashReceivableRepository);
            if (validate.Any())
            {
                _logger.LogWarning("Houve erro de validação: {@Validation}", validate);
                var outputValidator = new EditCashReceivableOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };
                return outputValidator;
            }

            var billToPayDatabase = await _cashReceivableRepository.GetById(input.Id);

            if (billToPayDatabase is null)
            {
                var outputError = new EditCashReceivableOutput
                {
                    Output = OutputBaseDetails.Error($"[56] - Erro ao buscar o BillToPay do Banco de Dados.", new Dictionary<string, string>(), 0)
                };
                return outputError;
            }

            var billToPay = MapInputToDomain(input, billToPayDatabase);

            var result = await _cashReceivableRepository.Edit(billToPay);

            if (input.MustEditRegistrationAccount)
            {
                var registration = await _cashReceivableRegistrationRepository.GetById(billToPay.IdCashReceivableRegistration);

                if (registration is null)
                {
                    _logger.LogWarning("Não foi possível fazer a edição da conta de registro. Essa edição era pra deixar a conta o mais atualizado possível");
                }

                registration.Name = input.Name;
                registration.Category = input.Category;
                registration.RegistrationType = input.RegistrationType;
                registration.Frequence = input.Frequence;
                registration.AdditionalMessage = string.Concat(" | ", input.AdditionalMessage);
                registration.LastChangeDate = DateTime.Now;

                var resultRegistration = await _cashReceivableRegistrationRepository.Edit(registration);

                if (resultRegistration > 0)
                {
                    _logger.LogWarning("Não foi possível fazer a edição da conta de registro. Essa edição era pra deixar a conta o mais atualizado possível");
                }
                else
                {
                    _logger.LogInformation("Alteração da conta {Name} de Id: {} realizado com sucesso.", registration.Name, registration.Id);
                }
            }

            var output = new EditCashReceivableOutput
            {
                Output = OutputBaseDetails.Success($"[{result}] - Conta a Receber alterada com sucesso.", new object())
            };

            return output;
        }

        private static Domain.Entities.CashReceivable MapInputToDomain(EditCashReceivableInput input,
            Domain.Entities.CashReceivable? cashReceivable)
        {
            return new Domain.Entities.CashReceivable()
            {
                Id = input.Id,
                IdCashReceivableRegistration = cashReceivable!.IdCashReceivableRegistration,
                Name = input.Name,
                Account = input.Account,
                Frequence = input.Frequence,
                RegistrationType = input.RegistrationType,
                AgreementDate = input.AgreementDate,
                DueDate = input.DueDate,
                YearMonth = input.YearMonth,
                Category = input.Category,
                Value = input.Value,
                ManipulatedValue = input!.ManipulatedValue,
                DateReceived = input.DateReceived,
                HasReceived = input.HasReceived,
                AdditionalMessage = input.AdditionalMessage,
                Enabled = input!.Enabled,
                CreationDate = cashReceivable!.CreationDate,
                LastChangeDate = input.LastChangeDate
            };
        }
    }
}