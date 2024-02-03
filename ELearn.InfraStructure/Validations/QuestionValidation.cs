using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class QuestionValidation : AbstractValidator<Question>
    {
        public QuestionValidation()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.Text).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Options).NotEmpty();

        }
    }
}
