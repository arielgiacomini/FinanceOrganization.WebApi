
namespace Application.Feature.Date.SearchAllWithFilters
{
    public interface ISearchDateAllWithFiltersHandler
    {
        Task<IList<SearchDateAllWithFiltersOutput>> Handle(SearchDateAllWithFiltersInput input, CancellationToken cancellationToken);
    }
}