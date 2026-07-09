namespace Application.Feature.Auth.GenerateToken
{
    public class GenerateTokenOutput
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}