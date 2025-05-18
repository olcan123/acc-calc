using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class PurchaseInvoiceValidator : AbstractValidator<PurchaseInvoice>
    {
        public PurchaseInvoiceValidator()
        {
            RuleFor(p => p.InvoiceNo).NotEmpty().WithMessage("Fatura numarası boş olamaz.");
            RuleFor(p => p.InvoiceNo).MaximumLength(50).WithMessage("Fatura numarası en fazla 50 karakter olmalıdır.");
            
            RuleFor(p => p.PartnerId).NotEmpty().WithMessage("Tedarikçi boş olamaz.");
            RuleFor(p => p.PartnerId).GreaterThan(0).WithMessage("Geçerli bir tedarikçi seçilmelidir.");
            
            RuleFor(p => p.InvoiceDate).NotEmpty().WithMessage("Fatura tarihi boş olamaz.");
            
            RuleFor(p => p.LedgerId).NotEmpty().WithMessage("Muhasebe fişi boş olamaz.");
            RuleFor(p => p.LedgerId).GreaterThan(0).WithMessage("Geçerli bir muhasebe fişi seçilmelidir.");
            
            RuleFor(p => p.CurrencyId).NotEmpty().WithMessage("Para birimi boş olamaz.");
            RuleFor(p => p.CurrencyId).GreaterThan(0).WithMessage("Geçerli bir para birimi seçilmelidir.");
            
            RuleFor(p => p.ExchangeRate).NotEmpty().WithMessage("Döviz kuru boş olamaz.");
            RuleFor(p => p.ExchangeRate).GreaterThan(0).WithMessage("Döviz kuru sıfırdan büyük olmalıdır.");
            
            RuleFor(p => p.Note).MaximumLength(500).WithMessage("Not alanı en fazla 500 karakter olmalıdır.");
        }
    }
}
