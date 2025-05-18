using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrate;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrate;
using DataAccess.Concrate.Dal;
using DataAccess.Concrate.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // SECTION: Kullanıcı ve Kimlik Doğrulama
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            // SECTION: Şirket ve Depo
            builder.RegisterType<CompanyManager>().As<ICompanyService>();
            builder.RegisterType<EfCompanyDal>().As<ICompanyDal>();

            builder.RegisterType<WarehouseManager>().As<IWarehouseService>();
            builder.RegisterType<EfWarehouseDal>().As<IWarehouseDal>();

            // SECTION: Banka ve Hesaplar
            builder.RegisterType<BankAccountManager>().As<IBankAccountService>();
            builder.RegisterType<EfBankAccountDal>().As<IBankAccountDal>();

            builder.RegisterType<BankAccountCompanyManager>().As<IBankAccountCompanyService>();
            builder.RegisterType<EfBankAccountCompanyDal>().As<IBankAccountCompanyDal>();

            builder.RegisterType<EfBankDal>().As<IBankDal>();
            builder.RegisterType<BankManager>().As<IBankService>();

            // SECTION: Adres ve İletişim
            builder.RegisterType<AddressManager>().As<IAddressService>();
            builder.RegisterType<EfAddressDal>().As<IAddressDal>();

            builder.RegisterType<AddressWarehouseManager>().As<IAddressWarehouseService>();
            builder.RegisterType<EfAddressWarehouseDal>().As<IAddressWarehouseDal>();

            builder.RegisterType<EfContactDal>().As<IContactDal>();
            builder.RegisterType<ContactManager>().As<IContactService>();

            builder.RegisterType<EfContactDetailDal>().As<IContactDetailDal>();
            builder.RegisterType<ContactDetailManager>().As<IContactDetailService>();

            builder.RegisterType<EfContactWarehouseDal>().As<IContactWarehouseDal>();
            builder.RegisterType<ContactWarehouseManager>().As<IContactWarehouseService>();

            // SECTION: İş Ortakları
            builder.RegisterType<EfPartnerDal>().As<IPartnerDal>();
            builder.RegisterType<PartnerManager>().As<IPartnerService>();

            builder.RegisterType<EfAddressPartnerDal>().As<IAddressPartnerDal>();
            builder.RegisterType<AddressPartnerManager>().As<IAddressPartnerService>();

            builder.RegisterType<EfBankAccountPartnerDal>().As<IBankAccountPartnerDal>();
            builder.RegisterType<BankAccountPartnerManager>().As<IBankAccountPartnerService>();

            builder.RegisterType<EfContactPartnerDal>().As<IContactPartnerDal>();
            builder.RegisterType<ContactPartnerManager>().As<IContactPartnerService>();

            // SECTION: Ürün ve Stok
            builder.RegisterType<EfVatDal>().As<IVatDal>();
            builder.RegisterType<VatManager>().As<IVatService>();

            builder.RegisterType<EfUnitOfMeasureDal>().As<IUnitOfMeasureDal>();
            builder.RegisterType<UnitOfMeasureManager>().As<IUnitOfMeasureService>();

            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>();
            builder.RegisterType<CategoryManager>().As<ICategoryService>();

            builder.RegisterType<EfProductDal>().As<IProductDal>();
            builder.RegisterType<ProductManager>().As<IProductService>();

            builder.RegisterType<EfProductPriceDal>().As<IProductPriceDal>();
            builder.RegisterType<ProductPriceManager>().As<IProductPriceService>();

            builder.RegisterType<EfBarcodeDal>().As<IBarcodeDal>();
            builder.RegisterType<BarcodeManager>().As<IBarcodeService>();

            builder.RegisterType<EfProductCategoryDal>().As<IProductCategoryDal>();
            builder.RegisterType<ProductCategoryManager>().As<IProductCategoryService>();

            // SECTION: Fatura ve Muhasebe
            builder.RegisterType<EfAccountDal>().As<IAccountDal>();
            builder.RegisterType<AccountManager>().As<IAccountService>();

            builder.RegisterType<EfAccountTypeDal>().As<IAccountTypeDal>();
            builder.RegisterType<AccountTypeManager>().As<IAccountTypeService>();

            builder.RegisterType<EfCurrencyDal>().As<ICurrencyDal>();
            builder.RegisterType<CurrencyManager>().As<ICurrencyService>();

            builder.RegisterType<EfLedgerDal>().As<ILedgerDal>();
            builder.RegisterType<LedgerManager>().As<ILedgerService>();

            builder.RegisterType<EfLedgerEntryDal>().As<ILedgerEntryDal>();
            builder.RegisterType<LedgerEntryManager>().As<ILedgerEntryService>();

            builder.RegisterType<EfPurchaseInvoiceDal>().As<IPurchaseInvoiceDal>();
            builder.RegisterType<PurchaseInvoiceManager>().As<IPurchaseInvoiceService>();

            builder.RegisterType<EfPurchaseInvoiceLineDal>().As<IPurchaseInvoiceLineDal>();
            builder.RegisterType<PurchaseInvoiceLineManager>().As<IPurchaseInvoiceLineService>();

            builder.RegisterType<EfPurchaseInvoiceExpenseDal>().As<IPurchaseInvoiceExpenseDal>();
            builder.RegisterType<PurchaseInvoiceExpenseManager>().As<IPurchaseInvoiceExpenseService>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
