using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICashReceivableRepository
    {
        Task<int> Edit(CashReceivable cashReceivable);
        Task<IList<CashReceivable>> GetAll();
        Task<CashReceivable?> GetByAccountAndMonthYear(string account, string monthYear);
        Task<CashReceivable> GetByCategoryAndMonthYear(string category, string monthYear);
        Task<CashReceivable?> GetById(Guid id);
        Task<IList<CashReceivable>> GetByMonthYear(string monthYear);
        Task<IList<CashReceivable>> GetByYearMonthAndCategoryAndRegistrationType(string yearMonth, string category, string registationType);
        Task<IList<CashReceivable>> GetCashReceivableRegistrationId(int cashReceivableRegistrationId);
        Task<int> Save(CashReceivable cashReceivable);
        Task<int> SaveRange(IList<CashReceivable> cashsReceivable);
    }
}