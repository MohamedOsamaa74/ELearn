using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
   public class UserQuestionValidation : AbstractValidator<UserAnswerQuestion>
    {
        public UserQuestionValidation() {
            RuleFor(uq => uq.QuestionId).NotEmpty();
            RuleFor(uq => uq.UserId).NotEmpty();
        }
    }
}
