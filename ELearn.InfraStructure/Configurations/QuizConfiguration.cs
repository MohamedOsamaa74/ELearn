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
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder) //3relation
        {
            builder.ToTable("Quizzes");
            builder.HasKey(x => x.QuizId);

            builder.HasOne(g => g.Group)
                .WithMany(u => u.Quizzes)
                .HasForeignKey(m => m.GroupId);
            
            builder.HasOne(u => u.User)
                .WithMany(u => u.Quizzes)
                .HasForeignKey(m => m.UserId);

            //one(quiz) to many (questions) in QuestionConfiguration





        }
    }
}
