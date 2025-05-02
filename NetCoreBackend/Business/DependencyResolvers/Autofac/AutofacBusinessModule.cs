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

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<CompanyManager>().As<ICompanyService>();
            builder.RegisterType<EfCompanyDal>().As<ICompanyDal>();

            builder.RegisterType<WarehouseManager>().As<IWarehouseService>();
            builder.RegisterType<EfWarehouseDal>().As<IWarehouseDal>();

            builder.RegisterType<BankAccountManager>().As<IBankAccountService>();
            builder.RegisterType<EfBankAccountDal>().As<IBankAccountDal>();

            builder.RegisterType<BankAccountCompanyManager>().As<IBankAccountCompanyService>();
            builder.RegisterType<EfBankAccountCompanyDal>().As<IBankAccountCompanyDal>();

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

            builder.RegisterType<EfBankDal>().As<IBankDal>();
            builder.RegisterType<BankManager>().As<IBankService>();


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
