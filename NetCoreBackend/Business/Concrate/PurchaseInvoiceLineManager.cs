using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class PurchaseInvoiceLineManager : IPurchaseInvoiceLineService
    {
        private readonly IPurchaseInvoiceLineDal _purchaseInvoiceLineDal;


        public PurchaseInvoiceLineManager(IPurchaseInvoiceLineDal purchaseInvoiceLineDal)
        {
            _purchaseInvoiceLineDal = purchaseInvoiceLineDal;


        }

        public IDataResult<List<PurchaseInvoiceLine>> GetList()
        {
            var result = _purchaseInvoiceLineDal.GetAll();
            return new SuccessDataResult<List<PurchaseInvoiceLine>>(result);
        }

        public IDataResult<List<PurchaseInvoiceLine>> GetListByInvoiceId(int invoiceId)
        {
            var result = _purchaseInvoiceLineDal.GetAll(x => x.PurchaseInvoiceId == invoiceId);
            return new SuccessDataResult<List<PurchaseInvoiceLine>>(result);
        }

        public IDataResult<PurchaseInvoiceLine> GetById(int id)
        {
            var result = _purchaseInvoiceLineDal.Get(x => x.Id == id);
            return new SuccessDataResult<PurchaseInvoiceLine>(result);
        }

        public IResult Add(PurchaseInvoiceLine purchaseInvoiceLine)
        {
            _purchaseInvoiceLineDal.Add(purchaseInvoiceLine);
            return new SuccessResult("Fatura satırı eklendi");
        }

        public IResult Update(PurchaseInvoiceLine purchaseInvoiceLine)
        {
            _purchaseInvoiceLineDal.Update(purchaseInvoiceLine);
            return new SuccessResult("Fatura satırı güncellendi");
        }

        public IResult Delete(PurchaseInvoiceLine purchaseInvoiceLine)
        {
            _purchaseInvoiceLineDal.Delete(purchaseInvoiceLine);
            return new SuccessResult("Fatura satırı silindi");
        }

        //SECTION - Bulk

        public IResult BulkAdd(List<PurchaseInvoiceLine> purchaseInvoiceLines)
        {
            _purchaseInvoiceLineDal.BulkAdd(purchaseInvoiceLines);
            return new SuccessResult("Fatura satırları eklendi");
        }

        public IResult BulkUpdate(List<PurchaseInvoiceLine> purchaseInvoiceLines)
        {
            var purchaseInvoiceId = purchaseInvoiceLines.FirstOrDefault()?.PurchaseInvoiceId;
            if (purchaseInvoiceId == null)
                return new ErrorResult("Fatura ID'si bulunamadı");
                
            _purchaseInvoiceLineDal.MergeSync(purchaseInvoiceLines,
            (source, target) => source.Id == target.Id,
             target => target.PurchaseInvoiceId == purchaseInvoiceId.Value);
            return new SuccessResult("Fatura satırları güncellendi");
        }

        public IResult BulkDeleteByPurchaseInvoiceId(int purchaseInvoiceId)
        {
            var purchaseInvoiceLines = _purchaseInvoiceLineDal.GetAll(x => x.PurchaseInvoiceId == purchaseInvoiceId);
            if (purchaseInvoiceLines != null && purchaseInvoiceLines.Count > 0)
            {
                _purchaseInvoiceLineDal.BulkDelete(purchaseInvoiceLines);
                return new SuccessResult("Fatura satırları silindi");
            }
            else
            {
                return new ErrorResult("Fatura satırları bulunamadı");
            }
        }
    }
}
