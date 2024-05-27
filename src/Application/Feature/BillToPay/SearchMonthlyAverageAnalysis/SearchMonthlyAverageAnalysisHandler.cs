using Domain.Interfaces;
using Serilog;

namespace Application.Feature.BillToPay.SearchMonthlyAverageAnalysis
{
    public class SearchMonthlyAverageAnalysisHandler : ISearchMonthlyAverageAnalysisHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;

        public SearchMonthlyAverageAnalysisHandler(ILogger logger, IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _billToPayRepository = billToPayRepository;
        }

        public async Task<SearchMonthlyAverageAnalysisOutput> Handle(SearchMonthlyAverageAnalysisInput input, CancellationToken cancellationToken = default)
        {
            _logger.Information("Está sendo efetuado uma busca de dados para análise mensal: {startDate}, {endDate}, {quantityMonthsAnalysis}",
                input.StartDate, input.EndDate, input.QuantityMonthsAnalysis);

            var validate = await SearchMonthlyAverageAnalysisValidator.ValidateInput(input);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new SearchMonthlyAverageAnalysisOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var result = await _billToPayRepository
                .GetMonthlyAverageAnalysesAsync(input.StartDate, input.EndDate, input.QuantityMonthsAnalysis);

            var output = new SearchMonthlyAverageAnalysisOutput()
            {
                Output = new OutputBaseDetails()
            };

            if (result != null)
            {
                output.Output = OutputBaseDetails.Success("", result, result.Count);
            }

            return output;
        }
    }
}