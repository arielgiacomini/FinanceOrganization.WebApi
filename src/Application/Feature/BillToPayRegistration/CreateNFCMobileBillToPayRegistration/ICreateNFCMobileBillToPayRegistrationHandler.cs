namespace Application.Feature.BillToPayRegistration.CreateCreditCardNFCMobileBillToPayRegistration
{
    public interface ICreateNFCMobileBillToPayRegistrationHandler
    {
        Task<CreateNFCMobileBillToPayRegistrationOutput> Handle(CreateNFCMobileBillToPayRegistrationInput input, CancellationToken cancellationToken = default);
    }
}