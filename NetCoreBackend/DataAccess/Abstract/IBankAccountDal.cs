using Core.DataAccess;
using Entities.Concrate;

namespace DataAccess.Abstract
{
    public interface IBankAccountDal : IEntityRepository<BankAccount>
    {
        void AddWithCompany(int companyId, BankAccount bankAccount);
    }
}
