using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class AuthDTO
    {
        public required string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
        public required string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
