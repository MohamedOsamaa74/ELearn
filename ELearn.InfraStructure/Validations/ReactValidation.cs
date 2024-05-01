using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class ReactValidation : AbstractValidator<React>
    {
        public ReactValidation()
        {
            RuleFor(r => r.PostID).NotEmpty();
            RuleFor(r => r.UserID).NotEmpty();
            RuleFor(r => r.CreationDate).NotEmpty();
            RuleFor(r => r.Type).NotNull();
            RuleFor(r => r.Post).NotNull();
            RuleFor(r => r.User).NotNull();
        }
    }
}
