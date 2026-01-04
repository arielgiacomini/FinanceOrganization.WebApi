using Domain.Interfaces;

namespace Application.Feature.Date.SearchMonthYear
{
    public class SearchMonthYearHandler : ISearchMonthYearHandler
    {
        private readonly IDimDateRepository _dimDateRepository;

        public SearchMonthYearHandler(IDimDateRepository dimDateRepository)
        {
            _dimDateRepository = dimDateRepository;
        }

        public async Task<SearchMonthYearOutput> Handle(SearchMonthYearInput input, CancellationToken cancellationToken)
        {
            var filters = await Filters(input);

            var output = new SearchMonthYearOutput
            {
                MonthYears = filters
            };

            return output;
        }

        /// <summary>
        /// Filtros Gerais
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<IList<string>> Filters(SearchMonthYearInput input)
        {
            if (input.StartYear.HasValue && input.EndYear.HasValue)
            {
                return await _dimDateRepository
                    .GetByStartEndYearGroupedByMonthYear(input.StartYear.Value, input.EndYear.Value);
            }
            else
            {
                return await _dimDateRepository
                    .GetAllGroupedByMonthYear();
            }
        }
    }
}