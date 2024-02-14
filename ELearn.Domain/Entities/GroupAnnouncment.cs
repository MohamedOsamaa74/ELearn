namespace ELearn.Domain.Entities
{
    public class GroupAnnouncment
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int AnnouncementId { get; set; }
        public virtual Group Group { get; set; }
        public virtual Announcement Announcement { get; set; }
    }
}