using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWalletToPayRepository
    {
        Task<int> Edit(BillToPay billToPay);
        Task<IList<BillToPay>> GetBillToPayByFixedInvoiceId(int fixedInvoiceId);
        Task<BillToPay?> GetBillToPayById(Guid id);
        Task<int> Save(IList<BillToPay> billsToPay);
    }
}