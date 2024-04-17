using Microsoft.AspNetCore.Http;

namespace ELearn.Application.DTOs.FileDTOs
{
    public class UploadFileDTO
    {
        public required IFormFile File { get; set; }
        public required string FolderName { get; set; }
        public required int ParentId { get; set; }
    }
}
