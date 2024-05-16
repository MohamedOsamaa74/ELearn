using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.AuthDTOs
{
    public class VerifyOTPDTO
    {
        public required string Email { get; set; }
        public required string OTP { get; set; }
    }
}
