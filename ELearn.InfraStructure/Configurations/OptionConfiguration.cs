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
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.ToTable("Options");
            builder.HasKey(x => x.Id);
            //one question to many options
            builder.HasOne(q=>q.Question)
                .WithMany(o=>o.Options)
                .HasForeignKey(q=>q.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            //one voting to many options
            builder.HasOne(v=>v.Voting)
                .WithMany(o => o.Options)
                .HasForeignKey(v=>v.Id)
                .OnDelete(DeleteBehavior.NoAction);

            //one survey to many options
            builder.HasOne(q => q.Survey)
                .WithMany(o => o.Options)
                .HasForeignKey(q => q.SurveyId);

        }
    }
}
