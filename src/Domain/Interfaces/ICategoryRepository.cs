using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IList<Category>?> GetAllAsync();
    }
}