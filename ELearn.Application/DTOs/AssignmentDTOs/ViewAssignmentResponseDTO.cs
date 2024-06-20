using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.AssignmentDTOs
{
    public class ViewAssignmentResponseDTO
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        public DateOnly UploadDate { get; set; }
        public TimeOnly UploadTime { get; set; }
        public string? Mark { get; set; }
        public required string FileURL { get; set; }
    }
}
