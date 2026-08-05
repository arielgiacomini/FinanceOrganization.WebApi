using Domain.Interfaces;

namespace Application.Feature.RealEstateFinancing.SearchRealEstateFinancing
{
    public class SearchRealEstateFinancingHandler : ISearchRealEstateFinancingHandler
    {
        private readonly IFinanciamentoImobiliarioRuaPascoalDias263Repository _repository;
        public SearchRealEstateFinancingHandler(IFinanciamentoImobiliarioRuaPascoalDias263Repository repository)
        {
            _repository = repository;
        }

        public async Task<Output<SearchRealEstateFinancingOutput>> Handle()
        {
            var financing = await _repository.GetAllInstallments();

            if (financing == null)
            {
                return Output<SearchRealEstateFinancingOutput>.NoContent("Não foi encontrado registros.");
            }

            return Output<SearchRealEstateFinancingOutput>.Success("", financing, financing.Count);
        }
    }
}
