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
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class AccountManager : IAccountService
    {
        private readonly IAccountDal _accountDal;

        public AccountManager(IAccountDal accountDal)
        {
            _accountDal = accountDal;
        }

        //
        // Async Methods
        //
        
        public async Task<IDataResult<List<Account>>> GetListAsync()
        {
            var result = await _accountDal.GetAllAsync();
            return new SuccessDataResult<List<Account>>(result);
        }
        
        //
        // Sync Methods
        //
        
        public IDataResult<List<Account>> GetList()
        {
            var result = _accountDal.GetAll();
            return new SuccessDataResult<List<Account>>(result);
        }

        public IDataResult<Account> GetById(int id)
        {
            var result = _accountDal.Get(x => x.Id == id);
            return new SuccessDataResult<Account>(result);
        }

        public IDataResult<List<Account>> GetByParentId(int parentId)
        {
            var result = _accountDal.GetAll(x => x.ParentAccountId == parentId);
            return new SuccessDataResult<List<Account>>(result);
        }

        public IResult Add(Account account)
        {

            _accountDal.Add(account);
            return new SuccessResult("Hesap eklendi");
        }

        public IResult Update(Account account)
        {

            _accountDal.Update(account);
            return new SuccessResult("Hesap güncellendi");
        }

        public IResult Delete(Account account)
        {
            _accountDal.Delete(account);
            return new SuccessResult("Hesap silindi");
        }
    }
}
