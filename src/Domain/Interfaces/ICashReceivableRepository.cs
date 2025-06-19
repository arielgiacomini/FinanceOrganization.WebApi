using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICashReceivableRepository
    {
        Task<int> Edit(CashReceivable cashReceivable);
        Task<IList<CashReceivable>> GetCashReceivableRegistrationId(int cashReceivableRegistrationId);
        Task<int> SaveRange(IList<CashReceivable> cashsReceivable);
    }
}