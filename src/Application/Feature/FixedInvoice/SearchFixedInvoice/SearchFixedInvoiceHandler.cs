using Domain.Interfaces;

namespace Application.Feature.FixedInvoice.SearchFixedInvoice
{
    public class SearchFixedInvoiceHandler : ISearchFixedInvoiceHandler
    {
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;

        public SearchFixedInvoiceHandler(IFixedInvoiceRepository fixedInvoiceRepository)
        {
            _fixedInvoiceRepository = fixedInvoiceRepository;
        }

        public async Task<SearchFixedInvoiceOutput> Handle(CancellationToken cancellationToken = default)
        {
            var getByAll = await _fixedInvoiceRepository.GetAll();

            var output = new SearchFixedInvoiceOutput
            {
                Output = OutputBaseDetails.Success("Busca realizada com sucesso.", getByAll, getByAll.Count)
            };

            return output;
        }
    }
}