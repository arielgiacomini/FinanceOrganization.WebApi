using Domain.Interfaces;

namespace Application.Feature.Account.SearchAccountOnlyName
{
    public class SearchAccountOnlyNameHandler : ISearchAccountOnlyNameHandler
    {
        private readonly IAccountRepository _accountRepository;

        public SearchAccountOnlyNameHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IList<string>> Handle()
        {
            IList<string> accountListString = new List<string>();

            var accounts = await _accountRepository
                .GetAllAccounts();

            if (accounts == null)
            {
                return accountListString;
            }

            var accountEnable = accounts
                .Where(x => x.Enable)
                .OrderBy(x => x.Name)
                .ToList();

            foreach (var account in accountEnable)
            {
                if (account.Name != null)
                {
                    accountListString.Add(account.Name);
                }
            }

            return accountListString;
        }
    }
}