using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public required string OldPassword { get; set; }
        [Required]
        public required string NewPassword { get; set; }
    }
}
