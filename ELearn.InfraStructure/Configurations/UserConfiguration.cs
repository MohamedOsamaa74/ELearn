using ELearn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ELearn.InfraStructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //one user(o)  to many posts(M)
            builder.HasMany(p => p.Posts)
                  .WithOne(r => r.User)
                  .HasForeignKey(p => p.UserId)
                  .IsRequired(false);


            //one user (o) to one react (M)

           builder .HasOne(u => u.React)
                .WithOne(r => r.User)
                .IsRequired(false); //optinal
        }
    }
}
