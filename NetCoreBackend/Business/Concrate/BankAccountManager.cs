using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using EFCore.BulkExtensions;
using Entities.Concrate;

namespace Business.Concrate
{
    public class BankAccountManager : IBankAccountService
    {
        private readonly IBankAccountDal _bankAccountDal;

        public BankAccountManager(IBankAccountDal bankAccountDal)
        {
            _bankAccountDal = bankAccountDal;
        }

        public IDataResult<BankAccount> GetById(int id)
        {
            var result = _bankAccountDal.Get(c => c.Id == id);
            return new SuccessDataResult<BankAccount>(result);
        }

        public IDataResult<List<BankAccount>> GetList()
        {
            var result = _bankAccountDal.GetAll();
            return new SuccessDataResult<List<BankAccount>>(result);
        }

        [ValidationAspect(typeof(BankAccountValidator), Priority = 1)]
        public IResult Add(BankAccount bankAccount)
        {
            _bankAccountDal.Add(bankAccount);
            return new SuccessResult("Banka Hesabı Eklendi");
        }

        [ValidationAspect(typeof(BankAccountValidator), Priority = 1)]
        public IResult AddBulk(List<BankAccount> bankAccounts)
        {
            _bankAccountDal.BulkAdd(bankAccounts, new BulkConfig { SetOutputIdentity = true });
            return new SuccessResult("Banka Hesabı Eklendi");
        }

        public IResult Delete(BankAccount bankAccount)
        {
            _bankAccountDal.Delete(bankAccount);
            return new SuccessResult("Banka Hesabı Silindi");
        }

        [ValidationAspect(typeof(BankAccountValidator), Priority = 1)]
        public IResult Update(BankAccount bankAccount)
        {
            _bankAccountDal.Update(bankAccount);
            return new SuccessResult("Banka Hesabı Güncellendi");
        }

        [ValidationAspect(typeof(BankAccountValidator), Priority = 1)]
        public IResult UpdateBulk(List<BankAccount> bankAccounts)
        {
            _bankAccountDal.BulkUpdate(bankAccounts);
            return new SuccessResult("Banka Hesabı Güncellendi");
        }
    }
}