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
    public class UserVotingConfiguration : IEntityTypeConfiguration<UserVoting>
    {
        public void Configure(EntityTypeBuilder<UserVoting> builder)
        {
            builder.HasKey(us => new { us.userId, us.VotingId });

            builder
                .HasOne(us => us.User)
                .WithMany(u => u.UserVoting)
                .HasForeignKey(us => us.userId);

            builder
                .HasOne(us => us.Voting)
                .WithMany(s => s.UserVoting)
                .HasForeignKey(us => us.VotingId);
        }
    }
}
