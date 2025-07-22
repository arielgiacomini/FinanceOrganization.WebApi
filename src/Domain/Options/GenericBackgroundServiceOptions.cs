namespace Domain.Options
{
    public class GenericBackgroundServiceOptions
    {
        /// <summary>
        /// Rotina de trabalho que será executada periodicamente.
        /// </summary>
        public RoutineWorker? RoutineWorker { get; set; }
    }
}