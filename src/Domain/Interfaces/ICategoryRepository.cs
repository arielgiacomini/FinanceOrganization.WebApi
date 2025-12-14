using Domain.Entities;
using Domain.Entities.Enums;
using System.Collections.Concurrent;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        void ClearTempCategories();
        Task<IList<Category>?> GetAllAsync(AccountType accountType, bool? filterEnable = null);
        Task<ConcurrentDictionary<string, IList<Category>>> GetCategoriesToActions();
        Task<int> SaveRange(IList<Category> categories);
        Task<bool> SetEnableOrDisableCategoryByRange(IList<Category> categoriesNotExists, bool enable);
    }
}