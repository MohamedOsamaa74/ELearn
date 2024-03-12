using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class SurveyValidation : AbstractValidator<Survey>
    {
        public SurveyValidation() {
            RuleFor(s => s.Id).NotEmpty();
            RuleFor(s => s.Text).NotEmpty();
            RuleFor(s => s.Options).NotEmpty().WithMessage("Survey must have at least one option.");
            RuleFor(s => s.Date).NotEmpty();
            RuleFor(s => s.ApplicationUserId).NotEmpty();
            RuleFor(s => s.ApplicationUser).NotNull();
            RuleFor(s => s.UserSurvey).NotNull();
            RuleForEach(s => s.UserSurvey).NotNull();
            RuleFor(s => s.user).NotNull();
            RuleFor(s => s.GroupSurvey).NotNull();
            RuleForEach(s => s.GroupSurvey).NotNull();
            RuleFor(s => s.Group).NotNull();
            RuleForEach(s => s.Group).NotNull();
            RuleFor(s => s.Question).NotNull();
            RuleForEach(s => s.Question).NotNull();
        }

    }
}
