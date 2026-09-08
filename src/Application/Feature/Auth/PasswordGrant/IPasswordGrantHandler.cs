namespace Application.Feature.Auth.PasswordGrant
{
    public interface IPasswordGrantHandler
    {
        Task<PasswordGrantOutput> Handle(PasswordGrantInput input);
    }
}
