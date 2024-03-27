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
    public class FileConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.ToTable("Files");
            builder.HasKey(x => x.Id);

            /*builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.FilePath).IsRequired();
            builder.Property(p => p.Url).IsRequired();
            builder.Property(p => p.Type).IsRequired();
            builder.Property(p => p.Createion).IsRequired();*/
        }
    }
}
