namespace Application.Feature.CashReceivableRegistration.DisableCashReceivableRegistration
{
    public interface IDisableCashReceivableRegistrationHandler
    {
        Task<DisableCashReceivableRegistrationOutput> Handle(DisableCashReceivableRegistrationInput input, CancellationToken cancellationToken = default);
    }
}