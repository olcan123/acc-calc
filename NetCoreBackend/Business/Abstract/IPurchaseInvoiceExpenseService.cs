using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IPurchaseInvoiceExpenseService
    {
        IDataResult<List<PurchaseInvoiceExpense>> GetList();
        IDataResult<List<PurchaseInvoiceExpense>> GetListByInvoiceId(int invoiceId);
        IDataResult<PurchaseInvoiceExpense> GetById(int id);
        IResult Add(PurchaseInvoiceExpense purchaseInvoiceExpense);
        IResult Update(PurchaseInvoiceExpense purchaseInvoiceExpense);
        IResult Delete(PurchaseInvoiceExpense purchaseInvoiceExpense);

        //SECTION - Bulk
        IResult BulkAdd(List<PurchaseInvoiceExpense> purchaseInvoiceExpenses);
        IResult BulkUpdate(List<PurchaseInvoiceExpense> purchaseInvoiceExpenses);
        IResult BulkDeleteByPurchaseInvoiceId(int purchaseInvoiceId);
    }
}
