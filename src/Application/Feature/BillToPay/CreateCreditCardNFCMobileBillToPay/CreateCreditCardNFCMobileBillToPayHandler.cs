using Domain.Entities;
using Domain.Interfaces;
using Domain.Utils;
using Serilog;

namespace Application.Feature.BillToPay.CreateCreditCardNFCMobileBillToPay
{
    public class CreateCreditCardNFCMobileBillToPayHandler : ICreateCreditCardNFCMobileBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;
        private readonly IBillToPayRepository _billToPayRepository;

        public CreateCreditCardNFCMobileBillToPayHandler(ILogger logger,
            IFixedInvoiceRepository fixedInvoiceRepository,
            IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _fixedInvoiceRepository = fixedInvoiceRepository;
            _billToPayRepository = billToPayRepository;
        }

        public async Task<CreateCreditCardNFCMobileBillToPayOutput> Handle(CreateCreditCardNFCMobileBillToPayInput input,
            CancellationToken cancellationToken = default)
        {
            _logger.Information("Está sendo criado a conta a pagar de nome: {Name}", input.Name);

            var validate = await CreateCreditCardNFCMobileBillToPayValidator.ValidateInput(input, _fixedInvoiceRepository, _billToPayRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new CreateCreditCardNFCMobileBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var isSaved = await _fixedInvoiceRepository.Save(MapInputFixedInvoiceToDomain(input));

            var output = new CreateCreditCardNFCMobileBillToPayOutput
            {
                Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
            };

            return await Task.FromResult(output);
        }

        private static Domain.Entities.FixedInvoice MapInputFixedInvoiceToDomain(CreateCreditCardNFCMobileBillToPayInput input)
        {
            return new Domain.Entities.FixedInvoice
            {
                Name = input.Name,
                Category = input.Category,
                Account = input.Account,
                Value = input.Value,
                PurchaseDate = input.PurchaseDate,
                BestPayDay = input.Account == Account.CARTAO_CREDITO ? 9 : DateTime.Now.ToLocalTime().Day,
                InitialMonthYear = DateServiceUtils.GetYearMonthPortugueseByDateTime(DateTime.Now.ToLocalTime()),
                FynallyMonthYear = DateServiceUtils.GetYearMonthPortugueseByDateTime(DateTime.Now.ToLocalTime()),
                Frequence = input.Frequence,
                RegistrationType = input.RegistrationType,
                AdditionalMessage = input.AdditionalMessage,
                CreationDate = DateTime.Now.ToLocalTime(),
                LastChangeDate = null
            };
        }
    }
}