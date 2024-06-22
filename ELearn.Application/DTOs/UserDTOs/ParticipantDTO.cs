using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.UserDTOs
{
    public class ParticipantDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Grade { get; set; }
        public required string UserName { get; set; }
        public required string DepartmentName { get; set; }
    }
}
