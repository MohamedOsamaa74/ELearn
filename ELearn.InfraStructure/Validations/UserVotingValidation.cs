using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class UserVotingValidation : AbstractValidator<UserVoting>
    {
        public UserVotingValidation()
        {
            RuleFor(uv => uv.userId).NotEmpty();
            RuleFor(uv => uv.VotingId).NotEmpty();
        }
    }
}
