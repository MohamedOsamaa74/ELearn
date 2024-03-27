using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class ForgetPasswordDTO
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string OTP { get; set; }

    }
}
