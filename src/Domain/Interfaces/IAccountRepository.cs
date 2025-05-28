using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> AccountNameIsCreditCard(string accountName);
        Task<Account?> GetAccountByName(string accountName);
        Task<IList<Account>> GetAllAccounts();
        Task<IList<Account>> GetAllCreditCardAccounts();
        Task<int> Save(Account account);
    }
}