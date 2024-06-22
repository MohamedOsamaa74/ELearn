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
            
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid Email address.");

            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("First name can only contain alphabetic characters.");


            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Name can only contain alphabetic characters.");


            RuleFor(user => user.BirthDate)
                .NotEmpty().WithMessage("Birthdate is required.")
                .Must(BeAValidAge).WithMessage("You must be at least 18 years old.");

            RuleFor(user => user.Address)
                .NotNull()
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(100).WithMessage("Address cannot exceed 100 characters.");

            RuleFor(user => user.Nationality)
                .NotNull()
                .NotEmpty().WithMessage("Nationality is required.")
                .MaximumLength(50).WithMessage("Nationality cannot exceed 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Nationality can only contain alphabetic characters.");

            RuleFor(user => user.Relegion)
                .NotEmpty().WithMessage("Religion is required.")
                .MaximumLength(50).WithMessage("Religion cannot exceed 50 characters.");

            RuleFor(user => user.Faculty)
                .NotEmpty().WithMessage("Faculty is required.")
                .MaximumLength(50).WithMessage("Faculty cannot exceed 50 characters.");

            RuleFor(user => user.NId)
                .NotEmpty().WithMessage("National ID is required.")
                .Matches(@"^\d{14}$").WithMessage("Invalid National ID");

            RuleFor(user => user.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.");

            RuleFor(user => user.PhoneNumber)
            .Matches(@"^\d{11}$").WithMessage("Invalid phone number format. Must be exactly 11 digits.");
        }

        private bool BeAValidAge(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            return age >= 17;
        }
    
    }
}