using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Group
    {
        public int GroupId { get; set; }
        public required string GroupName { get; set; }
        public DateTime CreationDate { get; set; }
        public required string Description { get; set; }

        public int CreatorId { get; set; }
        public int ParentGroupIdId { get; set; }

        //public virtual User User { get; set; }
        public virtual Group? group { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }

    }
}