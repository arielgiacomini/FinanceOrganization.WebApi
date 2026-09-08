namespace Domain.Interfaces
{
    /// <summary>
    /// Identifica o usuário dono da requisição HTTP atual, a partir da claim "sub" do
    /// token JWT validado. É a única fonte de verdade usada pelo filtro global de
    /// isolamento por usuário (ver FinanceOrganizationContext.OnModelCreating).
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Id do usuário autenticado, ou null quando não há usuário autenticado
        /// (requisição anônima, ou token cujo "sub" não representa um usuário).
        /// Nunca deve ser lido do body/query da requisição, só do token.
        /// </summary>
        Guid? UserId { get; }
    }
}
