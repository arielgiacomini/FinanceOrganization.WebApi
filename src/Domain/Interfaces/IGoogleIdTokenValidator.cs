using Domain.Entities.Extern;

namespace Domain.Interfaces
{
    /// <summary>
    /// Valida um id_token emitido pelo Google Identity Services (assinatura, emissor e audience).
    /// </summary>
    public interface IGoogleIdTokenValidator
    {
        /// <summary>
        /// Retorna os dados do usuário se o token for válido, ou null se for inválido/expirado/audience errada.
        /// </summary>
        Task<GoogleUserInfo?> ValidateAsync(string idToken);
    }
}
