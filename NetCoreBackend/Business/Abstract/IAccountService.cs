using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IAccountService
    {
        IDataResult<List<Account>> GetList();
        IDataResult<Account> GetById(int id);
        IDataResult<List<Account>> GetByParentId(int parentId);
        IResult Add(Account account);
        IResult Update(Account account);
        IResult Delete(Account account);
    }
}
