using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Şirket Adı Boş Olamaz.");
            RuleFor(c => c.Name).MaximumLength(500).WithMessage("Şirket Adı En Fazla 500 Karakterden Oluşmalıdır.");
            
            RuleFor(c => c.TradeName).MaximumLength(500).WithMessage("Ticari Şirket Adı En Fazla 500 Karakterden Oluşmalıdır.");
            
            RuleFor(c => c.UidNumber).NotEmpty().WithMessage("UID Numara Boş Olamaz.");
            RuleFor(c => c.UidNumber).MaximumLength(50).WithMessage("UID Numara En Fazla 50 Karakterden Oluşmalıdır.");
            
            RuleFor(c => c.VatNumber).MaximumLength(50).WithMessage("KDV Numarası En Fazla 50 Karakterden Oluşmalıdır.");
            
            RuleFor(c => c.Period).Length(4).WithMessage("Dönem 4 Karakterden Oluşmalıdır.");
            RuleFor(c => c.Period).MaximumLength(5).WithMessage("Dönem En Fazla 5 Karakterden Oluşmalıdır.");
        }
    }
}