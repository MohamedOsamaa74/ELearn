using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.AuthDTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public required string CurrentPassword { get; set; }
        [Required]
        public required string NewPassword { get; set; }
        [Required]
        public required string ConfirmPassword { get; set; }
    }
}
