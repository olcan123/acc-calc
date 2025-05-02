using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class BankAccountValidator : AbstractValidator<BankAccount>
    {
        public BankAccountValidator()
        {
            RuleFor(b => b.AccountNumber).NotEmpty().WithMessage("Hesap Numarası Boş Olamaz.");
            RuleFor(b => b.AccountNumber).MaximumLength(50).WithMessage("Hesap Numarası En Fazla 50 Karakterden Oluşmalıdır.");

            RuleFor(b => b.IBAN).MaximumLength(30).WithMessage("IBAN En Fazla 30 Karakterden Oluşmalıdır.");

            RuleFor(b => b.BankId).NotEmpty().WithMessage("Banka Boş Olamaz.");

            RuleFor(b => b.SwiftCode).MaximumLength(20).WithMessage("SWIFT Kodu En Fazla 20 Karakterden Oluşmalıdır.");

            RuleFor(b => b.Currency).NotEmpty().WithMessage("Para Birimi Boş Olamaz.");
            RuleFor(b => b.Currency).MaximumLength(30).WithMessage("Para Birimi En Fazla 30 Karakterden Oluşmalıdır.");

            RuleFor(b => b.BranchName).MaximumLength(200).WithMessage("Açıklama En Fazla 200 Karakterden Oluşmalıdır.");
        }
    }
}