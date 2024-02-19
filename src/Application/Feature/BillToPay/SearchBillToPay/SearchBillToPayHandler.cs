using Domain.Interfaces;
using Serilog;

namespace Application.Feature.BillToPay.SearchBillToPay
{
    public class SearchBillToPayHandler : ISearchBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;

        public SearchBillToPayHandler(ILogger logger, IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _billToPayRepository = billToPayRepository;
        }

        public async Task<SearchBillToPayOutput> Handle(SearchBillToPayInput input, CancellationToken cancellationToken = default)
        {
            _logger.Information("Está sendo criado a conta a pagar de nome: {YearMonth}", input.YearMonth);

            var validate = await SearchBillToPayValidator.ValidateInput(input);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new SearchBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var output = new SearchBillToPayOutput()
            {
                Output = new OutputBaseDetails()
            };

            if (input.YearMonth != null)
            {
                var billToPayByYearMonth = await _billToPayRepository.GetBillToPayByYearMonth(input.YearMonth);

                if (billToPayByYearMonth != null)
                {
                    output.Output = OutputBaseDetails.Success("", billToPayByYearMonth, billToPayByYearMonth.Count);

                    return output;
                }
            }

            if (input.Id != null)
            {
                foreach (var IdBillToPay in input.Id)
                {
                    var billToPayById = await _billToPayRepository.GetBillToPayById(IdBillToPay);

                    if (billToPayById != null)
                    {
                        output.Output = OutputBaseDetails.Success("", billToPayById, 1);

                        return output;
                    }
                }
            }

            if (input.IdFixedInvoices != null)
            {
                foreach (var idFixedInvoice in input.IdFixedInvoices)
                {
                    var billToPayByIdFixedInvoice = await _billToPayRepository.GetBillToPayByFixedInvoiceId(idFixedInvoice);

                    if (billToPayByIdFixedInvoice != null)
                    {
                        output.Output = OutputBaseDetails.Success("", billToPayByIdFixedInvoice, billToPayByIdFixedInvoice.Count);

                        return output;
                    }
                }
            }

            return output;
        }
    }
}