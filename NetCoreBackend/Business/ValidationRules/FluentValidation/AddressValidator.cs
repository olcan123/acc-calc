using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(a => a.City).NotEmpty().WithMessage("Şehir Boş Olamaz.");
            RuleFor(a => a.City).MaximumLength(150).WithMessage("Şehir En Fazla 150 Karakterden Oluşmalıdır.");
            RuleFor(a => a.Street).NotEmpty().WithMessage("Sokak Adı Boş Olamaz.");
            RuleFor(a => a.Street).MaximumLength(250).WithMessage("Sokak Adı En Fazla 250 Karakterden Oluşmalıdır.");
            RuleFor(a => a.ZipCode).NotEmpty().WithMessage("Posta Kodu Boş Olamaz.");
            RuleFor(a => a.ZipCode).Length(5).WithMessage("Posta Kodu 5 Karakterden Oluşmalıdır.");
            RuleFor(a => a.Country).NotEmpty().WithMessage("Ülke Boş Olamaz.");
            RuleFor(a => a.Country).MaximumLength(150).WithMessage("Ülke En Fazla 150 Karakterden Oluşmalıdır.");
        }
    }
}