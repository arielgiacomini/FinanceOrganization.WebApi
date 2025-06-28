
namespace Application.Feature.BillToPayRegistration.RecordsAwaitingCompleteRegistration
{
    public interface IRecordsAwaitingCompleteRegistrationHandler
    {
        Task<RecordsAwaitingCompleteRegistrationOutput> Handle(RecordsAwaitingCompleteRegistrationInput input, CancellationToken cancellationToken);
    }
}