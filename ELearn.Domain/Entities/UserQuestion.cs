using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class UserQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public DateTime DateAnswered { get; set; }
        public virtual Option Option { get; set; }
        public virtual Question Question { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}