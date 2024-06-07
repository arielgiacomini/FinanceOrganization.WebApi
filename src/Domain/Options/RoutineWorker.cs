namespace Domain.Options
{
    public class RoutineWorker
    {
        public bool Enable { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool AddDays { get; set; }
    }
}