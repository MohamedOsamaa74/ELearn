using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class GroupSurveyValidation : AbstractValidator<GroupSurvey>
    {
        public GroupSurveyValidation() {
            RuleFor(x=>x.Id).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
