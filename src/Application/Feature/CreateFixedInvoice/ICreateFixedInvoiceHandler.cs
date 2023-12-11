namespace Application.Feature.CreateFixedInvoice
{
    public interface ICreateFixedInvoiceHandler
    {
        Task<CreateFixedInvoiceOutput> Handle(CreateFixedInvoiceInput input, CancellationToken cancellationToken = default);
    }
}