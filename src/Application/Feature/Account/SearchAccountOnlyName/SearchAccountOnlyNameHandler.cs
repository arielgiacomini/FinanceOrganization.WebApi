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
            IList<string> stringList = new List<string>();

            var accounts = await _accountRepository
                .GetAllAccounts();

            if (accounts == null)
            {
                return stringList;
            }

            var accountEnable = accounts
                .Where(x => x.Enable)
                .ToList();

            foreach (var item in accountEnable)
            {
                if (item.Name != null)
                {
                    stringList.Add(item.Name);
                }
            }

            return stringList;
        }
    }
}