using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class Quiz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required int GroupId { get; set; }
        public required string UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public required string title { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public virtual Group Group { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual required ICollection<Question> Questions { get; set; }= new HashSet<Question>();
    }
}