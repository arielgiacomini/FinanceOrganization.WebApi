namespace Application.Feature.BillToPay.CreateBillToPay
{
    public interface ICreateBillToPayHandler
    {
        Task<CreateBillToPayOutput> Handle(CreateBillToPayInput input, CancellationToken cancellationToken = default);
    }
}