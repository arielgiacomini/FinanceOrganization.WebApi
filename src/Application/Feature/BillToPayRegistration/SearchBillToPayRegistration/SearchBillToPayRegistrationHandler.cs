using Domain.Interfaces;

namespace Application.Feature.BillToPayRegistration.SearchBillToPayRegistration
{
    public class SearchBillToPayRegistrationHandler : ISearchBillToPayRegistrationHandler
    {
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;

        public SearchBillToPayRegistrationHandler(IBillToPayRegistrationRepository billToPayRegistrationRepository)
        {
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
        }

        public async Task<SearchBillToPayRegistrationOutput> Handle(CancellationToken cancellationToken = default)
        {
            var getByAll = await _billToPayRegistrationRepository.GetAll();

            var output = new SearchBillToPayRegistrationOutput
            {
                Output = OutputBaseDetails.Success("Busca realizada com sucesso.", getByAll, getByAll.Count)
            };

            return output;
        }
    }
}