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
            builder.HasKey(x => x.QuestionId);

            builder.HasOne(q => q.Voting)
                    .WithOne(x => x.Question)
                    .HasForeignKey<Voting>(x => x.Id);

            builder.HasOne(s => s.Survey)
                .WithMany(q => q.Question)
                .HasForeignKey(s => s.SurveyId);

            //m to m
            builder.HasMany(u => u.ApplicationUser)
                .WithMany(u => u.Question)
                .UsingEntity<UserQuestion>();

        }
    }
}
