using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBillToPayRepository
    {
        Task<int> Edit(BillToPay billToPay);
        Task<IList<BillToPay>> GetBillToPayByFixedInvoiceId(int fixedInvoiceId);
        Task<BillToPay?> GetBillToPayById(Guid id);
        Task<BillToPay?> GetBillToPayByNameDueDateAndFrequence(string name, string yearMonth, string frequence);
        Task<IList<BillToPay>?> GetBillToPayByYearMonth(string yearMonth);
        Task<int> Save(IList<BillToPay> billsToPay);
    }
}