namespace Application.Feature.CreateFixedInvoice
{
    public interface ICreateFixedInvoiceHandler
    {
        Task<CreateFixedInvoiceOutput> Handler(CreateFixedInvoiceInput input, CancellationToken cancellationToken = default);
    }
}