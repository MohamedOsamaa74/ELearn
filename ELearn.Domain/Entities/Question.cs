namespace ELearn.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public char CorrectOption { get; set; }
        public required ICollection<Option> Option { get; set; }
        //QuizId
    }
}