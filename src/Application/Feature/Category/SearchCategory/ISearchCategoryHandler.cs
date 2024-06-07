
namespace Application.Feature.Category.SearchCategory
{
    public interface ISearchCategoryHandler
    {
        Task<IList<string>> Handle();
    }
}