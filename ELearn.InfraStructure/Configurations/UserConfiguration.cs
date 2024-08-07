﻿ using ELearn.Domain.Entities;
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

            //one user created many groups
            builder.HasMany(p => p.CreatedGroups)
                .WithOne(r => r.User)
                .HasForeignKey(p => p.CreatorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            //many users in many groups
            builder.HasMany(u => u.UserGroups)
                .WithOne(g => g.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //one user(o)  to many posts(M)
           builder.HasMany(p => p.Posts)
                  .WithOne(r => r.User)
                  .HasForeignKey(p => p.UserId)
                  .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);


            //one user (o) to one react (M)

        builder .HasOne(p => p.React)
                .WithOne(r => r.User)
                .HasForeignKey<React>(a=>a.UserID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction); //optinal


            //one user to many voting (staff)

         builder.HasMany(p => p.Votings)
                .WithOne(r => r.ApplicationUser)
                .HasForeignKey(v => v.CreatorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            //one user to many survey (staff)
        builder.HasMany(p => p.Surveys)
               .WithOne(r => r.ApplicationUser)
               .HasForeignKey(v => v.CreatorId)
               .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);


            //many to many survey (student) => userServeyconfiguration
            //many to many voting (student) => userVotingconfiguration

            //one user many comments

            builder.HasMany(p => p.Comments)
                   .WithOne(r => r.User)
                   .HasForeignKey(v => v.UserId)
                   .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);


            //Announcement
            builder.HasMany(p => p.Announcements)
             .WithOne(r => r.User)
             .HasForeignKey(v => v.UserId)
             .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            //one user create many tasks (satff)

            builder.HasMany(p => p.Assignments)
           .WithOne(r => r.User)
           .HasForeignKey(v => v.UserId)
           .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            //many student to many tasks (student) userassignment
          
            builder.HasMany(f => f.Files)
                .WithOne(c => c.Creator)
                .HasForeignKey(f => f.CreatorId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.ProfilePicture)
                .WithOne(u => u.User)
                .HasForeignKey<FileEntity>(f => f.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
