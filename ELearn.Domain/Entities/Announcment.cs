using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Announcement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public required string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Group>? GroupsOfAnnouncement { get; set; }
        public virtual ICollection<GroupAnnouncment>? GroupAnnouncements { get; set; }
    }
}