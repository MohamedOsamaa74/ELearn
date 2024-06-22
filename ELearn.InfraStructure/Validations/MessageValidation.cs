using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class MessageValidation : AbstractValidator<Message>
    {
        public MessageValidation()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Text).NotEmpty().NotNull().Length(1, 2000);
            RuleFor(x => x.SenderId).NotEmpty().NotNull();
            RuleFor(x => x.ReceiverId).NotEmpty().NotNull();
        }
    }
}
