namespace ELearn.Domain.Entities
{
    public class GroupSurvey
    {
        // public int GroupsurveyId { get; set; }
        public required int GroupId { get; set; }
        public Group Group { get; set; }
        public required int SurveyId { get; set; }
        
        public  Survey Survey { get; set; }

    }  
}