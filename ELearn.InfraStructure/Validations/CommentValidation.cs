using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class CommentValidation : AbstractValidator<Comment>
    {
        public CommentValidation() 
        {
            RuleFor(x => x.UserId)
                .NotNull();
            RuleFor(x=>x.Text)
                .NotEmpty();
            RuleFor(x => x.PostId)
                .NotNull();
            RuleFor(x => x.Date)
                .NotNull();

        }
    }
}
