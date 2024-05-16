using ELearn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Configurations
{
    public class UserAnswerAssignmentConfiguration : IEntityTypeConfiguration<UserAnswerAssignment>
    {
        public void Configure(EntityTypeBuilder<UserAnswerAssignment> builder)
        {
            builder.ToTable("UserAnswerAssignments");
            builder.HasKey(ua => ua.Id);

            builder
                .HasOne(ua => ua.Users)
                .WithMany(u => u.UserAssignment)
                .HasForeignKey(ua => ua.UserId);

            builder
                .HasOne(ua => ua.Assignment)
                .WithMany(a => a.UserAssignment)
                .HasForeignKey(ua => ua.AssignmentId);

            builder.HasMany(ua => ua.Files)
                .WithOne(f => f.UserAssignment)
                .HasForeignKey(f => f.UserAssignementId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
