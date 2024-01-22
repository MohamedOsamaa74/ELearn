using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class GroupVoting
    {
        //public int Id { get; set; }
        public required int GroupId { get; set; }
        public required int VotingId { get; set; }
        public virtual Group Group { get; set; }
        public virtual Voting Voting { get; set; }
    }
}