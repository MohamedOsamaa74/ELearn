namespace ELearn.Domain.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public required string Text { get; set; }
        public char CorrectOption { get; set; }
        public required ICollection<Option> Option { get; set; }
        //QuizId
        public int? QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }
    }
}