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
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public bool IsActive { get => Start <= DateTime.Now && End >= DateTime.Now; }
        public required string Option1 { get; set; }
        public required string Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Option5 { get; set; }

        #region ForeignKeys
        public required string CreatorId { get; set; }
        #endregion

        #region Relations
        public ICollection<Group> Group { get; set; }
        //public ICollection<Option> Options { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<UserAnswerVoting> UserVoting { get; set; }
        public ICollection<ApplicationUser> user { get; set; }
        public ICollection<GroupVoting> GroupVoting { get; set; }
        #endregion
    }
}