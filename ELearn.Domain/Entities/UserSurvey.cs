using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class UserSurvey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  
        public required string UserId { get; set; }
        public int SurveyId { get; set; }
        public int OptionId { get; set; }
        public DateTime DateAnswered { get; set; }
        public Survey Survey { get; set; }
        public ApplicationUser User { get; set; }
        public Option Option { get; set; }
    }
}