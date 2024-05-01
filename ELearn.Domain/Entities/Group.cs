using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();
        public required string Description { get; set; }
        public required string CreatorId { get; set; }
        public int DepartmentId { get; set; }

        public int? ParentGroupId { get; set; }
        //for create groups
        public virtual ApplicationUser User { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }
        public ICollection<Survey> Surveys { get; set; }
        public ICollection<GroupSurvey> GroupSurvey { get; set; }
        public ICollection<Voting> votings { get; set; }
        public ICollection<GroupVoting> GroupVoting { get; set; }

        //for self relation (subgroups)
        public virtual Group? ParentGroup { get; set; }
        public virtual ICollection<Group>? SubGroups { get; set; }= new HashSet<Group>();

        //user in groups(m to m)
        public virtual ICollection<ApplicationUser>? UsersInGroup { get; set; }=new HashSet<ApplicationUser>();
        public virtual ICollection<UserGroup>? UserGroups { get; set; }= new HashSet<UserGroup>();

        //many tasks in one group
        public virtual ICollection<Assignment>? Assignments { get; set; }

        //many announcement in many groups
        public virtual ICollection<Announcement>? AnnouncementsOfGroups { get; set; }
        public virtual ICollection<GroupAnnouncment>? GroupAnnouncments { get; set; }

        //dept has many groups
        public virtual Department Department { get; set; }

    }
}