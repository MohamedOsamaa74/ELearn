namespace ELearn.Application.DTOs.FileDTOs
{
    public class FileDTO
    {
        public required string Title { get; set; }
        public required string FileName { get; set; }
        public required string FolderName { get; set; }
        public required string FilePath { get; set; }
        public required string ViewUrl { get; set; }
        public required string DownloadUrl { get; set; }
        public required string Type { get; set; }
        public required string UserId { get; set; }
        public DateTime Creation { get; set; }
    }
}