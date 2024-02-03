namespace ELearn.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public char? CorrectOption { get; set; }
        public required ICollection<Option> Options { get; set; }=new HashSet<Option>();
        //one to many (questions)
        public int? QuizId { get; set; }
        public int? SurveyId { get; set; }
        public int? VotingId { get; set; }
        public virtual Quiz? Quiz { get; set; }
        public virtual Survey? Survey { get; set; }
        public virtual Voting? Voting { get; set; }
        public ICollection<UserQuestion>? UserQuestion { get; set; } =new HashSet<UserQuestion>();
        public ICollection<ApplicationUser> ApplicationUser { get; set; } = new HashSet<ApplicationUser>();

    }
}