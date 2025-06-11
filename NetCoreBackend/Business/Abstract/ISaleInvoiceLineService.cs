using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface ISaleInvoiceLineService
    {
        IDataResult<List<SaleInvoiceLine>> GetList();
        IDataResult<SaleInvoiceLine> GetById(int id);
        IResult Add(SaleInvoiceLine saleInvoiceLine);
        IResult Update(SaleInvoiceLine saleInvoiceLine);
        IResult Delete(SaleInvoiceLine saleInvoiceLine);

        //SECTION - Bulk

        IResult BulkAdd(List<SaleInvoiceLine> saleInvoiceLines);
        IResult BulkUpdate(List<SaleInvoiceLine> saleInvoiceLines);
        IResult BulkDeleteBySaleInvoiceId(int saleInvoiceId);
    }
}
