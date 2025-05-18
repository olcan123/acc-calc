using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IPurchaseInvoiceLineService
    {
        IDataResult<List<PurchaseInvoiceLine>> GetList();
        IDataResult<List<PurchaseInvoiceLine>> GetListByInvoiceId(int invoiceId);
        IDataResult<PurchaseInvoiceLine> GetById(int id);
        IResult Add(PurchaseInvoiceLine purchaseInvoiceLine);
        IResult Update(PurchaseInvoiceLine purchaseInvoiceLine);
        IResult Delete(PurchaseInvoiceLine purchaseInvoiceLine);

        //SECTION - Bulk

        IResult BulkAdd(List<PurchaseInvoiceLine> purchaseInvoiceLines);
        IResult BulkUpdate(List<PurchaseInvoiceLine> purchaseInvoiceLines);
        IResult BulkDeleteByPurchaseInvoiceId(int purchaseInvoiceId);
    }
}
