using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Text { get; set; }
        public required DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();
        public required string UserId { get; set; } //CreatorId
        public virtual ApplicationUser User { get; set; }
        public ICollection<FileEntity> Files { get; } = new List<FileEntity>();
        public ICollection<React>? Reacts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}