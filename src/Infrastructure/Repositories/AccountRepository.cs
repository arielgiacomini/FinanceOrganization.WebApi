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

        public async Task<Account?> GetById(int id)
        {
            var account = await _context.Accounts!.FirstOrDefaultAsync(x => x.Id == id);

            if (account != null)
            {
                await IntoColors(new List<Account> { account });
            }

            return account;
        }

        public async Task<int> Save(Account account)
        {
            _context.Add(account);
            var qtdEntry = await _context.SaveChangesAsync();

            return qtdEntry;
        }

        public async Task<int> Edit(Account account)
        {
            _context.ChangeTracker.Clear();

            _context.Accounts!.Update(account);
            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<int> Delete(Account account)
        {
            _context.ChangeTracker.Clear();

            _context.Accounts!.Remove(account);
            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<AccountColor?> GetAccountColorByAccountId(int accountId)
        {
            return await _context.AccountColors!.FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task<int> SaveAccountColor(AccountColor accountColor)
        {
            _context.Add(accountColor);
            var qtdEntry = await _context.SaveChangesAsync();

            return qtdEntry;
        }

        public async Task<int> EditAccountColor(AccountColor accountColor)
        {
            _context.ChangeTracker.Clear();

            _context.AccountColors!.Update(accountColor);
            var result = _context.SaveChanges();

            return await Task.FromResult(result);
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