using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class LedgerEntryValidator : AbstractValidator<LedgerEntry>
    {
        public LedgerEntryValidator()
        {
            RuleFor(l => l.LedgerId).NotEmpty().WithMessage("Fiş numarası boş olamaz.");
            RuleFor(l => l.LedgerId).GreaterThan(0).WithMessage("Geçerli bir fiş seçilmelidir.");
            
            RuleFor(l => l.AccountId).NotEmpty().WithMessage("Hesap kodu boş olamaz.");
            RuleFor(l => l.AccountId).GreaterThan(0).WithMessage("Geçerli bir hesap seçilmelidir.");
            
            RuleFor(l => l.Description).MaximumLength(200).WithMessage("Açıklama en fazla 200 karakter olmalıdır.");
        
            
            // Borç ve alacak tutarı aynı anda girilmemeli
            RuleFor(l => l)
                .Must(l => (l.Debit > 0 && l.Credit == 0) || (l.Credit > 0 && l.Debit == 0))
                .WithMessage("Bir satırda borç veya alacaktan sadece biri dolu olmalıdır.");
        }
    }
}
