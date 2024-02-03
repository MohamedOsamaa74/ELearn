using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class UserGroupValidation: AbstractValidator<UserGroup>
    {
        public UserGroupValidation() {
            RuleFor(ug => ug.GroupId).NotEmpty();
            RuleFor(ug => ug.UserId).NotEmpty();
           // RuleFor(ug => ug.Group).NotNull();
           // RuleFor(ug => ug.User).NotNull();
        }
    }
}
