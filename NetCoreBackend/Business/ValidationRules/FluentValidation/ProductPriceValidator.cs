using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductPriceValidator : AbstractValidator<ProductPrice>
    {
        public ProductPriceValidator()
        {
            RuleFor(p => p.ProductId).NotEmpty().WithMessage("Ürün seçilmelidir.");
            RuleFor(p => p.ProductId).GreaterThan(0).WithMessage("Geçerli bir ürün seçilmelidir.");
            
            RuleFor(p => p.UnitPrice).NotEmpty().WithMessage("Birim fiyat boş olamaz.");
            RuleFor(p => p.UnitPrice).GreaterThan(0).WithMessage("Birim fiyat 0'dan büyük olmalıdır.");
            
            // Promosyon fiyatı ise geçerlilik tarihleri zorunlu olmalıdır
            When(p => p.Category == PriceCategory.Promo, () => 
            {
                RuleFor(p => p.ValidFrom).NotNull().WithMessage("Promosyon başlangıç tarihi boş olamaz.");
                RuleFor(p => p.ValidTo).NotNull().WithMessage("Promosyon bitiş tarihi boş olamaz.");
                RuleFor(p => p.ValidTo).GreaterThan(p => p.ValidFrom.Value)
                    .When(p => p.ValidFrom.HasValue && p.ValidTo.HasValue)
                    .WithMessage("Promosyon bitiş tarihi, başlangıç tarihinden sonra olmalıdır.");
            });
        }
    }
}
