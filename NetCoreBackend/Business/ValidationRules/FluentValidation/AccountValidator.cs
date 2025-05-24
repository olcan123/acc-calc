using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(a => a.Code).NotEmpty().WithMessage("Hesap kodu boş olamaz.");
            RuleFor(a => a.Code).MaximumLength(20).WithMessage("Hesap kodu en fazla 20 karakter olabilir.");
            
            RuleFor(a => a.Name).NotEmpty().WithMessage("Hesap adı boş olamaz.");
            RuleFor(a => a.Name).MaximumLength(100).WithMessage("Hesap adı en fazla 100 karakter olabilir.");
            
            RuleFor(a => a.AccountType).IsInEnum().WithMessage("Geçerli bir hesap türü seçilmelidir.");
            
            RuleFor(a => a.NormalBalance).IsInEnum()
                .When(a => a.NormalBalance.HasValue)
                .WithMessage("Geçerli bir normal bakiye türü seçilmelidir.");
            
            // Description opsiyonel olabilir ancak bir uzunluk sınırı koyabiliriz
            RuleFor(a => a.Description).MaximumLength(500)
                .When(a => !string.IsNullOrEmpty(a.Description))
                .WithMessage("Açıklama en fazla 500 karakter olabilir.");
            
            // Üst hesabın, kendi kendisine atanmasını önleme (ParentAccountId > 0 olduğunda)
            RuleFor(a => a.ParentAccountId)
                .NotEqual(a => a.Id)
                .When(a => a.ParentAccountId.HasValue)
                .WithMessage("Bir hesap kendi kendisinin üst hesabı olamaz.");
                
            // İş mantığı kuralları için özel bir validator ekleme
            RuleFor(a => a)
                .Must(BePostableWhenNoChildren)
                .WithMessage("Alt hesapları olan bir hesap için 'IsPostable' özelliği false olmalıdır.");
        }
        
        private bool BePostableWhenNoChildren(Account account)
        {
            // Eğer hesabın alt hesapları varsa ve hesap kayıt yapılabilir durumdaysa false dön
            // Account oluşturulurken Children koleksiyonu henüz doldurulmamış olabilir, bu nedenle
            // bu validasyon daha sonra servis katmanında veya DbContext seviyesinde kontrol edilmelidir
            return !(account.Children?.Any() == true && account.IsPostable);
        }
    }
}
