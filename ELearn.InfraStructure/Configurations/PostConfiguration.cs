using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ELearn.InfraStructure.Configuritions
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

           builder.Property(t => t.Text)
                .IsRequired();

            builder.Property(t => t.CreationDate)
                .IsRequired();

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.HasMany(p => p.Reacts)
                   .WithOne(r => r.Post)
                   .HasForeignKey(p => p.PostID)
                   .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.Comments)
                  .WithOne(r => r.Post)
                  .HasForeignKey(p => p.PostId)
                  .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.Files)
                  .WithOne(r => r.Post)
                  .HasForeignKey(p => p.PostId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }

}