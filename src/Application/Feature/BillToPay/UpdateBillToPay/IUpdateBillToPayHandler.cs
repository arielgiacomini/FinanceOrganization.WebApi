namespace Application.Feature.BillToPay.UpdateBillToPay
{
    public interface IUpdateBillToPayHandler
    {
        Task<UpdateBillToPayOutput> Handle(UpdateBillToPayInput input, CancellationToken cancellationToken);
    }
}