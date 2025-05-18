using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class AccountTypeManager : IAccountTypeService
    {
        private readonly IAccountTypeDal _accountTypeDal;

        public AccountTypeManager(IAccountTypeDal accountTypeDal)
        {
            _accountTypeDal = accountTypeDal;
        }
        
        //
        // Async Methods
        //
        
        public async Task<IDataResult<List<AccountType>>> GetListAsync()
        {
            var result = await _accountTypeDal.GetAllAsync();
            return new SuccessDataResult<List<AccountType>>(result);
        }
        
        //
        // Sync Methods
        //
        
        public IDataResult<List<AccountType>> GetList()
        {
            var result = _accountTypeDal.GetAll();
            return new SuccessDataResult<List<AccountType>>(result);
        }

        public IDataResult<AccountType> GetById(short id)
        {
            var result = _accountTypeDal.Get(x => x.Id == id);
            return new SuccessDataResult<AccountType>(result);
        }


        public IResult Add(AccountType accountType)
        {
            _accountTypeDal.Add(accountType);
            return new SuccessResult("Hesap türü eklendi");
        }


        public IResult Update(AccountType accountType)
        {
            _accountTypeDal.Update(accountType);
            return new SuccessResult("Hesap türü güncellendi");
        }

        public IResult Delete(AccountType accountType)
        {
            _accountTypeDal.Delete(accountType);
            return new SuccessResult("Hesap türü silindi");
        }
    }
}
