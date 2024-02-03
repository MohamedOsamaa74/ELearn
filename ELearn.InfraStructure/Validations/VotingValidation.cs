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
            RuleFor(v => v.Duration).NotNull();
            RuleFor(v => v.Options).NotEmpty().WithMessage("Voting must have at least one option.");
            RuleFor(v => v.ApplicationUserId).NotEmpty();
            RuleFor(v => v.ApplicationUser).NotNull();
            RuleFor(v => v.UserVoting).NotNull();
            RuleForEach(v => v.UserVoting).NotNull();
            RuleFor(v => v.user).NotNull();
            RuleFor(v => v.GroupVoting).NotNull();
            RuleForEach(v => v.GroupVoting).NotNull();
            RuleFor(v => v.Group).NotNull();
            RuleForEach(v => v.Group).NotNull();
            RuleFor(v => v.QuestionId).NotEmpty();
            RuleFor(v => v.Question).NotNull();
        }
    }
}
