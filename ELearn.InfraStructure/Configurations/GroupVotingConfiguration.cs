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
    public class GroupVotingConfiguration : IEntityTypeConfiguration<GroupVoting>
    {
        public void Configure(EntityTypeBuilder<GroupVoting> builder)
        {
            builder.ToTable("GroupVotings");
            builder.HasKey(us => us.Id);

            builder
                    .HasOne(us => us.Group)
                    .WithMany(u => u.GroupVoting)
                    .HasForeignKey(us => us.GroupId);

                builder
                    .HasOne(us => us.Voting)
                    .WithMany(s => s.GroupVoting)
                    .HasForeignKey(us => us.VotingId);
            }
    }
}
