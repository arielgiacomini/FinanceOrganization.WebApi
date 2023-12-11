using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWalletToPayRepository
    {
        Task<IList<BillToPay>> GetBillToPayByFixedInvoiceId(int fixedInvoiceId);
        Task<int> Save(IList<BillToPay> billsToPay);
    }
}