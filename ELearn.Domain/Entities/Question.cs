using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Text { get; set; }
        public required int Grade { get; set; }
        public required string Option1 { get; set; }
        public required string Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Option5 { get; set; }
        public string? CorrectOption { get; set; }

        #region ForeignKeys
        public int? QuizId { get; set; }
        public int? SurveyId { get; set; }
        #endregion

        #region NavigationProberty
        //public ICollection<Option> Options { get; set; } = new HashSet<Option>();
        public virtual Quiz? Quiz { get; set; }
        public virtual Survey? Survey { get; set; }
        public FileEntity? File { get; set; }
        public ICollection<UserAnswerQuestion>? UserQuestion { get; set; } = new HashSet<UserAnswerQuestion>();
        public ICollection<ApplicationUser> ApplicationUser { get; set; } = new HashSet<ApplicationUser>();
        #endregion
    }
}