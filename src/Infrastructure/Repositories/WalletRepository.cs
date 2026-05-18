using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly FinanceOrganizationContext _context;
        private readonly ILogger _looger;

        public WalletRepository(ILogger logger, FinanceOrganizationContext context)
        {
            _context = context;
            _looger = logger;
        }

        public async Task<Wallet> GetById(Guid id)
        {
            var wallet = await _context.Wallets!.FirstOrDefaultAsync(w => w.Id == id);
            return wallet;
        }

        public async Task<IList<Wallet>> GetAllWallets()
        {
            var wallets = await _context.Wallets!.ToListAsync();

            return wallets;
        }

        public async Task<IList<Wallet>> GetByWalletKeyAsync(string walletKey)
        {
            var wallets = await _context.Wallets!
                .Where(w => w.WalletKey == walletKey)
                .ToListAsync();

            return wallets;
        }

        public async Task<int> Save(Wallet wallet)
        {
            try
            {
                _context.Add(wallet);
                var qtdEntry = await _context.SaveChangesAsync();

                return qtdEntry;
            }
            catch (Exception ex)
            {
                _looger.Error($"[WalletRepository.Save()] - Ocorreu um erro ao salvar a carteira. Exception: {ex}");

                return 0;
            }
        }

        public async Task<int> Edit(Wallet wallet)
        {
            _context.ChangeTracker.Clear();

            _context.Wallets!.Update(wallet);
            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<int> Delete(Wallet wallet)
        {
            _context.ChangeTracker.Clear();

            _ = _context.Wallets!.Remove(wallet);
            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }
    }
}