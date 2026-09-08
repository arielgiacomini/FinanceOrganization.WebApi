namespace Application.Feature.Auth.PasswordGrant
{
    public class PasswordGrantOutput
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}
