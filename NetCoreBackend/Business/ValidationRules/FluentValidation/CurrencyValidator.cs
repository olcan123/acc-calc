using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CurrencyValidator : AbstractValidator<Currency>
    {
        public CurrencyValidator()
        {
            RuleFor(c => c.Code).NotEmpty().WithMessage("Para birimi kodu boş olamaz.");
            RuleFor(c => c.Code).Length(3).WithMessage("Para birimi kodu 3 karakterden oluşmalıdır.");
            RuleFor(c => c.Name).NotEmpty().WithMessage("Para birimi adı boş olamaz.");
            RuleFor(c => c.Name).MaximumLength(50).WithMessage("Para birimi adı en fazla 50 karakter olmalıdır.");
        }
    }
}
