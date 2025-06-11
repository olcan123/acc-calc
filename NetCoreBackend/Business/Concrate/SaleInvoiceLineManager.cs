using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using LinqToDB.Tools;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class SaleInvoiceLineManager : ISaleInvoiceLineService
    {
        private readonly ISaleInvoiceLineDal _saleInvoiceLineDal;

        public SaleInvoiceLineManager(ISaleInvoiceLineDal saleInvoiceLineDal)
        {
            _saleInvoiceLineDal = saleInvoiceLineDal;
        }        public IDataResult<List<SaleInvoiceLine>> GetList()
        {
            var result = _saleInvoiceLineDal.GetAll();
            return new SuccessDataResult<List<SaleInvoiceLine>>(result);
        }



        public IDataResult<SaleInvoiceLine> GetById(int id)
        {
            var result = _saleInvoiceLineDal.Get(x => x.Id == id);
            return new SuccessDataResult<SaleInvoiceLine>(result);
        }

        public IResult Add(SaleInvoiceLine saleInvoiceLine)
        {
            _saleInvoiceLineDal.Add(saleInvoiceLine);
            return new SuccessResult("Satış faturası satırı eklendi");
        }

        public IResult Update(SaleInvoiceLine saleInvoiceLine)
        {
            _saleInvoiceLineDal.Update(saleInvoiceLine);
            return new SuccessResult("Satış faturası satırı güncellendi");
        }

        public IResult Delete(SaleInvoiceLine saleInvoiceLine)
        {
            _saleInvoiceLineDal.Delete(saleInvoiceLine);
            return new SuccessResult("Satış faturası satırı silindi");
        }

        //SECTION - Bulk

        public IResult BulkAdd(List<SaleInvoiceLine> saleInvoiceLines)
        {
            _saleInvoiceLineDal.BulkAdd(saleInvoiceLines);
            return new SuccessResult("Satış faturası satırları eklendi");
        }

        public IResult BulkUpdate(List<SaleInvoiceLine> saleInvoiceLines)
        {
            var saleInvoiceId = saleInvoiceLines.FirstOrDefault()?.SaleInvoiceId;
            if (saleInvoiceId == null)
                return new ErrorResult("Fatura ID'si bulunamadı");
                
            _saleInvoiceLineDal.MergeSync(saleInvoiceLines,
                (source, target) => source.Id == target.Id,
                target => target.SaleInvoiceId == saleInvoiceId.Value);
            return new SuccessResult("Satış faturası satırları güncellendi");
        }

        public IResult BulkDeleteBySaleInvoiceId(int saleInvoiceId)
        {
            var saleInvoiceLines = _saleInvoiceLineDal.GetAll(x => x.SaleInvoiceId == saleInvoiceId);
            if (saleInvoiceLines != null && saleInvoiceLines.Count > 0)
            {
                _saleInvoiceLineDal.BulkDelete(saleInvoiceLines);
                return new SuccessResult("Satış faturası satırları silindi");
            }
            else
            {
                return new ErrorResult("Satış faturası satırları bulunamadı");
            }
        }
    }
}
