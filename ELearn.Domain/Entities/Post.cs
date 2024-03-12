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
        public DateTime Date { get; set; }

        public string UserId { get; set; }//CreatorId

        public virtual required ApplicationUser User { get; set; }

        public ICollection<React>? Reacts { get; set; }
        public ICollection<Comment>? Comments { get; set; }

    }
}
