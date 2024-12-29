
namespace Application.Feature.Account.SearchAccountOnlyName
{
    public interface ISearchAccountOnlyNameHandler
    {
        Task<IList<string>> Handle();
    }
}