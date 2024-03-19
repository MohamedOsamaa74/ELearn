using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Option
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Text { get; set; }
        public int? QuestionId {  get; set; }
        public int? SurveyId { get; set; }
        public int? VotingId { get; set; }
        public virtual Question? Question { get; set; }
        public virtual Survey? Survey { get; set; }
        public virtual Voting? Voting { get; set; }
    }
}