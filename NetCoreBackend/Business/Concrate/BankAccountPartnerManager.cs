
using Business.Abstract;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class BankAccountPartnerManager : IBankAccountPartnerService
    {
        private readonly IBankAccountPartnerDal _bankAccountPartnerDal;
        private readonly IBankAccountService _bankAccountService;

        public BankAccountPartnerManager(IBankAccountPartnerDal bankAccountPartnerDal, IBankAccountService bankAccountService)
        {
            _bankAccountPartnerDal = bankAccountPartnerDal;
            _bankAccountService = bankAccountService;
        }

        public IDataResult<BankAccount> GetByBankAccountId(int bankAccountId)
        {
            var bankAccount = _bankAccountService.GetById(bankAccountId);
            return new SuccessDataResult<BankAccount>(bankAccount.Data);
        }

        public IDataResult<BankAccountPartner> GetByPartnerId(int partnerId)
        {
            var result = _bankAccountPartnerDal.Get(x => x.PartnerId == partnerId);
            return new SuccessDataResult<BankAccountPartner>(result);
        }

        public IDataResult<List<BankAccountPartner>> GetList()
        {
            var result = _bankAccountPartnerDal.GetAll();
            return new SuccessDataResult<List<BankAccountPartner>>(result);
        }

        public IDataResult<List<BankAccountPartner>> GetListIncludeByPartnerId(int partnerId)
        {
            var result = _bankAccountPartnerDal.GetAllWithIncludeChain(x => x.Include(x => x.BankAccount), x => x.PartnerId == partnerId);
            return new SuccessDataResult<List<BankAccountPartner>>(result);
        }

        public IDataResult<List<BankAccountPartner>> GetListIncludeByBankAccountId(int bankAccountId)
        {
            var result = _bankAccountPartnerDal.GetAllWithIncludeChain(x => x.Include(x => x.BankAccount), x => x.BankAccountId == bankAccountId);
            return new SuccessDataResult<List<BankAccountPartner>>(result);
        }

        public IDataResult<BankAccountPartner> GetIncludeByBankAccountIdAndPartnerId(int bankAccountId, int partnerId)
        {
            var result = _bankAccountPartnerDal.GetWithIncludeChain(x => x.Include(x => x.BankAccount), x => x.BankAccountId == bankAccountId && x.PartnerId == partnerId);
            return new SuccessDataResult<BankAccountPartner>(result);
        }

        public IResult Add(BankAccountPartner bankAccountPartner)
        {
            _bankAccountPartnerDal.Add(bankAccountPartner);
            return new SuccessResult("Banka Hesabı Eklendi");
        }

        public IResult Delete(BankAccountPartner bankAccountPartner)
        {
            _bankAccountPartnerDal.Delete(bankAccountPartner);
            return new SuccessResult("Banka Hesabı Silindi");
        }

        public IResult Update(BankAccountPartner bankAccountPartner)
        {
            _bankAccountPartnerDal.Update(bankAccountPartner);
            return new SuccessResult("Banka Hesabı Güncellendi");
        }

        [TransactionScopeAspect]
        public IResult AddBankAccountPartner(int partnerId, BankAccount bankAccount)
        {
            _bankAccountService.Add(bankAccount);
            var bankAccountPartner = new BankAccountPartner { BankAccountId = bankAccount.Id, PartnerId = partnerId };
            _bankAccountPartnerDal.Add(bankAccountPartner);
            return new SuccessResult("Banka Hesabı Eklendi");
        }

        public IResult DeleteBankAccountPartner(int partnerId, int bankAccountId)
        {
            var bankAccountPartner = _bankAccountPartnerDal.GetWithIncludeChain(
                x => x.Include(b => b.BankAccount),
                x => x.PartnerId == partnerId && x.BankAccountId == bankAccountId
            );

            _bankAccountService.Delete(bankAccountPartner.BankAccount);
            return new SuccessResult("Banka Hesabı Silindi");
        }

        [TransactionScopeAspect]
        public IResult UpdateBankAccountPartner(int partnerId, BankAccount bankAccount)
        {
            _bankAccountService.Update(bankAccount);
            return new SuccessResult("Banka Hesabı Güncellendi");
        }
    }
}
