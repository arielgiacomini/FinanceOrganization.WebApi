using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetById(Guid id);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByGoogleSub(string googleSub);
        Task<int> Save(User user);
        Task<int> Edit(User user);
    }
}
