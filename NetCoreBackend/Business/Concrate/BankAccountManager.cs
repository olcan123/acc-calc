using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using System.Collections.Generic;

namespace Business.Concrate
{
    public class BankAccountManager : IBankAccountService
    {
        private readonly IBankAccountDal _bankAccountDal;

        public BankAccountManager(IBankAccountDal bankAccountDal)
        {
            _bankAccountDal = bankAccountDal;
        }

        public IDataResult<List<BankAccount>> GetAll()
        {
            var accounts = _bankAccountDal.GetAll();
            return new SuccessDataResult<List<BankAccount>>(accounts);
        }

        public IDataResult<BankAccount> Get(int id)
        {
            var account = _bankAccountDal.Get(x => x.Id == id);
            return new SuccessDataResult<BankAccount>(account);
        }

        public IResult Add(BankAccount bankAccount)
        {
            _bankAccountDal.Add(bankAccount);
            return new SuccessResult("Bank account added.");
        }

        public IResult Delete(BankAccount bankAccount)
        {
            _bankAccountDal.Delete(bankAccount);
            return new SuccessResult("Bank account deleted.");
        }

        public IResult Update(BankAccount bankAccount)
        {
            _bankAccountDal.Update(bankAccount);
            return new SuccessResult("Bank account updated.");
        }

        public IResult AddWithCompany(BankAccount bankAccount, int companyId, string companyName)
        {
            _bankAccountDal.AddWithExistingRelation<Company>(bankAccount, companyId, companyName);
            return new SuccessResult("Bank account added.");
        }
    }
}
