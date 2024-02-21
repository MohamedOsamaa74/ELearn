using ELearn.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class AssignmentDTO
    {
        public required string Title { get; set; }
        public required DateTime Date { get; set; }
        public required Duration Duration { get; set; }
        public required IFormFile File { get; set; }

    }
}
