using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class SaleInvoiceLineValidator : AbstractValidator<SaleInvoiceLine>
    {
        public SaleInvoiceLineValidator()
        {
            RuleFor(sl => sl.SaleInvoiceId).GreaterThan(0).WithMessage("Fatura ID'si geçersiz");
            
            RuleFor(sl => sl.ProductId).GreaterThan(0).WithMessage("Ürün seçimi zorunludur");
            
            RuleFor(sl => sl.WarehouseId).GreaterThan(0).WithMessage("Depo seçimi zorunludur");
            
            RuleFor(sl => sl.UnitOfMeasureId).GreaterThan(0).WithMessage("Ölçü birimi seçimi zorunludur");
            
            RuleFor(sl => sl.SaleAccountId).GreaterThan(0).WithMessage("Satış hesabı seçimi zorunludur");
            
            RuleFor(sl => sl.VatId).GreaterThan(0).WithMessage("KDV seçimi zorunludur");
            
            RuleFor(sl => sl.Quantity).GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalıdır");
            
            RuleFor(sl => sl.UnitPrice).GreaterThan(0).WithMessage("Birim fiyat sıfırdan büyük olmalıdır");
            
            RuleFor(sl => sl.Amount).GreaterThan(0).WithMessage("Tutar sıfırdan büyük olmalıdır");
            
            RuleFor(sl => sl.TotalPrice).GreaterThan(0).WithMessage("Toplam fiyat sıfırdan büyük olmalıdır");
            
            RuleFor(sl => sl.DiscountRate).GreaterThanOrEqualTo(0).WithMessage("İndirim oranı negatif olamaz");
            RuleFor(sl => sl.DiscountRate).LessThanOrEqualTo(100).WithMessage("İndirim oranı %100'den fazla olamaz");
            
            RuleFor(sl => sl.DiscountAmount).GreaterThanOrEqualTo(0).WithMessage("İndirim tutarı negatif olamaz");
            
            RuleFor(sl => sl.VatTaxAmount).GreaterThanOrEqualTo(0).WithMessage("KDV tutarı negatif olamaz");
            
            RuleFor(sl => sl.Description).MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir");
        }
    }
}
