namespace Application.Feature.Auth.GoogleSignIn
{
    public interface IGoogleSignInHandler
    {
        Task<GoogleSignInOutput> Handle(GoogleSignInInput input);
    }
}
