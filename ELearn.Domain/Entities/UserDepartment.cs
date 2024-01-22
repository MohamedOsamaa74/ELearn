using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class UserDepartment
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }


    }
}