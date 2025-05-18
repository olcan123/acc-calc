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
    public class CurrencyManager : ICurrencyService
    {
        private readonly ICurrencyDal _currencyDal;

        public CurrencyManager(ICurrencyDal currencyDal)
        {
            _currencyDal = currencyDal;
        }
        
        //
        // Async Methods
        //
        
        public async Task<IDataResult<List<Currency>>> GetListAsync()
        {
            var result = await _currencyDal.GetAllAsync();
            return new SuccessDataResult<List<Currency>>(result);
        }
        
        //
        // Sync Methods
        //
        
        public IDataResult<List<Currency>> GetList()
        {
            var result = _currencyDal.GetAll();
            return new SuccessDataResult<List<Currency>>(result);
        }

        public IDataResult<Currency> GetById(int id)
        {
            var result = _currencyDal.Get(x => x.Id == id);
            return new SuccessDataResult<Currency>(result);
        }

        public IDataResult<Currency> GetByCode(string code)
        {

            var result = _currencyDal.Get(x => x.Code == code);
            return new SuccessDataResult<Currency>(result);
        }

        public IResult Add(Currency currency)
        {
            _currencyDal.Add(currency);
            return new SuccessResult("Para birimi eklendi");
        }

        public IResult Update(Currency currency)
        {
            _currencyDal.Update(currency);
            return new SuccessResult("Para birimi güncellendi");
        }

        public IResult Delete(Currency currency)
        {
            _currencyDal.Delete(currency);
            return new SuccessResult("Para birimi silindi");
        }
    }
}
