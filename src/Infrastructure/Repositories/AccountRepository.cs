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

            await IntoColors(accounts);

            return accounts;
        }

        public async Task<IList<Account>> GetAllCreditCardAccounts()
        {
            var accounts = await GetAllAccounts();

            await IntoColors(accounts);

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
                .FirstOrDefault(account => account.Name == accountName);
        }

        public async Task<int> Save(Account account)
        {
            _context.Add(account);
            var qtdEntry = await _context.SaveChangesAsync();

            return qtdEntry;
        }

        private async Task<IList<AccountColor>?> GetAllAccountColors()
        {
            return await _context.AccountColors!.ToListAsync();
        }

        private async Task IntoColors(IList<Account>? accounts)
        {
            var accountColors = await GetAllAccountColors();

            foreach (var account in accounts)
            {
                var accountColor = accountColors?.Where(x => x.AccountId == account.Id).FirstOrDefault();

                if (accountColor != null)
                {
                    account.Colors = new AccountColor()
                    {
                        Id = accountColor!.Id,
                        AccountId = accountColor.AccountId,
                        BackgroundColorHexadecimal = accountColor.BackgroundColorHexadecimal,
                        FonteColorHexadecimal = accountColor.FonteColorHexadecimal,
                        Enable = accountColor.Enable,
                        CreationDate = accountColor.CreationDate,
                        LastChangeDate = accountColor.LastChangeDate
                    };
                }
            }
        }
    }
}