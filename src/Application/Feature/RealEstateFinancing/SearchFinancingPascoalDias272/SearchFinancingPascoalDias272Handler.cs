using Domain.Interfaces;

namespace Application.Feature.RealEstateFinancing.SearchFinancingPascoalDias272
{
    public class SearchFinancingPascoalDias272Handler : ISearchFinancingPascoalDias272Handler
    {
        private readonly IFinanciamentoImobiliarioRuaPascoalDias272Repository _repository;

        public SearchFinancingPascoalDias272Handler(IFinanciamentoImobiliarioRuaPascoalDias272Repository repository)
        {
            _repository = repository;
        }

        public async Task<Output<SearchFinancingPascoalDias272Output>> Handle()
        {
            var financingRegistration = await _repository.GetFinancingList();

            if (financingRegistration == null)
            {
                return Output<SearchFinancingPascoalDias272Output>.NoContent("Não foi encontrado registros.");
            }

            return Output<SearchFinancingPascoalDias272Output>.Success("",  financingRegistration, financingRegistration.Count);
        }
    }
}