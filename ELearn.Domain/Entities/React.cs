using ELearn.Domain.Enum;


namespace ELearn.Domain.Entities
{
    public class React
    {
        public int ReactId { get; set; }
        public int PostID { get; set; }
        public string UserID { get; set; }
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Date { get; set; }
        public required ReactType Type { get; set; }
        

    }
}