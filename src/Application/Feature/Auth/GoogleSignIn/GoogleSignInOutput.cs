namespace Application.Feature.Auth.GoogleSignIn
{
    public class GoogleSignInOutput
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}
