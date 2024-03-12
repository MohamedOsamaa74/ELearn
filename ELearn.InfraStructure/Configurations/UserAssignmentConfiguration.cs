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
    public class UserAssignmentConfiguration : IEntityTypeConfiguration<UserAssignment>
    {
        public void Configure(EntityTypeBuilder<UserAssignment> builder)
        {
            builder.ToTable("UserAssignments");
            builder.HasKey(ua => ua.Id);

            builder
                .HasOne(ua => ua.Users)
                .WithMany(u => u.UserAssignment)
                .HasForeignKey(ua => ua.UserId);

            builder
                .HasOne(ua => ua.Assignment)
                .WithMany(a => a.UserAssignment)
                .HasForeignKey(ua => ua.AssignmentId);
        }
    }
}
