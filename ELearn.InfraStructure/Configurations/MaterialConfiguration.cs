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
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("Materials");
            builder.HasKey(x => x.MaterialId);
            builder.Property(s => s.title).HasMaxLength(30);

            builder.HasOne(u => u.User)
                .WithMany(m=>m.Materials)
                .HasForeignKey(m => m.UserId);

            builder.HasOne(g=>g.Group)
                .WithMany(m => m.Materials)
                .HasForeignKey(g => g.GroupId);



        }
    }
}
