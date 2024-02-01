using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Group
    {
        /// <summary>
        /// The relationship from 'Group.Creator' to 'ApplicationUser.CreatedGroups' with foreign key properties
        /// {'CreatorId' : int} cannot target the primary key {'Id' : string} because it is not compatible.
        /// Configure a principal key or a set of foreign key properties with compatible types for this relationship.
        /// </summary>
        public int GroupId { get; set; }
        public required string GroupName { get; set; }
        public DateTime CreationDate { get; set; }
        public required string Description { get; set; }

        //for create groups
        public required int CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }

        //public virtual User User { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }

       // public virtual Group? group { get; set; }
        public ICollection<Survey> Surveys { get; set; }
        public ICollection<GroupSurvey> GroupSurvey { get; set; }
        public ICollection<Voting> votings { get; set; }
        public ICollection<GroupVoting> GroupVoting { get; set; }

        //for self relation (subgroups)
        public int ParentGroupId { get; set; }
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
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

    }
}