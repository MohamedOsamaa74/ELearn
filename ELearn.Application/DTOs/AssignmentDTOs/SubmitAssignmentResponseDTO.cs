using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.AssignmentDTOs
{
    public class SubmitAssignmentResponseDTO
    {
        public int AssignmentId { get; set; }
        public required IFormFile file { get; set; }
    }
}
