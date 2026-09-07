using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetById(Guid id);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByGoogleSub(string googleSub);
        /// <summary>
        /// Todos os usuários cadastrados. Usado pela rotina em background, que precisa processar
        /// cada usuário no seu próprio contexto (não existe "o" usuário fora de uma requisição HTTP).
        /// </summary>
        Task<IList<User>> GetAll();
        Task<int> Save(User user);
        Task<int> Edit(User user);
    }
}
