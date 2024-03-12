using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class GroupSurvey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required int GroupId { get; set; }
        public required int SurveyId { get; set; }
        public Group Group { get; set; }
        
        public  Survey Survey { get; set; }

    }  
}