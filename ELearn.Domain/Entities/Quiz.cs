namespace ELearn.Domain.Entities
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public required int GroupId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public required string title { get; set; }
        public Duration Duration { get; set; }
        public virtual Group Group { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}