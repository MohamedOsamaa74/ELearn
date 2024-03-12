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
    public class GroupSurveyConfiguration : IEntityTypeConfiguration<GroupSurvey>
    {
        public void Configure(EntityTypeBuilder<GroupSurvey> builder)
        {
            builder.ToTable("GroupSurveys");
            builder.HasKey(us => us.Id);

            builder
                .HasOne(us => us.Group)
                .WithMany(u => u.GroupSurvey)
                .HasForeignKey(us => us.GroupId);

            builder
                .HasOne(us => us.Survey)
                .WithMany(s => s.GroupSurvey)
                .HasForeignKey(us => us.SurveyId);
        }
    }
}
