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
    public class QuestionQuizConfiguration : IEntityTypeConfiguration<QuestionQuiz>
    {
        public void Configure(EntityTypeBuilder<QuestionQuiz> builder)
        {
            builder.ToTable("QuestionQuiz");
            builder.HasKey(x => x.QuestionId);
            
        }
    }
}
