using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class PurchaseInvoiceLineValidator : AbstractValidator<PurchaseInvoiceLine>
    {
        public PurchaseInvoiceLineValidator()
        {
            RuleFor(p => p.PurchaseInvoiceId).NotEmpty().WithMessage("Fatura boş olamaz.");
            RuleFor(p => p.PurchaseInvoiceId).GreaterThan(0).WithMessage("Geçerli bir fatura seçilmelidir.");
            
            RuleFor(p => p.ProductId).NotEmpty().WithMessage("Ürün boş olamaz.");
            RuleFor(p => p.ProductId).GreaterThan(0).WithMessage("Geçerli bir ürün seçilmelidir.");
            
            RuleFor(p => p.UnitPrice).NotEmpty().WithMessage("Birim fiyat boş olamaz.");
            RuleFor(p => p.UnitPrice).GreaterThan(0).WithMessage("Birim fiyat 0'dan büyük olmalıdır.");
            
            RuleFor(p => p.Quantity).NotEmpty().WithMessage("Miktar boş olamaz.");
            RuleFor(p => p.Quantity).GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır.");
    
        }
    }
}
