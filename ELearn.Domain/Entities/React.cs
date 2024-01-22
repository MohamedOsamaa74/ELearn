using ELearn.Domain.Enum;


namespace ELearn.Domain.Entities
{
    public class React
    {
        //public int ReactId { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public required ReactType Type { get; set; }
        public virtual required Post Post { get; set; }
        public virtual required ApplicationUser User { get; set; }

    }
}