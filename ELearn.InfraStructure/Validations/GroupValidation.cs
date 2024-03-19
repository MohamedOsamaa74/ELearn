using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class GroupValidation : AbstractValidator<Group>
    {
        public GroupValidation() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().Length(5, 30);
            RuleFor(x => x.Description)
                .NotEmpty().Length(5, 30);
            RuleFor(x => x.CreatorId)
                .NotNull();
        }
    }
}
