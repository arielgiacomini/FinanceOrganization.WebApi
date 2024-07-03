using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IList<Category>?> GetAllAsync();
        Task<IList<string>?> GetNonRegister();
        Task<int> SaveRange(IList<Category> categories);
    }
}