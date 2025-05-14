using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class VatValidator : AbstractValidator<Vat>
    {
        public VatValidator()
        {
            RuleFor(v => v.Name).NotEmpty().WithMessage("KDV adı boş olamaz.");
            RuleFor(v => v.Name).MaximumLength(50).WithMessage("KDV adı en fazla 50 karakterden oluşmalıdır.");

            RuleFor(v => v.Rate).GreaterThanOrEqualTo(0).WithMessage("KDV oranı 0 veya daha büyük olmalıdır.");
        }
    }

}
