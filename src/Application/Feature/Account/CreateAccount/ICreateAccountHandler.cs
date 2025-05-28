
namespace Application.Feature.Account.CreateAccount
{
    public interface ICreateAccountHandler
    {
        Task<CreateAccountOutput> Handle(CreateAccountInput input, CancellationToken cancellationToken = default);
    }
}