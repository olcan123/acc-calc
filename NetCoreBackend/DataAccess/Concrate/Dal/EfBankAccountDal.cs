using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrate.EntityFramework.Context;
using Entities.Concrate;

namespace DataAccess.Concrate.Dal
{
    public class EfBankAccountDal : EfEntityRepositoryBase<BankAccount, FcdAccContext>, IBankAccountDal
    {
    }
}
