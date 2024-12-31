
namespace Application.Feature.Account.SearchAccount
{
    public interface ISearchAccountHandler
    {
        Task<Output<SearchAccountOutput>> Handle();
    }
}