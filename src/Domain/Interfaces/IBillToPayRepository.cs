using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBillToPayRepository
    {
        Task<int> Edit(BillToPay billToPay);
        Task<int> EditRange(IList<BillToPay> billToPays);
        Task<IList<BillToPay>> GetBillToPayByFixedInvoiceId(int fixedInvoiceId);
        Task<BillToPay?> GetBillToPayById(Guid id);
        Task<BillToPay?> GetBillToPayByNameDueDateAndFrequence(string name, string yearMonth, string frequence);
        Task<IList<BillToPay>?> GetBillToPayByYearMonth(string yearMonth);
        Task<IList<BillToPay>?> GetBillToPayByYearMonthAndAccount(string yearMonth, string account);
        Task<BillToPay?> GetByYearMonthCategoryAndRegistrationType(string yearMonth, string category, string registrationType);
        Task<int> SaveRange(IList<BillToPay> billsToPay);
    }
}