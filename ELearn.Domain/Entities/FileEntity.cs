﻿namespace ELearn.Domain.Entities
{
    public class FileEntity
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string FileName { get; set; }
        public required string FolderName { get; set; }
        public required string FilePath { get; set; }
        public required string ViewUrl { get; set; }
        public required string DownloadUrl { get; set; }
        public required string Type { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();

        #region ForeignKeys
        public required string CreatorId { get; set; }
        public string? UserId { get; set; }
        public int? MaterialId { get; set; }
        public int? AnnouncementId { get; set; }
        public int? AssignmentId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public int? QuestionId { get; set; }
        public int? MessageId { get; set; }
        public int? UserAssignementId { get; set; }
        #endregion

        #region NavigationProperties
        public ApplicationUser Creator { get; set; }
        public ApplicationUser? User { get; set; }
        public Material? Material { get; set; }
        public Announcement? Announcement { get; set; }
        public Assignment? Assignment { get; set; }
        public Post? Post { get; set; }
        public Comment? Comment { get; set; }
        public Question? Question { get; set; }
        public Message? Message { get; set; }
        public UserAnswerAssignment? UserAssignment { get; set; }
        #endregion
    }
}