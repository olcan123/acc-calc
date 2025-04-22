using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using System.Collections.Generic;

namespace Business.Concrate
{
    public class BankManager : IBankService
    {
        private readonly IBankDal _bankDal;

        public BankManager(IBankDal bankDal)
        {
            _bankDal = bankDal;
        }

        public IDataResult<List<Bank>> GetAll()
        {
            var banks = _bankDal.GetAll();
            return new SuccessDataResult<List<Bank>>(banks);
        }

        public IDataResult<Bank> Get(int id)
        {
            var bank = _bankDal.Get(x => x.Id == id);
            return new SuccessDataResult<Bank>(bank);
        }

        public IResult Add(Bank bank)
        {
            _bankDal.AddWithChildren(bank);
            return new SuccessResult("Bank Added");
        }

        public IResult Delete(Bank bank)
        {
            _bankDal.Delete(bank);
            return new SuccessResult("Bank Deleted");
        }

        public IResult Update(Bank bank)
        {
            _bankDal.Update(bank);
            return new SuccessResult("Bank Updated");
        }
    }
}
