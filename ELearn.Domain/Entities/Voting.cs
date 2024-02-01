namespace ELearn.Domain.Entities
{

    public class Voting
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public required Duration Duration { get; set; }
        public required ICollection<Option> Option { get; set; }
        //UserId
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<UserVoting> UserVoting { get; set; }
        public ICollection<ApplicationUser> user { get; set; }

        public ICollection<GroupVoting> GroupVoting { get; set; }
        public ICollection<Group> Group { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}