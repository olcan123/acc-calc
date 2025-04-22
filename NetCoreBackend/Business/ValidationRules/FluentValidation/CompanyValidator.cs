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
            RuleFor(c => c.Name).NotEmpty().WithMessage("Şirket Adı Boş Olamaz.");
            RuleFor(c => c.UidNumber).NotEmpty().WithMessage("UID Numara Boş Olamaz.");
            RuleFor(c => c.UidNumber).Length(9).WithMessage("UID Numara 9 Karakterden Oluşmalıdır.");
        }
        
    }
}