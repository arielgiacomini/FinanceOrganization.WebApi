namespace Application.Feature.BillToPayRegistration.SearchBillToPayRegistration
{
    public interface ISearchBillToPayRegistrationHandler
    {
        Task<SearchBillToPayRegistrationOutput> Handle(CancellationToken cancellationToken = default);
    }
}