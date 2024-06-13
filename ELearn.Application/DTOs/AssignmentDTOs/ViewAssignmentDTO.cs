using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.AssignmentDTOs
{
    public class ViewAssignmentDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string CreatorName { get; set; }
    }
}
