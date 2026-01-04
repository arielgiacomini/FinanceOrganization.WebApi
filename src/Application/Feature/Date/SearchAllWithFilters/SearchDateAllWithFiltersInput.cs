namespace Application.Feature.Date.SearchAllWithFilters
{
    public class SearchDateAllWithFiltersInput
    {
        /// <summary>
        /// Data
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Ano
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// Mês e Ano (Janeiro/2020, Fevereiro/2020, etc)
        /// </summary>
        public string? MonthYear { get; set; }
    }
}