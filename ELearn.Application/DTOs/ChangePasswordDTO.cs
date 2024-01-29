using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class ChangePasswordDTO
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
