namespace ELearn.Domain.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required DateTime Date { get; set; }
        public required Duration Duration { get; set; }
        //UserId
        public string UserId { get; set; }//CreatorId

        public virtual required ApplicationUser User { get; set; }

        //many tasks in one group
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        //many to many task 
        public List<UserAssignment> UserAssignment { get; set; }

        public ICollection<ApplicationUser> users { get; set; }

    }
}