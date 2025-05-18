using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class AccountTypeValidator : AbstractValidator<AccountType>
    {
        public AccountTypeValidator()
        {
            RuleFor(at => at.Name).NotEmpty().WithMessage("Hesap türü adı boş olamaz.");
            RuleFor(at => at.Name).MaximumLength(100).WithMessage("Hesap türü adı en fazla 100 karakter olmalıdır.");
            
            RuleFor(at => at.Description).MaximumLength(500).WithMessage("Hesap türü açıklaması en fazla 500 karakter olmalıdır.");
        }
    }
}
