using Domain.Interfaces;
using Serilog;

namespace Application.Feature.BillToPay.CreateBillToPay
{
    public class CreateBillToPayHandler : ICreateBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;
        private readonly IBillToPayRepository _billToPayRepository;

        public CreateBillToPayHandler(ILogger logger,
            IFixedInvoiceRepository fixedInvoiceRepository,
            IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _fixedInvoiceRepository = fixedInvoiceRepository;
            _billToPayRepository = billToPayRepository;
        }

        public async Task<CreateBillToPayOutput> Handle(CreateBillToPayInput input,
            CancellationToken cancellationToken = default)
        {
            _logger.Information("Está sendo criado a conta a pagar de nome: {Name}", input.Name);

            var validate = await CreateBillToPayValidator.ValidateInput(input, _fixedInvoiceRepository, _billToPayRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new CreateBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var isSaved = await _fixedInvoiceRepository.Save(MapInputFixedInvoiceToDomain(input));

            var output = new CreateBillToPayOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
            };

            return await Task.FromResult(output);
        }

        private static Domain.Entities.FixedInvoice MapInputFixedInvoiceToDomain(CreateBillToPayInput input)
        {
            return new Domain.Entities.FixedInvoice
            {
                Name = input.Name,
                Category = input.Category,
                Account = input.Account,
                Value = input.Value,
                PurchaseDate = input.PurchaseDate,
                BestPayDay = input.BestPayDay ?? input.PurchaseDate!.Value.Day,
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