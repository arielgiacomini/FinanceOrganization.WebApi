namespace Application.Feature.FixedInvoice.CreateFixedInvoice
{
    public interface ICreateFixedInvoiceHandler
    {
        Task<CreateFixedInvoiceOutput> Handle(CreateFixedInvoiceInput input, CancellationToken cancellationToken = default);
    }
}