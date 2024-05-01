using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class UserAnswerVoting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();
        public required string Option { get; set; }

        #region ForeignKeys
        public required string UserId { get; set; }
        public int VotingId { get; set; }
        #endregion

        #region Relations
        public ApplicationUser User { get; set; }
        public Voting Voting { get; set; }
        //public Option Options { get; set; }
        #endregion
    }
}