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
            RuleFor(x => x.Id).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.Text).NotEmpty();
            RuleFor(x => x.SenderId).NotEmpty();
            RuleFor(x => x.ReceiverId).NotEmpty();
        }
    }
}
