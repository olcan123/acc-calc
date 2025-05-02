using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IBankAccountService
    {
        IDataResult<List<BankAccount>> GetList();
        IDataResult<BankAccount> GetById(int id);
        IResult Add(BankAccount bankAccount);
        IResult AddBulk(List<BankAccount> bankAccounts);
        IResult Delete(BankAccount bankAccount);
        IResult Update(BankAccount bankAccount);
        IResult UpdateBulk(List<BankAccount> bankAccounts);
    }
}