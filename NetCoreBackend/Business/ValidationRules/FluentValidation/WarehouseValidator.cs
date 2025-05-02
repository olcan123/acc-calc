using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules
{
    public class WarehouseValidator : AbstractValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Depo Adı Boş Olamaz.");
            RuleFor(c => c.Name).MaximumLength(150).WithMessage("Depo Adı En Fazla 150 Karakterden Oluşmalıdır.");
        }
    }
}