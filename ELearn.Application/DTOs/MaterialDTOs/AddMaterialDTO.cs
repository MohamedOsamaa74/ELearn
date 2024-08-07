﻿using ELearn.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.MaterialDTOs
{
    public class AddMaterialDTO
    {
        public int Week { get; set; }
        public required IFormFile File { get; set; }
        public required MaterialType Type { get; set; }
    }
}
