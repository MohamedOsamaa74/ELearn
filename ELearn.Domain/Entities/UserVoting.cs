using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class UserVoting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string userId { get; set; }
        public int VotingId { get; set; }
        public int OptionsId { get; set; }
        public DateTime DateAnswered { get; set; }
        public ApplicationUser User { get; set; }
        public Voting Voting { get; set; }
        public Option Options { get; set; }
    }
}