using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrate.EntityFramework.Context;
using Entities.Concrate;

namespace DataAccess.Concrate.Dal
{
    public class EfContactDal : EfEntityRepositoryBase<Contact, FcdAccContext>, IContactDal
    {
    }
}
