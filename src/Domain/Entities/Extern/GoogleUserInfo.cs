namespace Domain.Entities.Extern
{
    /// <summary>
    /// Dados extraídos de um id_token do Google já validado.
    /// </summary>
    public class GoogleUserInfo
    {
        public string Subject { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
