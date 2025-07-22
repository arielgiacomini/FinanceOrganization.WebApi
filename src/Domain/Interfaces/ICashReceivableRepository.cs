using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICashReceivableRepository
    {
        Task<int> Edit(CashReceivable cashReceivable);
        Task<CashReceivable?> GetByAccountAndMonthYear(string account, string monthYear);
        Task<IList<CashReceivable>> GetCashReceivableRegistrationId(int cashReceivableRegistrationId);
        Task<int> Save(CashReceivable cashReceivable);
        Task<int> SaveRange(IList<CashReceivable> cashsReceivable);
    }
}