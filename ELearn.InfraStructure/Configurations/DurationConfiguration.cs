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
    public class DurationConfiguration : IEntityTypeConfiguration<Duration>
    {
        public void Configure(EntityTypeBuilder<Duration> builder)
        {
            builder.HasNoKey();
        }
    }
}
