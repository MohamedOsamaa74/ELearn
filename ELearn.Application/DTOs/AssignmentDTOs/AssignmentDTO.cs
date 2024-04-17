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
    public class AssignmentDTO
    {
        public required string Title { get; set; }
        public required DateTime Date { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }

        public required IFormFile File { get; set; }
    }
}
