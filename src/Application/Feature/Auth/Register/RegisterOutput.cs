namespace Application.Feature.Auth.Register
{
    public class RegisterOutput
    {
        public OutputBaseDetails Output { get; set; } = new OutputBaseDetails();
        public string? AccessToken { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}
