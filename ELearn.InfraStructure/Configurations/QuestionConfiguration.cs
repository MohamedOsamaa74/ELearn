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
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");
            builder.HasKey(x => x.Id);

            //m to m
            builder.HasMany(u => u.ApplicationUser)
                .WithMany(u => u.Question)
                .UsingEntity<UserQuestion>();
            //one(quiz) to many (questions)
            builder.HasOne(q => q.Quiz)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.QuizId);

            builder.HasMany(u => u.UserQuestion)
                .WithOne(q => q.Question)
                .HasForeignKey(q => q.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}