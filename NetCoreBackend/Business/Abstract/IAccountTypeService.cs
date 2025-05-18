using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IAccountTypeService
    {
        IDataResult<List<AccountType>> GetList();
        IDataResult<AccountType> GetById(short id);
        IResult Add(AccountType accountType);
        IResult Update(AccountType accountType);
        IResult Delete(AccountType accountType);
    }
}
