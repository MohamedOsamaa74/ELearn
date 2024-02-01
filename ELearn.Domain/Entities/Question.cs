namespace ELearn.Domain.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public required string Text { get; set; }
        public char CorrectOption { get; set; }
        public required ICollection<Option> Option { get; set; }
        //one to many (questions)
        public int? QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }
        public int? SurveyId { get; set; }
        public virtual Survey? Survey { get; set; }
        public virtual Voting? Voting { get; set; }
        public ICollection<UserQuestion>? UserQuestion { get; set; }
        public ICollection<ApplicationUser> ApplicationUser { get; set; }

    }
}