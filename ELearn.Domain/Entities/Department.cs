using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public required string title { get; set; }
        public virtual ICollection<ApplicationUser>? Users { get; set; }=new List<ApplicationUser>();

        //dept has many groups
        public virtual ICollection<Group>? GroupsOfDepartment { get; set; } =new HashSet<Group>();

    }
}