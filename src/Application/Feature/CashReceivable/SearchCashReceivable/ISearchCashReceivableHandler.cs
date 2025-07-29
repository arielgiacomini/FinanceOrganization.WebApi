
namespace Application.Feature.CashReceivable.SearchCashReceivable
{
    public interface ISearchCashReceivableHandler
    {
        Task<SearchCashReceivableOutput> Handle(SearchCashReceivableInput input, CancellationToken cancellationToken);
    }
}