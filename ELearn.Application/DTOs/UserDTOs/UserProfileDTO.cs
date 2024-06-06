using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.UserDTOs
{
    public class UserProfileDTO
    {
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Nationality { get; set; }
        public required string Relegion { get; set; }
        public required string Faculty { get; set; }
        public required string NId { get; set; }
        public required string Department { get; set; }
        public required string Level { get; set; }
        public required string ProfilePictureName { get; set; }
    }
}
