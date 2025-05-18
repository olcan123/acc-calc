using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(a => a.Code).NotEmpty().WithMessage("Hesap kodu boş olamaz.");
            RuleFor(a => a.Code).MaximumLength(50).WithMessage("Hesap kodu en fazla 50 karakter olmalıdır.");

            RuleFor(a => a.Name).NotEmpty().WithMessage("Hesap adı boş olamaz.");
            RuleFor(a => a.Name).MaximumLength(200).WithMessage("Hesap adı en fazla 200 karakter olmalıdır.");

            RuleFor(a => a.AccountTypeId).NotEmpty().WithMessage("Hesap türü boş olamaz.");
            RuleFor(a => a.AccountTypeId).GreaterThan((short)0).WithMessage("Geçerli bir hesap türü seçilmelidir.");

            // Üst hesabın, kendi kendisine atanmasını önleme (ParentAccountId > 0 olduğunda)
            RuleFor(a => a)
                .Must(a => a.Id != a.ParentAccountId || a.ParentAccountId == 0)
                .WithMessage("Bir hesap, kendisi için üst hesap olarak atanamaz.");
        }
    }
}
