namespace Application.Feature.SearchFixedInvoice
{
    public interface ISearchFixedInvoiceHandler
    {
        Task<SearchFixedInvoiceOutput> Handle(CancellationToken cancellationToken = default);
    }
}