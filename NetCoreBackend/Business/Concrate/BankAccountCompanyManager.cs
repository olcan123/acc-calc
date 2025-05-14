using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class BankAccountCompanyManager : IBankAccountCompanyService
    {
        private readonly IBankAccountCompanyDal _bankAccountCompanyDal;
        private readonly IBankAccountService _bankAccountService;

        public BankAccountCompanyManager(IBankAccountCompanyDal bankAccountCompanyDal, IBankAccountService bankAccountService)
        {
            _bankAccountCompanyDal = bankAccountCompanyDal;
            _bankAccountService = bankAccountService;
        }

        //SECTION - Bu sadece bankAccountManager kullanilarak yapildi...
        public IDataResult<BankAccount> GetByBankAccountId(int bankAccountId)
        {
            var bankAccount = _bankAccountService.GetById(bankAccountId);
            return new SuccessDataResult<BankAccount>(bankAccount.Data);
        }


        //NOTE - Company Bank Account veri listelenme islemleri icin gerekli metot

        public IDataResult<BankAccountCompany> GetByCompanyId(int companyId)
        {
            var bankAccountCompany = _bankAccountCompanyDal.Get(x => x.CompanyId == companyId);
            return new SuccessDataResult<BankAccountCompany>(bankAccountCompany);
        }

        public IDataResult<List<BankAccountCompany>> GetList()
        {
            var bankAccountCompanies = _bankAccountCompanyDal.GetAll();
            return new SuccessDataResult<List<BankAccountCompany>>(bankAccountCompanies);
        }

        public IDataResult<List<BankAccountCompany>> GetListIncludeByCompanyId(int companyId) //BankAccountCompany();
        {
            var bankAccountCompanies = _bankAccountCompanyDal.GetAllWithIncludeChain(x => x.Include(x => x.BankAccount), x => x.CompanyId == companyId);
            return new SuccessDataResult<List<BankAccountCompany>>(bankAccountCompanies);
        }
        public IDataResult<List<BankAccountCompany>> GetListIncludeByBankAccountId(int bankAccountId)
        {
            var bankAccountCompanies = _bankAccountCompanyDal.GetAllWithIncludeChain(x => x.Include(x => x.BankAccount), x => x.BankAccountId == bankAccountId);
            return new SuccessDataResult<List<BankAccountCompany>>(bankAccountCompanies);
        }

        public IDataResult<BankAccountCompany> GetIncludeByBankAccountIdAndCompanyId(int bankAccountId, int companyId)
        {
            var bankAccountCompany = _bankAccountCompanyDal.GetWithIncludeChain(x => x.Include(x => x.BankAccount), x => x.BankAccountId == bankAccountId && x.CompanyId == companyId);
            return new SuccessDataResult<BankAccountCompany>(bankAccountCompany);
        }



        //NOTE - BankAccountCompany veri ekleme, silme ve guncelleme islemleri icin gerekli metot
        public IResult Add(BankAccountCompany bankAccountCompany)
        {
            _bankAccountCompanyDal.Add(bankAccountCompany);
            return new SuccessResult("Banka Hesabı Eklendi");
        }

        public IResult Delete(BankAccountCompany bankAccountCompany)
        {
            _bankAccountCompanyDal.Delete(bankAccountCompany);
            return new SuccessResult("Banka Hesabı Silindi");
        }


        public IResult Update(BankAccountCompany bankAccountCompany)
        {
            _bankAccountCompanyDal.Update(bankAccountCompany);
            return new SuccessResult("Banka Hesabı Güncellendi");
        }



        //NOTE - Company Bank Account veri ekleme, silme ve guncelleme islemleri icin gerekli metot

        [TransactionScopeAspect]
        public IResult AddBankAccountCompany(int companyId, BankAccount bankAccount)
        {
            _bankAccountService.Add(bankAccount);
            var bankAccountCompany = new BankAccountCompany { BankAccountId = bankAccount.Id, CompanyId = companyId };
            _bankAccountCompanyDal.Add(bankAccountCompany);
            return new SuccessResult("Banka Hesabı Eklendi");
        }

        public IResult DeleteBankAccountCompany(int companyId, int bankAccountId)
        {
            var bankAccountCompany = _bankAccountCompanyDal.GetWithIncludeChain(
                x => x.Include(b => b.BankAccount),
                x => x.CompanyId == companyId && x.BankAccountId == bankAccountId);

            // Önce BankAccount'u sil 
            //NOTE - BankAccount silerken iliskili oldugu BankAccountCompany de siler
            _bankAccountService.Delete(bankAccountCompany.BankAccount);

            // Ardından ara tabloyuda siliyor bu yuzden bunu kaldirdim
            // _bankAccountCompanyDal.Delete(bankAccountCompany);

            return new SuccessResult("Banka Hesabı Silindi");
        }


        [TransactionScopeAspect]
        public IResult UpdateBankAccountCompany(int companyId, BankAccount bankAccount)
        {
            _bankAccountService.Update(bankAccount);
            return new SuccessResult("Banka Hesabı Güncellendi");
        }
    }
}