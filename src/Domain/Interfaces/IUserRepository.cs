using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetById(Guid id);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByGoogleSub(string googleSub);
        /// <summary>
        /// Busca pelo hash (SHA-256, hex) da chave de Lançamento Rápido. Usado só pelo
        /// QuickCaptureKeyHandler para resolver o header X-Quick-Capture-Key num usuário.
        /// </summary>
        Task<User?> GetByQuickCaptureKeyHash(string quickCaptureKeyHash);
        /// <summary>
        /// Todos os usuários cadastrados. Usado pela rotina em background, que precisa processar
        /// cada usuário no seu próprio contexto (não existe "o" usuário fora de uma requisição HTTP).
        /// </summary>
        Task<IList<User>> GetAll();
        Task<int> Save(User user);
        Task<int> Edit(User user);
    }
}
