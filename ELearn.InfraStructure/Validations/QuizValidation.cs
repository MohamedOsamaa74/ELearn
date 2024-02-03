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
            RuleFor(x => x.Id).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.title).MaximumLength(30).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.GroupId).NotEmpty();

        }
    }
}
