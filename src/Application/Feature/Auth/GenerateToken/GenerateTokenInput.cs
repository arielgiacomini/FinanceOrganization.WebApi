namespace Application.Feature.Auth.GenerateToken
{
    public class GenerateTokenInput
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}