using System;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearn.Domain.Entities;

namespace ELearn.InfraStructure.Validations
{
    public class AnnouncementValidation : AbstractValidator<Announcement>
    {
        public AnnouncementValidation() 
        {
            RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Please Enter Announcement Text");

            RuleFor(x => x.UserId)
            .NotNull();

        }
    }
}
