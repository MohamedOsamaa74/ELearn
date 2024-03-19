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
    public class UserSurveyConfiguration : IEntityTypeConfiguration<UserSurvey>
    {
        public void Configure(EntityTypeBuilder<UserSurvey> builder)
        {
            builder.ToTable("UserSurveys");
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
