namespace Application.Feature.Date.SearchMonthYear
{
    public interface ISearchMonthYearHandler
    {
        Task<SearchMonthYearOutput> Handle(SearchMonthYearInput input, CancellationToken cancellationToken);
    }
}