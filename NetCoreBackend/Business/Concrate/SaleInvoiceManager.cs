using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Ledgerization.Strategies;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class SaleInvoiceManager : ISaleInvoiceService
    {
        private readonly ISaleInvoiceDal _saleInvoiceDal;
        private readonly ILedgerService _ledgerService;
        private readonly ILedgerEntryService _ledgerEntryService;
        private readonly ISaleInvoiceLineService _saleInvoiceLineService;

        public SaleInvoiceManager(ISaleInvoiceDal saleInvoiceDal, ILedgerService ledgerService, ILedgerEntryService ledgerEntryService, ISaleInvoiceLineService saleInvoiceLineService)
        {
            _saleInvoiceDal = saleInvoiceDal;
            _ledgerService = ledgerService;
            _ledgerEntryService = ledgerEntryService;
            _saleInvoiceLineService = saleInvoiceLineService;
        }

        public IDataResult<List<SaleInvoice>> GetList()
        {
            var result = _saleInvoiceDal.GetAll();
            return new SuccessDataResult<List<SaleInvoice>>(result);
        }
        public IDataResult<List<SaleInvoice>> GetListWithIncludes()
        {
            var result = _saleInvoiceDal.GetAllWithIncludeChain(
                x => x.Include(y => y.SaleInvoiceLines)
                      .Include(y => y.Ledger).ThenInclude(z => z.LedgerEntries)
            );
            return new SuccessDataResult<List<SaleInvoice>>(result);
        }

        public IDataResult<List<SaleInvoice>> GetListByPartnerId(int partnerId)
        {
            var result = _saleInvoiceDal.GetAllWithIncludeChain(
                x => x.Include(y => y.SaleInvoiceLines)
                      .Include(y => y.Ledger).ThenInclude(z => z.LedgerEntries),
                x => x.PartnerId == partnerId
            );
            return new SuccessDataResult<List<SaleInvoice>>(result);
        }

        public IDataResult<SaleInvoice> GetById(int id)
        {
            var result = _saleInvoiceDal.Get(x => x.Id == id);
            return new SuccessDataResult<SaleInvoice>(result);
        }

        public IDataResult<SaleInvoice> GetByIdWithDetails(int id)
        {
            var result = _saleInvoiceDal.GetWithIncludeChain(
                x => x.Include(y => y.SaleInvoiceLines)
                      .Include(y => y.Ledger).ThenInclude(z => z.LedgerEntries),
                x => x.Id == id
            );
            return new SuccessDataResult<SaleInvoice>(result);
        }


        [ValidationAspect(typeof(SaleInvoiceValidator))]
        public IResult Add(SaleInvoice saleInvoice)
        {
            _saleInvoiceDal.Add(saleInvoice);
            return new SuccessResult("Satış faturası eklendi");
        }

        [ValidationAspect(typeof(SaleInvoiceValidator))]
        public IResult Update(SaleInvoice saleInvoice)
        {
            _saleInvoiceDal.Update(saleInvoice);
            return new SuccessResult("Satış faturası güncellendi");
        }

        public IResult Delete(SaleInvoice saleInvoice)
        {
            _saleInvoiceDal.Delete(saleInvoice);
            return new SuccessResult("Satış faturası silindi");
        }

        //SECTION - Sale Invoice
        [TransactionScopeAspect]
        public IResult AddInvoice(Ledger ledger, SaleInvoice saleInvoice, List<SaleInvoiceLine> saleInvoiceLines)
        {
            var ledgerEntries = new List<LedgerEntry>();

            // Ledger ekleme
            var ledgerResult = _ledgerService.Add(ledger);
            if (!ledgerResult.Success)
                return new ErrorResult("Defter kaydı eklenemedi");

            // SaleInvoice'a LedgerId atama
            saleInvoice.LedgerId = ledger.Id;
            var invoiceResult = Add(saleInvoice);
            if (!invoiceResult.Success)
                return new ErrorResult("Satış faturası eklenemedi");

            // SaleInvoiceLines ekleme
            if (saleInvoiceLines != null && saleInvoiceLines.Count > 0)
            {
                saleInvoiceLines.ForEach(x =>
                {
                    x.SaleInvoiceId = saleInvoice.Id;
                });
                var linesResult = _saleInvoiceLineService.BulkAdd(saleInvoiceLines);
                if (!linesResult.Success)
                    return new ErrorResult("Satış faturası satırları eklenemedi");
            }

            // Ledgerizations - Satış faturası için defter kayıtları oluşturma
            var ledgerizations = new LedgerizationSaleInvoice();
            ledgerEntries = ledgerizations.CreateAllSaleInvoiceLedgerEntries(ledger.Id, saleInvoice, saleInvoiceLines);

            var entriesResult = _ledgerEntryService.BulkAdd(ledgerEntries);
            if (!entriesResult.Success)
                return new ErrorResult("Defter kayıtları eklenemedi");

            return new SuccessResult("Satış faturası başarıyla eklendi");
        }

        [TransactionScopeAspect]
        public IResult DeleteInvoice(int saleInvoiceId)
        {
            var saleInvoice = _saleInvoiceDal.Get(x => x.Id == saleInvoiceId);
            if (saleInvoice == null)
                return new ErrorResult("Satış faturası bulunamadı");

            // SaleInvoiceLines silme
            _saleInvoiceLineService.BulkDeleteBySaleInvoiceId(saleInvoiceId);

            // SaleInvoice silme
            Delete(saleInvoice);

            // Ledger kayıtları silme
            _ledgerEntryService.BulkDeleteByLedgerId(saleInvoice.LedgerId);
            _ledgerService.Delete(new Ledger { Id = saleInvoice.LedgerId });

            return new SuccessResult("Satış faturası silindi");
        }

        [TransactionScopeAspect]
        public IResult UpdateInvoice(Ledger ledger, SaleInvoice saleInvoice, List<SaleInvoiceLine> saleInvoiceLines)
        {
            var ledgerEntries = new List<LedgerEntry>();

            // Ledger güncelleme
            _ledgerService.Update(ledger);

            // SaleInvoice güncelleme
            Update(saleInvoice);

            // SaleInvoiceLines güncelleme
            if (saleInvoiceLines != null && saleInvoiceLines.Count > 0)
            {
                saleInvoiceLines.ForEach(x =>
                {
                    x.SaleInvoiceId = saleInvoice.Id;
                });
                _saleInvoiceLineService.BulkUpdate(saleInvoiceLines);
            }

            // Ledgerizations - Satış faturası için defter kayıtları güncelleme
            var ledgerizations = new LedgerizationSaleInvoice();
            ledgerEntries = ledgerizations.CreateAllSaleInvoiceLedgerEntries(ledger.Id, saleInvoice, saleInvoiceLines);
            _ledgerEntryService.BulkUpdate(ledgerEntries);

            return new SuccessResult("Satış faturası güncellendi");
        }
    }
}
