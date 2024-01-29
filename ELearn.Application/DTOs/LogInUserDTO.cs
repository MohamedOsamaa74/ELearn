using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class LogInUserDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
