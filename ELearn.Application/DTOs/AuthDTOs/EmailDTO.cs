using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.AuthDTOs
{
    public class EmailDTO
    {
        public required string Email { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public IList<IFormFile>? Attachements { get; set; }
    }
}
