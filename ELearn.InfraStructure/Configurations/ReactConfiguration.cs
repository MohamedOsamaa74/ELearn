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
    public class ReactConfiguration : IEntityTypeConfiguration<React>
    {
        public void Configure(EntityTypeBuilder<React> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.UserID)
                .IsRequired();

            builder.Property(r => r.PostID)
                .IsRequired();

            builder.Property(r => r.CreationDate)
                .IsRequired();
        }
    }
}
