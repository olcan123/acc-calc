using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface ISaleInvoiceService
    {
        IDataResult<List<SaleInvoice>> GetList();
        IDataResult<List<SaleInvoice>> GetListWithIncludes();
        IDataResult<List<SaleInvoice>> GetListByPartnerId(int partnerId);
        IDataResult<SaleInvoice> GetById(int id);
        IDataResult<SaleInvoice> GetByIdWithDetails(int id);
        IResult Add(SaleInvoice saleInvoice);
        IResult Update(SaleInvoice saleInvoice);
        IResult Delete(SaleInvoice saleInvoice);

        //SECTION - Sale Invoice
        IResult AddInvoice(Ledger ledger, SaleInvoice saleInvoice, List<SaleInvoiceLine> saleInvoiceLines);
        IResult DeleteInvoice(int saleInvoiceId);
        IResult UpdateInvoice(Ledger ledger, SaleInvoice saleInvoice, List<SaleInvoiceLine> saleInvoiceLines);
    }
}
