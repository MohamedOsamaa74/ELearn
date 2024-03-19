using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string title { get; set; }
        public virtual ICollection<ApplicationUser>? Users { get; set; }=new List<ApplicationUser>();

        //dept has many groups
        public virtual ICollection<Group>? GroupsOfDepartment { get; set; } =new HashSet<Group>();

    }
}