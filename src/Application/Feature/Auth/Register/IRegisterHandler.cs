namespace Application.Feature.Auth.Register
{
    public interface IRegisterHandler
    {
        Task<RegisterOutput> Handle(RegisterInput input);
    }
}
