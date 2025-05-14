using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UnitOfMeasureValidator : AbstractValidator<UnitOfMeasure>
    {
        public UnitOfMeasureValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("Birim adı boş olamaz.");
            RuleFor(u => u.Name).MaximumLength(100).WithMessage("Birim adı en fazla 100 karakterden oluşmalıdır.");
            RuleFor(u => u.Abbreviation).NotEmpty().WithMessage("Birim kısaltması boş olamaz.");
            RuleFor(u => u.Abbreviation).MaximumLength(10).WithMessage("Birim kısaltması en fazla 10 karakterden oluşmalıdır.");
        }
    }
}
