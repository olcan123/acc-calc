using Core.DataAccess;
using Entities.Concrate;

namespace DataAccess.Abstract
{
    public interface IAccountTypeDal : IEntityRepository<AccountType>
    {
        // İhtiyaca göre özel metotlar eklenebilir.
    }
}