using ELearn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearn.InfraStructure.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.ToTable("Files");
            builder.HasKey(x => x.Id);

            #region Relations
            builder.HasOne(c => c.Comment)
                .WithOne(f => f.File)
                .HasForeignKey<FileEntity>(i => i.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(q => q.Question)
                .WithOne(f => f.File)
                .HasForeignKey<FileEntity>(f => f.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Message)
                .WithOne(f => f.File)
                .HasForeignKey<FileEntity>(f => f.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.User)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(M => M.Material)
                .WithMany(m => m.Files)
                .HasForeignKey(f => f.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Announcement)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.AnnouncementId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Assignment)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.UserAssignment)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.UserAssignementId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Post)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
