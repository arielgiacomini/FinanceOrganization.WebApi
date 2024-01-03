namespace Application.Feature.BillToPay.SearchBillToPay
{
    public interface ISearchBillToPayHandler
    {
        Task<SearchBillToPayOutput> Handle(SearchBillToPayInput input, CancellationToken cancellationToken = default);
    }
}