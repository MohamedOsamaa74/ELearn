using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.DTOs.AnnouncementDTOs
{
    public class ViewAnnouncementDTO
    {
        public required string Text { get; set; }
        public required string UserId { get; set; }
        public required DateTime CreationDate { get; set; }
        public required ICollection<int> Groups { get; set; }
        public ICollection<string>? FilesUrls { get; set; } = [];
    }
}
