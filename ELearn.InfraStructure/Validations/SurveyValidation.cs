using ELearn.Domain.Entities;
using FluentValidation;

namespace ELearn.InfraStructure.Validations
{
    public class SurveyValidation : AbstractValidator<Survey>
    {
        public SurveyValidation() {
            RuleFor(s => s.Id).NotEmpty().NotNull();
            RuleFor(s => s.Text).NotEmpty().NotNull();
            //RuleFor(s => s.Options).NotEmpty().WithMessage("Survey must have at least one option.");
            RuleFor(s => s.CreationDate).NotEmpty().NotNull();
            RuleFor(s => s.Start).NotEmpty().NotNull();
            RuleFor(s => s.End).NotEmpty().NotNull();
           
            RuleFor(s => s.CreatorId).NotEmpty().NotNull();
            RuleFor(s => s.ApplicationUser).NotNull();
            RuleFor(s => s.UserSurvey).NotNull();
            RuleForEach(s => s.UserSurvey).NotNull();
            RuleFor(s => s.User).NotNull();
            RuleFor(s => s.GroupSurvey).NotNull();
            RuleForEach(s => s.GroupSurvey).NotNull();
            RuleFor(s => s.Group).NotNull();
            RuleForEach(s => s.Group).NotNull();
            RuleFor(s => s.Question).NotNull();
            RuleForEach(s => s.Question).NotNull();

        }

    }
}
