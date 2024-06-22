using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class GroupVotingValidation : AbstractValidator<GroupVoting>
    {
        public GroupVotingValidation()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
