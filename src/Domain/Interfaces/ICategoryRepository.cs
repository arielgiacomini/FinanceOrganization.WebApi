using Domain.Entities;
using Domain.Entities.Enums;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IList<Category>?> GetAllAsync(AccountType accountType);
        Task<IList<string>?> GetNonRegister();
        Task<int> SaveRange(IList<Category> categories);
    }
}