namespace Application.Feature.BillToPay.EditBillToPay
{
    public interface IEditBillToPayHandler
    {
        Task<EditBillToPayOutput> Handle(EditBillToPayInput input, CancellationToken cancellationToken);
    }
}