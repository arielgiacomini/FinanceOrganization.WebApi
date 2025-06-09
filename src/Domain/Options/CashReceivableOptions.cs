namespace Domain.Options
{
    public class CashReceivableOptions
    {
        /// <summary>
        /// Quantos anos no futuro?
        /// </summary>
        public int HowManyYearsForward { get; set; }
        /// <summary>
        /// Quantos meses no futuro?
        /// </summary>
        public int HowManyMonthForward { get; set; }
        public RoutineWorker RoutineWorker { get; set; } = new RoutineWorker();
    }
}