using ELearn.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.ReactDTOs
{
    public class ReactDTO
    {
        public required string Parent { get; set; }
        public int ParentId { get; set; }
        public string? FirstName { get; set; }
    }
}
