using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class QuizValidation : AbstractValidator<Quiz>
    {
        public QuizValidation()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.title).MaximumLength(100).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.GroupId).NotEmpty().NotNull();

            RuleFor(x => x.Grade)
                .NotEmpty().WithMessage("Grade is required.")
                .GreaterThan(0).WithMessage("Grade must be greater than 0.");
            RuleFor(x => x.Start)
               .NotEmpty().WithMessage("Start date is required.")
               .NotNull().WithMessage("Start date cannot be null.")
               .GreaterThan(DateTime.UtcNow.AddMinutes(5)).WithMessage("Start date must be at least 5 minutes from now.");

            RuleFor(x => x.End)
                .NotEmpty().WithMessage("End date is required.")
                .NotNull().WithMessage("End date cannot be null.")
                .GreaterThan(x => x.Start).WithMessage("End date must be after the start date.");

            RuleFor(x => x.Questions)
                .NotEmpty().WithMessage("There must be at least one question.")
                .ForEach(q => q.SetValidator(new QuestionValidation()));

        }
    }
}
