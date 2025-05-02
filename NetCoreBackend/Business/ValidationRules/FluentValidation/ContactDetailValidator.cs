using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ContactDetailValidator : AbstractValidator<ContactDetail>
    {
        public ContactDetailValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Boş Olamaz.");
            RuleFor(x => x.Name).MaximumLength(150).WithMessage("İsim En Fazla 150 Karakterden Oluşmalıdır.");

            RuleFor(x => x.Value).NotEmpty().WithMessage("Değer Boş Olamaz.");
            RuleFor(x => x.Value).MaximumLength(250).WithMessage("Değer En Fazla 250 Karakterden Oluşmalıdır.");

            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Açıklama En Fazla 500 Karakterden Oluşmalıdır.");

            RuleFor(x => x.ContactId).NotEmpty().WithMessage("İletişim Isimi Boş Olamaz.");
        }
    }
}