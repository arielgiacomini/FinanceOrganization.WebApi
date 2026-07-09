namespace Application.Feature.Account.DeleteAccount
{
    public interface IDeleteAccountHandler
    {
        Task<DeleteAccountOutput> Handle(DeleteAccountInput input, CancellationToken cancellationToken = default);
    }
}
