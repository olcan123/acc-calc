using Core.Utilities.Results;
using Entities.Concrate;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IBankAccountService
    {
        IDataResult<List<BankAccount>> GetAll();
        IDataResult<BankAccount> Get(int id);
        IResult Add(BankAccount bankAccount);
        IResult AddWithCompany(BankAccount bankAccount, int companyId, string companyName);
        IResult Update(BankAccount bankAccount);
        IResult Delete(BankAccount bankAccount);
    }
}
