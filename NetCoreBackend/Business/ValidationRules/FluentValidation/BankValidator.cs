using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class BankValidator : AbstractValidator<Bank>
    {
        public BankValidator()
        {
            RuleFor(b => b.Name).NotEmpty().WithMessage("Banka Adı Boş Olamaz.");
            RuleFor(b => b.Name).MaximumLength(150).WithMessage("Banka Adı En Fazla 150 Karakterden Oluşmalıdır.");
        }
    }
}