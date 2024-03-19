  using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class AddMaterialDTO
    {
        public required string Title { get; set; }
        public int Week { get; set; }
        public required IFormFile File { get; set; }
    }
}
