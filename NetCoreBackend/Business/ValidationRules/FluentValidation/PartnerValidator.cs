using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class PartnerValidator : AbstractValidator<Partner>
    {
        public PartnerValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Şirket Adı Boş Olamaz.");
            RuleFor(p => p.Name).MaximumLength(100).WithMessage("Şirket Adı En Fazla 100 Karakterden Oluşmalıdır.");

            RuleFor(p => p.TradeName).MaximumLength(100).WithMessage("Ticari Şirket Adı En Fazla 100 Karakterden Oluşmalıdır.");

            RuleFor(p => p.IdentityNumber).MaximumLength(50).WithMessage("ID(UID) Numarası En Fazla 50 Karakterden Oluşmalıdır.");

            RuleFor(p => p.VatNumber).MaximumLength(50).WithMessage("KDV Numarası En Fazla 50 Karakterden Oluşmalıdır.");

            RuleFor(x => x.BusinessPartnerType)
          .NotNull()
          .WithMessage("Bireysel veya Kurumsal alanı zorunludur.")
          .When(x => x.PartnerType == PartnerType.Business || x.PartnerType == PartnerType.Individual);
        }
    }
}
