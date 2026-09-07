namespace Domain.Options
{
    public class AuthClientOptions
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        /// <summary>
        /// Usuário dono dos tokens emitidos via grant_type=client_credentials, usado apenas durante a
        /// transição do frontend legado (que ainda não autentica pessoas) para o login real por usuário.
        /// Sem isso configurado, um token client_credentials não enxerga nenhum dado (não representa ninguém).
        /// </summary>
        public Guid? OwnerUserId { get; set; }
    }
}