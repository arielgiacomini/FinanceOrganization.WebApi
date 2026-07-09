namespace Application.Feature.Account.EditAccount
{
    public interface IEditAccountHandler
    {
        Task<EditAccountOutput> Handle(EditAccountInput input, CancellationToken cancellationToken = default);
    }
}
