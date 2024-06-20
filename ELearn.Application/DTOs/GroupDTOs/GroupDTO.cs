using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.GroupDTOs
{
    public class GroupDTO
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? CreatorId { get; set; }
        public string? InstructorName { get; set; }
        public required int DepartmentId { get; set; }

        //public DateTime CreateDate => DateTime.UtcNow.ToLocalTime();
    }
}