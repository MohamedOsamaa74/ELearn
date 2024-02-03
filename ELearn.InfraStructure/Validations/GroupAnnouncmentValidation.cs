using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class GroupAnnouncmentValidation : AbstractValidator<GroupAnnouncment>
    {
        public GroupAnnouncmentValidation() 
        {
            RuleFor(x => x.GroupId)
                .NotNull();
            RuleFor(x => x.AnnouncementId)
                .NotNull();

        }
    }
}
