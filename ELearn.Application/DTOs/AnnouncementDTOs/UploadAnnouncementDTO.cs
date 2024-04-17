using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ELearn.Application.DTOs.AnnouncementDTOs
{
    public class UploadAnnouncementDTO
    {
        [Required]
        public required string Text { get; set; }
        [Required]
        public required IEnumerable<int> Groups { get; set; }
        [AllowNull]
        public ICollection<IFormFile>? Files { get; set; }
    }
}