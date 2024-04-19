using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class GroupVoting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region ForeignKeys
        public required int GroupId { get; set; }
        public required int VotingId { get; set; }
        #endregion

        #region Relations
        public virtual Group Group { get; set; }
        public virtual Voting Voting { get; set; }
        #endregion
    }
}