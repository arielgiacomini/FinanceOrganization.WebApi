
namespace Application.Feature.BillToPay.SearchMonthlyAverageAnalysis
{
    public interface ISearchMonthlyAverageAnalysisHandler
    {
        Task<SearchMonthlyAverageAnalysisOutput> Handle(SearchMonthlyAverageAnalysisInput input, CancellationToken cancellationToken = default);
    }
}