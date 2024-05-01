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
    public class UserAnswerVotingConfiguration : IEntityTypeConfiguration<UserAnswerVoting>
    {
        public void Configure(EntityTypeBuilder<UserAnswerVoting> builder)
        {
            builder.ToTable("UserAnswerVotings");
            builder.HasKey(uv => uv.Id);
            builder
                .HasOne(us => us.User)
                .WithMany(u => u.UserVoting)
                .HasForeignKey(us => us.UserId);

            builder
                .HasOne(us => us.Voting)
                .WithMany(s => s.UserVoting)
                .HasForeignKey(us => us.VotingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
