namespace Domain.Entities.Extern
{
    public class MonthlyAverageAnalysis
    {
        public string? Category { get; set; }
        public int? QuantityTotal { get; set; }
        public decimal? SumTotal { get; set; }
        public int? MonthQuantity { get; set; }
        public decimal? AvgPriceMonthly { get; set; }
        public int? AvgQuantityMonthly { get; set; }
        public string? FistDate { get; set; }
        public string? LastDate { get; set; }
    }
}