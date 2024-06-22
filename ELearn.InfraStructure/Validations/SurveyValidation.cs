using ELearn.Domain.Entities;
using FluentValidation;

namespace ELearn.InfraStructure.Validations
{
    public class SurveyValidation : AbstractValidator<Survey>
    {
        public SurveyValidation() {
            RuleFor(s => s.Id).NotNull();
            RuleFor(s => s.Text).NotEmpty().NotNull();
            //RuleFor(s => s.Options).NotEmpty().WithMessage("Survey must have at least one option.");
            RuleFor(s => s.CreationDate).NotEmpty().NotNull();
            RuleFor(s => s.Start).NotEmpty().NotNull();
            RuleFor(s => s.End).NotEmpty().NotNull();
            RuleFor(s => s.CreatorId).NotEmpty().NotNull();

        }

    }
}
