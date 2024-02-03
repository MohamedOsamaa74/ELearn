using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class ApplicationUserValidation : AbstractValidator<ApplicationUser>
    {
        public ApplicationUserValidation() 
        {
            RuleFor(x => x)
                .NotNull();
            RuleFor(x => x.FirstName)
                .NotEmpty();
            RuleFor(x=> x.LastName)
                .NotEmpty();
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid Email address.");
            RuleFor(x => x.EmailConfirmed)
                .NotEmpty().WithMessage("Email address is required.");
            RuleFor(x => x.PhoneNumber)
                .Length(11);
            RuleFor(x => x.Address)
                .NotEmpty()
                .MinimumLength(8);
            RuleFor(x => x.BirthDate)
                .NotEmpty();
            RuleFor(x => x.DepartmentId)
                .NotNull();
            RuleFor(x=>x.Grade)
                .NotEmpty();
            RuleFor(x=>x.Nationality)
                .NotEmpty();

        }
        
    }
}
