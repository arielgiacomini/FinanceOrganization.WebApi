namespace Application.Feature.Date.SearchAllWithFilters
{
    public class SearchDateAllWithFiltersOutput
    {
        /// <summary>
        /// Data
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Ano
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Mês (1-12)
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Dia (1-31)
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// Mês por extenso (Janeiro, Fevereiro, etc)
        /// </summary>
        public string? MonthName { get; set; }
        /// <summary>
        /// Mês e Ano (Janeiro/2020, Fevereiro/2020, etc)
        /// </summary>
        public string? MonthYear { get; set; }
        /// <summary>
        /// Trimestre of the year (1-4)
        /// </summary>
        public int Trimester { get; set; }
        /// <summary>
        /// Nome do dia da semana (Segunda, Terça, etc)
        /// </summary>
        public string? DayWeekName { get; set; }
        /// <summary>
        /// Dia da Semana (1-7)
        /// </summary>
        public int DayWeek { get; set; }
        /// <summary>
        /// Semana do Ano (1-53)
        /// </summary>
        public int WeekYear { get; set; }
        /// <summary>
        /// Dia do Ano (1-366)
        /// </summary>
        public int DayYear { get; set; }
        /// <summary>
        /// É fim de semana?
        /// </summary>
        public bool IsWeekend { get; set; }
        /// <summary>
        /// É feriado?
        /// </summary>
        public bool IsHoliday { get; set; }
        /// <summary>
        /// Nome do Feriado
        /// </summary>
        public string? HolidayName { get; set; }
    }
}