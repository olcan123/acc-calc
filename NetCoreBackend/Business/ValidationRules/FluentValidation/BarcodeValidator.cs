using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class BarcodeValidator : AbstractValidator<Barcode>
    {
        public BarcodeValidator()
        {
            RuleFor(b => b.Code).NotEmpty().WithMessage("Barkod kodu boş olamaz.");
            RuleFor(b => b.Code).MaximumLength(50).WithMessage("Barkod kodu en fazla 50 karakterden oluşmalıdır.");
            
            RuleFor(b => b.Type).NotEmpty().WithMessage("Barkod tipi boş olamaz.");
            RuleFor(b => b.Type).MaximumLength(20).WithMessage("Barkod tipi en fazla 20 karakterden oluşmalıdır.");
            
            RuleFor(b => b.ProductId).NotEmpty().WithMessage("Ürün seçilmelidir.");
            RuleFor(b => b.ProductId).GreaterThan(0).WithMessage("Geçerli bir ürün seçilmelidir.");
        }
    }
}
