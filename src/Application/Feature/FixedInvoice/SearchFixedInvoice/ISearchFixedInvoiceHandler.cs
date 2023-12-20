namespace Application.Feature.FixedInvoice.SearchFixedInvoice
{
    public interface ISearchFixedInvoiceHandler
    {
        Task<SearchFixedInvoiceOutput> Handle(CancellationToken cancellationToken = default);
    }
}