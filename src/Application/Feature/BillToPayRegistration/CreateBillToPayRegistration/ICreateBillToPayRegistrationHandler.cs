namespace Application.Feature.BillToPayRegistration.CreateBillToPayRegistration;

public interface ICreateBillToPayRegistrationHandler
{
    Task<CreateBillToPayRegistrationOutput> Handle(CreateBillToPayRegistrationInput input, CancellationToken cancellationToken = default);
}