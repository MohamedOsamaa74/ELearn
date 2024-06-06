using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.AuthDTOs
{
    public class AuthDTO
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public required string Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string Nationality { get; set; }
        public required string Religion { get; set; }
        public required string Faculty { get; set; }
        public required string NId { get; set; }
        public required string Department { get; set; }
        public required string Grade { get; set; }
        public required string Role { get; set; }
        public required string Token { get; set; }
        //public DateTime ExpiresOn { get; set; }
        public IEnumerable<string> Errors { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
