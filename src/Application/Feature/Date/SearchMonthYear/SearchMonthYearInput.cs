namespace Application.Feature.Date.SearchMonthYear
{
    public class SearchMonthYearInput
    {
        /// <summary>
        /// Ano inicial para filtro. Ex.: 2020
        /// </summary>
        public int? StartYear { get; set; }
        /// <summary>
        /// Ano final para filtro. Ex.: 2023
        /// </summary>
        public int? EndYear { get; set; }
    }
}