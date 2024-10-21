using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.CashReceivableRegistration.CreateCashReceivableRegistration
{
    public class CreateCashReceivableRegistrationHandler : ICreateCashReceivableRegistrationHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly ICashReceivableRegistrationRepository _cashReceivableRepository;

        public CreateCashReceivableRegistrationHandler(Serilog.ILogger logger,
            ICashReceivableRegistrationRepository cashReceivableRepository)
        {
            _logger = logger;
            _cashReceivableRepository = cashReceivableRepository;
        }

        public async Task<CreateCashReceivableRegistrationOutput> Handle(CreateCashReceivableRegistrationInput input, CancellationToken cancellationToken)
        {
            _logger.Information("Está sendo criado a conta a receber de nome: {Name}", input.Name);

            var validate = await CreateCashReceivableRegistrationValidator.ValidateInput(input, _cashReceivableRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new CreateCashReceivableRegistrationOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var isSaved = await _cashReceivableRepository.Save(MapInputCashReceivableRegistrationToDomain(input));

            var output = new CreateCashReceivableRegistrationOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
            };

            return await Task.FromResult(new CreateCashReceivableRegistrationOutput());
        }

        private static Domain.Entities.CashReceivableRegistration MapInputCashReceivableRegistrationToDomain(CreateCashReceivableRegistrationInput input)
        {
            return new Domain.Entities.CashReceivableRegistration
            {
                Name = input.Name,
                Category = input.Category,
                Account = input.Account,
                Value = input.Value,
                AgreementDate = input.AgreementDate,
                BestReceivingDay = input.BestReceivingDay ?? input.AgreementDate!.Value.Day,
                InitialMonthYear = input.InitialMonthYear,
                FynallyMonthYear = input.FynallyMonthYear,
                Frequence = input.Frequence,
                RegistrationType = input.RegistrationType,
                AdditionalMessage = input.AdditionalMessage,
                CreationDate = input.CreationDate,
                LastChangeDate = input.LastChangeDate
            };
        }
    }
}