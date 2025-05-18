using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class PurchaseInvoiceExpenseValidator : AbstractValidator<PurchaseInvoiceExpense>
    {
        public PurchaseInvoiceExpenseValidator()
        {
            RuleFor(p => p.PurchaseInvoiceId).NotEmpty().WithMessage("Fatura boş olamaz.");
            RuleFor(p => p.PurchaseInvoiceId).GreaterThan(0).WithMessage("Geçerli bir fatura seçilmelidir.");
            
            RuleFor(p => p.PartnerId).NotEmpty().WithMessage("Tedarikçi boş olamaz.");
            RuleFor(p => p.PartnerId).GreaterThan(0).WithMessage("Geçerli bir tedarikçi seçilmelidir.");
            
            RuleFor(p => p.PartnerInvoiceNo).NotEmpty().WithMessage("Fatura numarası boş olamaz.");
            RuleFor(p => p.PartnerInvoiceNo).MaximumLength(50).WithMessage("Fatura numarası en fazla 50 karakter olmalıdır.");
            
            RuleFor(p => p.PartnerInvoiceDate).NotEmpty().WithMessage("Fatura tarihi boş olamaz.");
            
            RuleFor(p => p.ExpenseType).NotEmpty().WithMessage("Masraf türü boş olamaz.");
            
            RuleFor(p => p.Amount).NotEmpty().WithMessage("Tutar boş olamaz.");
            RuleFor(p => p.Amount).GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalıdır.");
            
            RuleFor(p => p.AmountFc).GreaterThanOrEqualTo(0).WithMessage("Döviz tutarı 0 veya daha büyük olmalıdır.");
        }
    }
}
