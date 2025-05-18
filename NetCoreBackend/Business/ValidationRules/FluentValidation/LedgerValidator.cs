using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class LedgerValidator : AbstractValidator<Ledger>
    {
        public LedgerValidator()
        {
            RuleFor(l => l.ReferenceNo).NotEmpty().WithMessage("Referans numarası boş olamaz.");
            RuleFor(l => l.ReferenceNo).MaximumLength(50).WithMessage("Referans numarası en fazla 50 karakter olmalıdır.");
            RuleFor(l => l.DocumentDate).NotEmpty().WithMessage("Belge tarihi boş olamaz.");
            RuleFor(l => l.DocumentType).NotEmpty().WithMessage("Belge türü boş olamaz.");
            RuleFor(l => l.Description).MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olmalıdır.");
        }
    }
}
