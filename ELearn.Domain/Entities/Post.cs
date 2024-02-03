namespace ELearn.Domain.Entities
{
    public class Post
    {

        public int PostId { get; set; }
        public required string PostText { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }//CreatorId

        public virtual required ApplicationUser User { get; set; }

        public ICollection<React>? Reacts { get; set; }
        public ICollection<Comment>? Comments { get; set; }

    }
}
