using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.AssignmentDTOs
{
    public class UploadAssignmentDTO
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int? Grade { get; set; }
        public required int GroupId { get; set; }
        public required DateTime End { get; set; }
        public ICollection<IFormFile>? Attachements { get; set; }
    }
}
