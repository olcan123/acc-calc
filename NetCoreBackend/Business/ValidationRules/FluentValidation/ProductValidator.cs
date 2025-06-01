using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Ürün adı boş olamaz.");
            RuleFor(p => p.Name).MaximumLength(100).WithMessage("Ürün adı en fazla 100 karakterden oluşmalıdır.");
            
            RuleFor(p => p.Description).MaximumLength(500).WithMessage("Ürün açıklaması en fazla 500 karakterden oluşmalıdır.");
            
            RuleFor(p => p.VatId).NotEmpty().WithMessage("KDV oranı seçilmelidir.");
            RuleFor(p => p.VatId).GreaterThan(0).WithMessage("Geçerli bir KDV oranı seçilmelidir.");
            
            RuleFor(p => p.UnitOfMeasureId).NotEmpty().WithMessage("Ölçü birimi seçilmelidir.");
            RuleFor(p => p.UnitOfMeasureId).GreaterThan(0).WithMessage("Geçerli bir ölçü birimi seçilmelidir.");
            
            // Vergi oranları pozitif veya 0 olmalıdır ve decimal(5,2) formatında
            RuleFor(p => p.CustomsTaxRate).GreaterThanOrEqualTo(0f).WithMessage("Gümrük vergisi oranı 0 veya daha büyük olmalıdır.");
            RuleFor(p => p.CustomsTaxRate).LessThanOrEqualTo(999.99f).WithMessage("Gümrük vergisi oranı 999.99'dan küçük olmalıdır.");
            
            RuleFor(p => p.ExciseTaxRate).GreaterThanOrEqualTo(0f).WithMessage("ÖTV oranı 0 veya daha büyük olmalıdır.");
            RuleFor(p => p.ExciseTaxRate).LessThanOrEqualTo(999.99f).WithMessage("ÖTV oranı 999.99'dan küçük olmalıdır.");
        }
    }
}
