using Serilog;

namespace Application.Feature.BillToPay.SearchBillToPay
{
    public class SearchBillToPayHandler : ISearchBillToPayHandler
    {
        private readonly ILogger _logger;

        public SearchBillToPayHandler(ILogger logger)
        {
            _logger = logger;
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



            return await Task.FromResult(new SearchBillToPayOutput());
        }
    }
}