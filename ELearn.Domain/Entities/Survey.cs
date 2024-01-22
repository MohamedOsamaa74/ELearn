namespace ELearn.Domain.Entities
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public required string Text { get; set; }
        public required ICollection<Option> Options { get; set; }
        public DateTime Date { get; set; }
        public required Duration Duration { get; set; }

    }
}