using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class SaleInvoiceValidator : AbstractValidator<SaleInvoice>
    {
        public SaleInvoiceValidator()
        {
            RuleFor(s => s.InvoiceNo).NotEmpty().WithMessage("Fatura numarası boş olamaz");
            RuleFor(s => s.InvoiceNo).MaximumLength(50).WithMessage("Fatura numarası en fazla 50 karakter olabilir");
            
            RuleFor(s => s.InvoiceDate).NotEmpty().WithMessage("Fatura tarihi boş olamaz");
            
            RuleFor(s => s.PartnerId).GreaterThan(0).WithMessage("Partner seçimi zorunludur");
            
            RuleFor(s => s.CustomerAccountId).GreaterThan(0).WithMessage("Müşteri hesabı seçimi zorunludur");
            
            RuleFor(s => s.SaleInvoiceType).IsInEnum().WithMessage("Geçerli bir fatura türü seçiniz");
            
            RuleFor(s => s.CashPaymentAmount).GreaterThanOrEqualTo(0).WithMessage("Nakit ödeme tutarı negatif olamaz");
        }
    }
}
