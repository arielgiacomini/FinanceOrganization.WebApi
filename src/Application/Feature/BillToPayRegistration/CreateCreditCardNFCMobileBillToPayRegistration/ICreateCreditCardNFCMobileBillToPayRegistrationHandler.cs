namespace Application.Feature.BillToPayRegistration.CreateCreditCardNFCMobileBillToPayRegistration
{
    public interface ICreateCreditCardNFCMobileBillToPayRegistrationHandler
    {
        Task<CreateCreditCardNFCMobileBillToPayRegistrationOutput> Handle(CreateCreditCardNFCMobileBillToPayRegistrationInput input, CancellationToken cancellationToken = default);
    }
}