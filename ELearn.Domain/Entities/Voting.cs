using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class Voting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public required string CreatorId { get; set; }
        public ICollection<Group> Group { get; set; }
        public ICollection<Option> Options { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<UserVoting> UserVoting { get; set; }
        public ICollection<ApplicationUser> user { get; set; }
        public ICollection<GroupVoting> GroupVoting { get; set; }
    }
}