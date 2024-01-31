namespace ELearn.Domain.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public DateTime Date { get; set; }
        public required Duration Duration { get; set; }
        //GroupId
        //UserId
        public int UserId { get; set; }//CreatorId

        public virtual required ApplicationUser User { get; set; }
    }
}