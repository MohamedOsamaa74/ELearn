namespace ELearn.Domain.Entities
{
    public class UserQuestion
    {
        public int Id { get; set; }
        //QuestionId
        //UserId
        //OptionId
        public int OptionId { get; set; }
        public virtual Option Option { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}