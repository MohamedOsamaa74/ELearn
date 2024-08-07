﻿namespace ELearn.Application.DTOs.AnnouncementDTOs
{
    public class ViewAnnouncementDTO
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public required string UserName { get; set; }
        public required DateTime CreationDate { get; set; }
        public required ICollection<int> Groups { get; set; }
        public ICollection<string>? FilesUrls { get; set; } = [];
        public string? UserProfilePictureUrl { get; set; }
    }
}
