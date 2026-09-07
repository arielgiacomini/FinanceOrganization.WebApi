namespace Domain.Options
{
    public class GoogleAuthOptions
    {
        /// <summary>
        /// OAuth Client ID do Google usado para validar a audience do id_token recebido do frontend.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;
    }
}
