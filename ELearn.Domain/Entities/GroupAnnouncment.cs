using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class GroupAnnouncment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int AnnouncementId { get; set; }
        public virtual Group Group { get; set; }
        public virtual Announcement Announcement { get; set; }
    }
}