using Domain.Entities.Extern;
using Domain.Interfaces;

namespace Application.Feature.Date.SearchAllWithFilters
{
    public class SearchDateAllWithFiltersHandler : ISearchDateAllWithFiltersHandler
    {
        private readonly IDimDateRepository _dimDateRepository;

        public SearchDateAllWithFiltersHandler(IDimDateRepository dimDateRepository)
        {
            _dimDateRepository = dimDateRepository;
        }

        public async Task<IList<SearchDateAllWithFiltersOutput>> Handle(SearchDateAllWithFiltersInput input, CancellationToken cancellationToken)
        {
            var filters = await Filters(input);

            return filters.Select(d => new SearchDateAllWithFiltersOutput
            {
                Date = d.Date,
                Year = d.Year,
                Month = d.Month,
                Day = d.Day,
                MonthName = d.MonthName,
                MonthYear = d.MonthYear,
                Trimester = d.Trimester,
                DayWeekName = d.DayWeekName,
                DayWeek = d.DayWeek,
                WeekYear = d.WeekYear,
                DayYear = d.DayYear,
                IsWeekend = d.IsWeekend,
                IsHoliday = d.IsHoliday,
                HolidayName = d.HolidayName
            }).ToList();
        }

        /// <summary>
        /// Filtros Gerais
        /// </summary>
        /// <param name="dimDatesByDb"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<IList<DimDate>> Filters(SearchDateAllWithFiltersInput input)
        {
            if (input.Date.HasValue)
            {
                return await FilterByDate(input.Date.Value);
            }
            else if (input.Year.HasValue)
            {
                return await FilterByYear(input.Year.Value);
            }
            else if (input.MonthYear != null)
            {
                return await _dimDateRepository.GetByMonthYear(input.MonthYear);
            }
            else
            {
                return await _dimDateRepository.GetAllAsync();
            }
        }

        /// <summary>
        /// Filtro por Data
        /// </summary>
        /// <param name="input"></param>
        /// <param name="listDimDate"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<IList<DimDate>> FilterByDate(DateTime date)
        {
            return await _dimDateRepository.GetByDate(date);
        }

        /// <summary>
        /// Filtro por Ano
        /// </summary>
        /// <param name="input"></param>
        /// <param name="listDimDate"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<IList<DimDate>> FilterByYear(int year)
        {
            return await _dimDateRepository.GetByYear(year);
        }
    }
}