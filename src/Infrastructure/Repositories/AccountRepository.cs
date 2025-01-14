using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FinanceOrganizationContext _context;
        private readonly ILogger _looger;

        public AccountRepository(ILogger logger, FinanceOrganizationContext context)
        {
            _context = context;
            _looger = logger;
        }

        public async Task<IList<Account>> GetAllAccounts()
        {
            var accounts = await _context.Accounts!.ToListAsync();

            return accounts;
        }

        public async Task<IList<Account>> GetAllCreditCardAccounts()
        {
            var accounts = await GetAllAccounts();

            return accounts
                .Where(creditCard => creditCard.IsCreditCard)
                .ToList();
        }

        public async Task<bool> AccountNameIsCreditCard(string? accountName)
        {
            var accounts = await GetAllAccounts();

            return accounts
                .FirstOrDefault(creditCard => creditCard.Name == accountName)?.IsCreditCard ?? false;
        }

        public async Task<Account?> GetAccountByName(string accountName)
        {
            var accounts = await GetAllAccounts();

            return accounts
                .FirstOrDefault(creditCard => creditCard.Name == accountName);
        }
    }
}