using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWalletRepository
    {
        Task<int> Delete(Wallet wallet);
        Task<int> Edit(Wallet wallet);
        Task<IList<Wallet>> GetAllWallets();
        Task<IList<Wallet>> GetByWalletKeyAsync(string walletKey);
        Task<int> Save(Wallet wallet);
    }
}