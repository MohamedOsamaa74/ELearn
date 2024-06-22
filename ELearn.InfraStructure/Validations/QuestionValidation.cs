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
            RuleFor(x => x.Id).NotNull();
            RuleFor(q => q.Text)
                .NotEmpty().MaximumLength(1000).WithMessage("Question text is required.");
            RuleFor(x => x.Grade)
                .NotEmpty().WithMessage("Grade is required.")
                .GreaterThan(0.0).WithMessage("Grade must be greater than 0.");

        }
    }
}
