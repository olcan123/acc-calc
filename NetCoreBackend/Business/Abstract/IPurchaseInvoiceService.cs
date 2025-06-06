using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IPurchaseInvoiceService
    {
        IDataResult<List<PurchaseInvoice>> GetList();
        IDataResult<List<PurchaseInvoice>> GetListWithIncludes();
        IDataResult<List<PurchaseInvoice>> GetListByPartnerId(int partnerId);
        IDataResult<PurchaseInvoice> GetById(int id);
        IDataResult<PurchaseInvoice> GetByIdWithDetails(int id);
        IResult Add(PurchaseInvoice purchaseInvoice);
        IResult Update(PurchaseInvoice purchaseInvoice);
        IResult Delete(PurchaseInvoice purchaseInvoice);

        //SECTION - Purchase Invoice
        IResult AddInvoice(Ledger ledger, PurchaseInvoice purchaseInvoice, List<PurchaseInvoiceLine> purchaseInvoiceLines, List<PurchaseInvoiceExpense> purchaseInvoiceExpenses);
        IResult DeleteInvoice(int purchaseInvoiceId);
        IResult UpdateInvoice(Ledger ledger, PurchaseInvoice purchaseInvoice, List<PurchaseInvoiceLine> purchaseInvoiceLines, List<PurchaseInvoiceExpense> purchaseInvoiceExpenses);
    }
}
