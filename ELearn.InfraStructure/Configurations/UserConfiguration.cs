using ELearn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

        builder .HasOne(p => p.React)
                .WithOne(r => r.User)
                .IsRequired(false); //optinal


            //one user to many voting (staff)

         builder.HasMany(p => p.Votings)
                .WithOne(r => r.ApplicationUser)
                .HasForeignKey(v => v.ApplicationUserId)
                .IsRequired(false);

            //one user to many survey (staff)
        builder.HasMany(p => p.Surveys)
               .WithOne(r => r.ApplicationUser)
               .HasForeignKey(v => v.ApplicationUserId)
               .IsRequired(false);


            //many to many survey (student) => userServeyconfiguration
            //many to many voting (student) => userVotingconfiguration

            //one user many comments

            builder.HasMany(p => p.Comments)
                   .WithOne(r => r.User)
                   .HasForeignKey(v => v.UserId)
                   .IsRequired(false);


            //Announcement
            builder.HasMany(p => p.Announcements)
             .WithOne(r => r.User)
             .HasForeignKey(v => v.UserId)
             .IsRequired(false);
        }
    }
}
