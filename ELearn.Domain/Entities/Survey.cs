using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Survey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Text { get; set; }
        public required ICollection<Option> Options { get; set; } = new HashSet<Option>();
        public DateTime Date { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<UserSurvey> UserSurvey { get; set; }

        public ICollection<ApplicationUser> user { get; set; } = new List<ApplicationUser>();
        public ICollection<GroupSurvey> GroupSurvey { get; set; }= new HashSet<GroupSurvey>();
        public ICollection<Group> Group { get; set; } = new HashSet<Group>();
        public virtual ICollection<Question> Question { get; set; } = new HashSet<Question>();  

    }
}