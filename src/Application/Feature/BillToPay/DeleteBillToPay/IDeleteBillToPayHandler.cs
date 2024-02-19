

namespace Application.Feature.BillToPay.DeleteBillToPay
{
    public interface IDeleteBillToPayHandler
    {
        Task<DeleteBillToPayOutput> Handle(DeleteBillToPayInput input, CancellationToken cancellationToken);
    }
}