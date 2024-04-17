using ELearn.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.InfraStructure.Validations
{
    public class MaterialValidation : AbstractValidator<Material>
    {
        public MaterialValidation()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
