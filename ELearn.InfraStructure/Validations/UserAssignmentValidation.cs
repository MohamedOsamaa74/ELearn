using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
  public class UserAssignmentValidation : AbstractValidator<UserAssignment>
    {
        public UserAssignmentValidation() {
            RuleFor(ua => ua.AssignmentId).NotEmpty();
            RuleFor(ua => ua.UserId).NotEmpty();
           // RuleFor(ua => ua.Assignment).NotNull();
            //RuleFor(ua => ua.Users).NotNull();
        }
    }
}
