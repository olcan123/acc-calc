using System;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductCategoryValidator : AbstractValidator<ProductCategory>
    {
        public ProductCategoryValidator()
        {
            RuleFor(pc => pc.ProductId).NotEmpty().WithMessage("Ürün seçilmelidir.");
            RuleFor(pc => pc.ProductId).GreaterThan(0).WithMessage("Geçerli bir ürün seçilmelidir.");
            
            RuleFor(pc => pc.CategoryId).NotEmpty().WithMessage("Kategori seçilmelidir.");
            RuleFor(pc => pc.CategoryId).GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir.");
        }
    }
}
