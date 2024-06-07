namespace Domain.Options
{
    public class BillToPayOptions
    {
        public int HowManyYearsForward { get; set; }
        public int HowManyMonthForward { get; set; }
        public RoutineWorker RoutineWorker { get; set; } = new RoutineWorker();
    }
}