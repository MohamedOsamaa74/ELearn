﻿using ELearn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Groups");
            builder.HasKey(x => x.Id);
            //10 relation 

            //self relation (one to many)
            builder.HasOne(p =>p.ParentGroup)
                .WithMany(s => s.SubGroups)
                .HasForeignKey(e => e.ParentGroupId)
                .IsRequired(false);

            //one user create many groups  
            builder.HasOne(u => u.User)
                .WithMany(g => g.CreatedGroups)
                .HasForeignKey(u => u.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            //many user in many groups
            builder.HasMany(u => u.UsersInGroup)
                .WithMany(g => g.MyGroups)
                .UsingEntity<UserGroup>();

            //many tasks in one group
            builder.HasMany(a => a.Assignments)
                .WithOne(a => a.Group)
                .HasForeignKey(a => a.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
            //many announcement in many groups
            builder.HasMany(u => u.AnnouncementsOfGroups)
                .WithMany(g => g.GroupsOfAnnouncement)
                .UsingEntity<GroupAnnouncment>();
            //many vote&survey in many group(doha)

            //dept has many groups
            builder.HasOne(d => d.Department)
                .WithMany(g => g.GroupsOfDepartment)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(m => m.Materials)
                .WithOne(g => g.Group)
                .HasForeignKey(g => g.GroupId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(q => q.Quizzes)
                .WithOne(g => g.Group)
                .HasForeignKey(g => g.GroupId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.UserGroups)
                .WithOne(g => g.Group)
                .HasForeignKey(g => g.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
