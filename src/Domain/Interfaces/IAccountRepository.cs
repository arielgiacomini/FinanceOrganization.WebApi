using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<IList<Account>> GetAllAccounts();
    }
}