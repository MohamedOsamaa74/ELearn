using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs
{
    public class AnnouncementDTO
    {
        [Required]
        public required string text { get; set; }
        [Required]
        public required ICollection<int> Groups { get; set; }
        //public DateTime Time => DateTime.UtcNow.ToLocalTime();
    }
}
