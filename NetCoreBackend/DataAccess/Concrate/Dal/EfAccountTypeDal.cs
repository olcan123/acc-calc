using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrate.EntityFramework.Context;
using Entities.Concrate;

namespace DataAccess.Concrate.Dal
{
    public class EfAccountTypeDal : EfEntityRepositoryBase<AccountType, FcdAccContext>, IAccountTypeDal
    {
        // IAccountTypeDal interface'inden gelen metotlar burada implemente edilir.
    }
}