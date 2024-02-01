namespace ELearn.Domain.Entities
{
    [NotMapped]
    public class Duration
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan CalculateDuration()
        {
            return EndTime - StartTime;
        }
    }
}