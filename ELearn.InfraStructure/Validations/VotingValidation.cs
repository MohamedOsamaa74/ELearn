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

            RuleFor(v => v.CreatorId).NotEmpty().NotNull();

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .NotNull().WithMessage("Title cannot be null.")
                .MaximumLength(150).WithMessage("Title cannot be more than 100 characters.");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required.")
                .NotNull().WithMessage("Description cannot be null.");

            RuleFor(v => v.Start)
               .NotEmpty().WithMessage("Start date is required.")
               .NotNull().WithMessage("Start date cannot be null.")
               .GreaterThan(DateTime.Now.AddMinutes(5)).WithMessage("Start date must be at least 5 minutes from now.")
               .GreaterThanOrEqualTo(v => v.CreationDate).WithMessage("Start date must be after the creation date.");

            RuleFor(v => v.End)
                .NotEmpty().WithMessage("End date is required.")
                .NotNull().WithMessage("End date cannot be null.")
                .GreaterThan(v => v.Start).WithMessage("End date must be after the start date.");

            RuleFor(v => v.CreationDate)
                .NotEmpty().WithMessage("Creation date is required.")
                .NotNull().WithMessage("Creation date cannot be null.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Creation date cannot be in the future.");



        }
    }
}
