using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public required string Text { get; set; }


        public required int UserId { get; set; }
        public virtual required ApplicationUser User { get; set; }

        //many announcement in many groups
        public virtual ICollection<Group> GroupsOfAnnouncement { get; set; }
        public virtual ICollection<GroupAnnouncment> GroupAnnouncements { get; set; }


    }
}