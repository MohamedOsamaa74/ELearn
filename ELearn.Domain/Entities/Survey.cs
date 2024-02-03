namespace ELearn.Domain.Entities
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public required string Text { get; set; }
        public required ICollection<Option> Options { get; set; } = new HashSet<Option>();
        public DateTime Date { get; set; }
        public required Duration Duration { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<UserSurvey> UserSurvey { get; set; }

        public ICollection<ApplicationUser> user { get; set; } = new List<ApplicationUser>();
        public ICollection<GroupSurvey> GroupSurvey { get; set; }= new HashSet<GroupSurvey>();
        public ICollection<Group> Group { get; set; }=new HashSet<Group>();
        public virtual ICollection<Question> Question { get; set; }= new HashSet<Question>();  

    }
}