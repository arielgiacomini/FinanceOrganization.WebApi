namespace Application.Feature.CashReceivableRegistration.CreateCashReceivableRegistration
{
    public interface ICreateCashReceivableRegistrationHandler
    {
        Task<CreateCashReceivableRegistrationOutput> Handle(CreateCashReceivableRegistrationInput input, CancellationToken cancellationToken);
    }
}