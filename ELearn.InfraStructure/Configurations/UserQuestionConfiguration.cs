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
    public class UserQuestionConfiguration : IEntityTypeConfiguration<UserQuestion>
    {
        public void Configure(EntityTypeBuilder<UserQuestion> builder)
        {
            throw new NotImplementedException();//option id
        }
    }
}
