using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.MaterialDTOs
{
    public class UpdateMaterialDTO
    {

        public string Title { get; set; }
        public string? Link { get; set; }
        public int Week { get; set; }
        public IFormFile? File { get; set; }
    }
}
