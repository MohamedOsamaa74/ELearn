using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class Quiz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string title { get; set; }
        public required int Grade { get; set; }
        public DateTime CreationDate => DateTime.UtcNow.ToLocalTime();
        public bool IsActive { get => Start <= DateTime.UtcNow.ToLocalTime() && End >= DateTime.UtcNow.ToLocalTime(); }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public bool IsActive => Start <= DateTime.Now && End >= DateTime.Now;

        #region ForeignKeys
        public required int GroupId { get; set; }
        public required string UserId { get; set; }
        #endregion

        #region Relations
        public virtual Group Group { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual required ICollection<Question> Questions { get; set; } = [];
        public virtual ICollection<UserAnswerQuiz> UserQuiz { get; set; } = [];
        #endregion
    }
}