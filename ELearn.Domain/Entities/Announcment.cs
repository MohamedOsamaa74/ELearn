using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public required string Text { get; set; }

        public required int UserId { get; set; }

        //public virtual User User { get; set; }
    }
}