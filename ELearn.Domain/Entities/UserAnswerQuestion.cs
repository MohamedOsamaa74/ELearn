using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class UserAnswerQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateAnswered => DateTime.UtcNow.ToLocalTime();

        #region ForeignKeys
        public required string UserId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        #endregion

        #region NavigationProperties
        public virtual Option Option { get; set; }
        public virtual Question Question { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        #endregion
    }
}