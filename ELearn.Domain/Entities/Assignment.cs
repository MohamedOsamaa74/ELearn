using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int? Grade { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();
        public required DateTime End { get; set; }
        public bool IsActive { get => CreationDate <= DateTime.UtcNow.ToLocalTime() && End >= DateTime.UtcNow.ToLocalTime(); }

        #region ForeignKeys
        public required string UserId { get; set; }
        public int GroupId { get; set; }
        #endregion

        #region NavigationProperties
        public virtual ApplicationUser User { get; set; }
        //many tasks in one group
        public virtual Group Group { get; set; }
        //many to many task 
        public List<UserAnswerAssignment> UserAssignment { get; set; }
        public ICollection<ApplicationUser> users { get; set; }
        public ICollection<FileEntity> Files { get; } = new List<FileEntity>();
        #endregion
    }
}