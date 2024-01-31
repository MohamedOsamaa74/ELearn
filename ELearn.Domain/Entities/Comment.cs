namespace ELearn.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public DateTime Date { get; set; }
        //UserId
        public required ApplicationUser CreatorId { get; set; }



        //PostId
        public required int PostId { get; set; }
        public virtual required Post Post { get; set; }
    }
}
