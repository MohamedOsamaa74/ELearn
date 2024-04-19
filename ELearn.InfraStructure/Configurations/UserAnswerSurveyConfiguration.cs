using ELearn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Configurations
{
    public class UserAnswerSurveyConfiguration : IEntityTypeConfiguration<UserAnswerSurvey>
    {
        public void Configure(EntityTypeBuilder<UserAnswerSurvey> builder)
        {
            builder.ToTable("UserAnswerSurveys");
            builder.HasKey(us => us.Id);

            builder
                .HasOne(us => us.User)
                .WithMany(u => u.UserSurvey)
                .HasForeignKey(us => us.UserId);

            builder
                .HasOne(us => us.Survey)
                .WithMany(s => s.UserSurvey)
                .HasForeignKey(us => us.SurveyId);


        }
    }
}
