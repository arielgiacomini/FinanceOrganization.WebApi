using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> AccountNameIsCreditCard(string accountName);
        Task<int> Delete(Account account);
        Task<int> Edit(Account account);
        Task<int> EditAccountColor(AccountColor accountColor);
        Task<Account?> GetAccountByName(string accountName);
        Task<AccountColor?> GetAccountColorByAccountId(int accountId);
        Task<Account?> GetById(int id);
        Task<IList<Account>> GetAllAccounts();
        Task<IList<Account>> GetAllCreditCardAccounts();
        Task<int> Save(Account account);
        Task<int> SaveAccountColor(AccountColor accountColor);
    }
}