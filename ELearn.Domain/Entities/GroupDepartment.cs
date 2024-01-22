using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class GroupDepartment
    {
        public int GroupDepartmentId { get; set; }
        public required int GroupId { get; set; }
        public required int DepartmentId { get; set; }
        public virtual Group? Group { get; set; }
        //public virtual Department Department { get; set; }
    }
}