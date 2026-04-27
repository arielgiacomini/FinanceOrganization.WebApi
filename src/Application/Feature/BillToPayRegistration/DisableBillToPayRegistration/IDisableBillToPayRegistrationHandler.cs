namespace Application.Feature.BillToPayRegistration.DisableBillToPayRegistration
{
    public interface IDisableBillToPayRegistrationHandler
    {
        Task<DisableBillToPayRegistrationOutput> Handle(DisableBillToPayRegistrationInput input, CancellationToken cancellationToken = default);
    }
}