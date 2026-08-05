
namespace Application.Feature.RealEstateFinancing.SearchRealEstateFinancing
{
    public interface ISearchRealEstateFinancingHandler
    {
        Task<Output<SearchRealEstateFinancingOutput>> Handle();
    }
}
