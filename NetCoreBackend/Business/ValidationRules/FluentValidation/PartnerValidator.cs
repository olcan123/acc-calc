
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class PartnerValidator : AbstractValidator<Partner>
    {
        public PartnerValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Şirket Adı Boş Olamaz.");
            RuleFor(p => p.Name).MaximumLength(700).WithMessage("Şirket Adı En Fazla 700 Karakterden Oluşmalıdır.");

            RuleFor(p => p.IdentityNumber).MaximumLength(25).WithMessage("ID(UID) Numarası En Fazla 25 Karakterden Oluşmalıdır.");

            RuleFor(p => p.VatNumber).MaximumLength(25).WithMessage("KDV Numarası En Fazla 25 Karakterden Oluşmalıdır.");

            RuleFor(x => x.BusinessPartnerType)
          .NotNull()
          .WithMessage("Bireysel veya Kurumsal alanı zorunludur.")
          .When(x => x.PartnerType == PartnerType.Business || x.PartnerType == PartnerType.Individual);
        }
    }
}
