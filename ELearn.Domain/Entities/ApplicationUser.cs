using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ELearn.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "National Id Required")]
        [StringLength(14, ErrorMessage = "National ID must be 14 Numbers.")]
        public int N_ID { get; set; }
        public required string PhoneNum { get; set; }
        public string? Image { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Address { get; set; }
        public required string Nationality { get; set; }
        public required string Religion { get; set; }
        public required string Grade { get; set; }
    }
}