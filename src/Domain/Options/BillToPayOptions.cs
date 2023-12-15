namespace Domain.Options
{
    public class BillToPayOptions
    {
        public int HowManyYearsForward { get; set; }
        public int HowManyMonthForward { get; set; }
        public RoutineWorker RoutineWorker { get; set; } = new RoutineWorker();
    }

    public class RoutineWorker
    {
        public bool Enable { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool AddDays { get; set; }
    }
}