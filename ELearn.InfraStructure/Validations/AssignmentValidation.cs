using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class AssignmentValidation: AbstractValidator<Assignment>
    {
        public AssignmentValidation() 
        {
            RuleFor(x=>x.CreationDate)
                .NotNull();

            RuleFor(x => x.End)
                .NotNull();

            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(1,25);

            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(1, 250);

            RuleFor(x => x.UserId)
                .NotNull();

            RuleFor(x => x.GroupId)
                .NotNull();
        }
    }
}
