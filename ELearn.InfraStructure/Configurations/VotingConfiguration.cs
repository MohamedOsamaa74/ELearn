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
    public class VotingConfiguration : IEntityTypeConfiguration<Voting>
    {
        public void Configure(EntityTypeBuilder<Voting> builder)
        {
            builder.ToTable("Votings");
            builder.HasKey(v => v.Id);
            builder.HasMany(g => g.Group)
                .WithMany(v => v.votings)
                .UsingEntity<GroupVoting>();
                           /*(gv => gv.HasOne(gv => gv.Group).WithMany(g => g.GroupVotings).HasForeignKey(gv => gv.GroupId),
                           gv => gv.HasOne(gv => gv.Voting).WithMany(v => v.GroupVotings).HasForeignKey(gv => gv.VotingId));*/
        }
    }
}
