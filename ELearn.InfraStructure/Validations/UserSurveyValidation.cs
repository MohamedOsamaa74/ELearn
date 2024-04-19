using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class UserSurveyValidation : AbstractValidator<UserAnswerSurvey>
    {
        public UserSurveyValidation() {
            RuleFor(us => us.SurveyId).NotEmpty();
            RuleFor(us => us.UserId).NotEmpty();
        }
    }
}
