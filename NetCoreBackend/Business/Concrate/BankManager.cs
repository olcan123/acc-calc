using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class BankManager : IBankService
    {
        private readonly IBankDal _bankDal;

        public BankManager(IBankDal bankDal)
        {
            _bankDal = bankDal;
        }

        //
        // Async Methods
        //
        
        public async Task<IDataResult<List<Bank>>> GetListAsync()
        {
            var banks = await _bankDal.GetAllAsync();
            return new SuccessDataResult<List<Bank>>(banks);
        }
        
        //
        // Sync Methods
        //

        public IDataResult<Bank> GetById(int id)
        {
            var bank = _bankDal.Get(x => x.Id == id);
            return new SuccessDataResult<Bank>(bank);
        }

        public IDataResult<List<Bank>> GetList()
        {
            var banks = _bankDal.GetAll();
            return new SuccessDataResult<List<Bank>>(banks);
        }

        [ValidationAspect(typeof(BankValidator), Priority = 1)]
        public IResult Add(Bank bank)
        {
            _bankDal.Add(bank);
            return new SuccessResult("Banka Basariyla Eklendi");
        }

        public IResult Delete(Bank bank)
        {
            _bankDal.Delete(bank);
            return new SuccessResult("Banka Basariyla Silindi");
        }

        [ValidationAspect(typeof(BankValidator), Priority = 1)]
        public IResult Update(Bank bank)
        {
            _bankDal.Update(bank);
            return new SuccessResult("Banka Basariyla Güncellendi");
        }
    }
}