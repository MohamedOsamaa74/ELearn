namespace ELearn.Domain.Entities
{
    public class GroupSurvey
    {
        // public int GroupsurveyId { get; set; }
        public required int GroupId { get; set; }
        public required int SyrveyId { get; set; }
        public virtual Group Group { get; set; }
        public virtual Voting Survey { get; set; }

    }  
}