using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.PostDTOs
{
    public class CreatePostDTO
    {
        public required string Text { get; set; }
        public ICollection<IFormFile>? Files { get; set; }
    }
}
