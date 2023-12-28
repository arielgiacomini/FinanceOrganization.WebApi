using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.FixedInvoice.CreateFixedInvoice
{
    public class CreateFixedInvoiceHandler : ICreateFixedInvoiceHandler
    {
        private readonly ILogger<CreateFixedInvoiceHandler> _logger;
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;

        public CreateFixedInvoiceHandler(ILogger<CreateFixedInvoiceHandler> logger,
            IFixedInvoiceRepository fixedInvoiceRepository)
        {
            _logger = logger;
            _fixedInvoiceRepository = fixedInvoiceRepository;
        }

        public async Task<CreateFixedInvoiceOutput> Handle(CreateFixedInvoiceInput input,
            CancellationToken cancellationToken = default)
        {
            var validate = await CreateFixedInvoiceValidator.ValidateInput(input, _fixedInvoiceRepository);

            if (validate.Any())
            {
                _logger.LogWarning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new CreateFixedInvoiceOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var IdFixedInvoice = await _fixedInvoiceRepository.Save(MapInputFixedInvoiceToDomain(input));

            var output = new CreateFixedInvoiceOutput
            {
                Output = OutputBaseDetails.Success($"[{IdFixedInvoice}] - Cadastro realizado com sucesso.", new object())
            };

            return await Task.FromResult(output);
        }

        private static Domain.Entities.FixedInvoice MapInputFixedInvoiceToDomain(CreateFixedInvoiceInput input)
        {
            return new Domain.Entities.FixedInvoice
            {
                Name = input.Name,
                Category = input.Category,
                Account = input.Account,
                Value = input.Value,
                BestPayDay = input.BestPayDay,
                InitialMonthYear = input.InitialMonthYear,
                FynallyMonthYear = input.FynallyMonthYear,
                Frequence = input.Frequence,
                CreationDate = input.CreationDate,
                LastChangeDate = input.LastChangeDate
            };
        }
    }
}