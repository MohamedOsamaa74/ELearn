using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Survey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Text { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public bool IsActive => Start <= DateTime.Now && End >= DateTime.Now;

        #region ForeignKeys
        public required string CreatorId { get; set; }
        #endregion

        #region Relations
        //public required ICollection<Option> Options { get; set; } = new HashSet<Option>();
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<UserAnswerSurvey> UserSurvey { get; set; } = [];

        public ICollection<ApplicationUser> User { get; set; } = [];
        public ICollection<GroupSurvey> GroupSurvey { get; set; } = [];
        public ICollection<Group> Group { get; set; } = [];
        public virtual ICollection<Question> Question { get; set; } = [];
        #endregion
    }
}