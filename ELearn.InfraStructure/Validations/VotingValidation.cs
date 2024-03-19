using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class VotingValidation : AbstractValidator<Voting>
    {
        public VotingValidation()
        {
            RuleFor(v => v.Text).NotEmpty();
            RuleFor(v => v.CreateDate).NotEmpty();
            RuleFor(v => v.CreatorId).NotEmpty();
        }
    }
}
