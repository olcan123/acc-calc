using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("İsim Boş Olamaz.");
            RuleFor(c => c.Name).MaximumLength(150).WithMessage("İsim En Fazla 150 Karakterden Oluşmalıdır.");
        }
    }
}