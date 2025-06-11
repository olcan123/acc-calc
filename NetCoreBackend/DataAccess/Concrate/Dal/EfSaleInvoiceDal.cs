using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrate.EntityFramework.Context;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrate.Dal
{
    public class EfSaleInvoiceDal : EfEntityRepositoryBase<SaleInvoice, FcdAccContext>, ISaleInvoiceDal
    {

    }
}
