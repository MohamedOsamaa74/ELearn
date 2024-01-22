namespace ELearn.Domain.Entities
{
    public class QuestionQuiz
    {
        public int QuestionId { get; set; }
        public required string QuizId { get; set; }
        public required Question Question { get; set; }
        public required Quiz Quiz { get; set; }
    }
}