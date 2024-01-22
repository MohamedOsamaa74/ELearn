namespace ELearn.Domain.Entities
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public required string title { get; set; }
        public required Duration Duration { get; set; }
    }
}