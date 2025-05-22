using Domain.Interfaces;

namespace Application.Feature.Account.SearchAccount
{
    public class SearchAccountHandler : ISearchAccountHandler
    {
        private readonly IAccountRepository _accountRepository;

        public SearchAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Output<SearchAccountOutput>> Handle()
        {
            var accounts = await _accountRepository
                .GetAllAccounts();

            if (accounts == null)
            {
                return Output<SearchAccountOutput>.NoContent("Não foi encontrado nenhuma Conta nos registros.");
            }

            return Output<SearchAccountOutput>.Success("", accounts, accounts.Count);
        }
    }
}