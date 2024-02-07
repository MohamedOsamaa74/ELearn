using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Text { get; set; }


        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //many announcement in many groups
        public virtual ICollection<Group> GroupsOfAnnouncement { get; set; }
        public virtual ICollection<GroupAnnouncment> GroupAnnouncements { get; set; }


    }
}