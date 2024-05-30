namespace Application.Feature.BillToPay.SearchMonthlyAverageAnalysis
{
    public class SearchMonthlyAverageAnalysisInput
    {
        /// <summary>
        /// Data Inicial da Análise
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Data Final da Análise
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Quantidade de Meses que serão analisados para trazer as médias
        /// </summary>
        public int QuantityMonthsAnalysis { get; set; }
    }
}