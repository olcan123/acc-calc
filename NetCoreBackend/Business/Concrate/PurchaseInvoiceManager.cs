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
    public class PurchaseInvoiceManager : IPurchaseInvoiceService
    {
        private readonly IPurchaseInvoiceDal _purchaseInvoiceDal;
        private readonly ILedgerService _ledgerService;
        private readonly ILedgerEntryService _ledgerEntryService;
        private readonly IPurchaseInvoiceExpenseService _purchaseInvoiceExpenseService;
        private readonly IPurchaseInvoiceLineService _purchaseInvoiceLineService;

        public PurchaseInvoiceManager(IPurchaseInvoiceDal purchaseInvoiceDal, ILedgerService ledgerService, ILedgerEntryService ledgerEntryService, IPurchaseInvoiceExpenseService purchaseInvoiceExpenseService, IPurchaseInvoiceLineService purchaseInvoiceLineService)
        {
            _purchaseInvoiceDal = purchaseInvoiceDal;
            _ledgerService = ledgerService;
            _ledgerEntryService = ledgerEntryService;
            _purchaseInvoiceExpenseService = purchaseInvoiceExpenseService;
            _purchaseInvoiceLineService = purchaseInvoiceLineService;
        }

        public IDataResult<List<PurchaseInvoice>> GetList()
        {
            var result = _purchaseInvoiceDal.GetAll();
            return new SuccessDataResult<List<PurchaseInvoice>>(result);
        }
        public IDataResult<List<PurchaseInvoice>> GetListWithIncludes()
        {
            var result = _purchaseInvoiceDal.GetAllWithIncludeChain(
                q => q.Include(p => p.PurchaseInvoiceLines)
                    .Include(p => p.PurchaseInvoiceExpenses)
                    .Include(p => p.Ledger)
                      .ThenInclude(l => l.LedgerEntries)
);
            return new SuccessDataResult<List<PurchaseInvoice>>(result);
        }

        public IDataResult<List<PurchaseInvoice>> GetListByPartnerId(int partnerId)
        {
            var result = _purchaseInvoiceDal.GetAll(x => x.PartnerId == partnerId);
            return new SuccessDataResult<List<PurchaseInvoice>>(result);
        }

        public IDataResult<PurchaseInvoice> GetById(int id)
        {
            var result = _purchaseInvoiceDal.Get(x => x.Id == id);
            return new SuccessDataResult<PurchaseInvoice>(result);
        }
        public IDataResult<PurchaseInvoice> GetByIdWithDetails(int id)
        {
            var result = _purchaseInvoiceDal.GetWithIncludeChain(
                q => q.Include(p => p.PurchaseInvoiceLines)
                    .Include(p => p.PurchaseInvoiceExpenses)
                    .Include(p => p.Ledger)
                      .ThenInclude(l => l.LedgerEntries),
                x => x.Id == id);
            return new SuccessDataResult<PurchaseInvoice>(result);
        }


        [ValidationAspect(typeof(PurchaseInvoiceValidator))]
        public IResult Add(PurchaseInvoice purchaseInvoice)
        {
            _purchaseInvoiceDal.Add(purchaseInvoice);
            return new SuccessResult("Satınalma faturası oluşturuldu");
        }


        [ValidationAspect(typeof(PurchaseInvoiceValidator))]
        public IResult Update(PurchaseInvoice purchaseInvoice)
        {
            _purchaseInvoiceDal.Update(purchaseInvoice);
            return new SuccessResult("Satınalma faturası güncellendi");
        }


        public IResult Delete(PurchaseInvoice purchaseInvoice)
        {
            _purchaseInvoiceDal.Delete(purchaseInvoice);
            return new SuccessResult("Satınalma faturası silindi");
        }

        //SECTION - Purchase Invoice
        [TransactionScopeAspect]
        public IResult AddInvoice(Ledger ledger, PurchaseInvoice purchaseInvoice, List<PurchaseInvoiceLine> purchaseInvoiceLines, List<PurchaseInvoiceExpense> purchaseInvoiceExpenses)
        {

            var ledgerEntries = new List<LedgerEntry>();
            _ledgerService.Add(ledger);
            purchaseInvoice.LedgerId = ledger.Id;
            Add(purchaseInvoice);

            //SECTION - Purchase Expenses and Lines
            if (purchaseInvoiceExpenses != null && purchaseInvoiceExpenses.Count > 0)
            {
                purchaseInvoiceExpenses.ForEach(x =>
                {
                    x.PurchaseInvoiceId = purchaseInvoice.Id;
                });
                _purchaseInvoiceExpenseService.BulkAdd(purchaseInvoiceExpenses);
            }


            purchaseInvoiceLines.ForEach(x =>
            {
                x.PurchaseInvoiceId = purchaseInvoice.Id;
            });
            _purchaseInvoiceLineService.BulkAdd(purchaseInvoiceLines);

            //SECTION - Ledgerations
            var ledgerizations = new LedgerizationPurchaseInvoice();

            ledgerEntries = ledgerizations.CreateAllPurchaseInvoiceLedgerEntries(ledger.Id, purchaseInvoice, purchaseInvoiceLines, purchaseInvoiceExpenses);
            _ledgerEntryService.BulkAdd(ledgerEntries);

            //SECTION - Ledger Entries
            return new SuccessResult("Satınalma faturası oluşturuldu");
        }

        [TransactionScopeAspect]
        public IResult DeleteInvoice(int purchaseId)
        {
            var purchaseInvoice = GetByIdWithDetails(purchaseId).Data;
            if (purchaseInvoice == null)
                return new ErrorResult("Satınalma faturası bulunamadı");

            _purchaseInvoiceExpenseService.BulkDeleteByPurchaseInvoiceId(purchaseInvoice.Id);
            _purchaseInvoiceLineService.BulkDeleteByPurchaseInvoiceId(purchaseInvoice.Id);
            Delete(purchaseInvoice);

            _ledgerEntryService.BulkDeleteByLedgerId(purchaseInvoice.LedgerId);
            _ledgerService.Delete(new Ledger { Id = purchaseInvoice.LedgerId });
            return new SuccessResult("Satınalma faturası silindi");
        }

        [TransactionScopeAspect]
        public IResult UpdateInvoice(Ledger ledger, PurchaseInvoice purchaseInvoice, List<PurchaseInvoiceLine> purchaseInvoiceLines, List<PurchaseInvoiceExpense> purchaseInvoiceExpenses)
        {
            var ledgerEntries = new List<LedgerEntry>();
            _ledgerService.Update(ledger);
            Update(purchaseInvoice);

            //SECTION - Purchase Expenses and Lines
            if (purchaseInvoiceExpenses != null && purchaseInvoiceExpenses.Count > 0)
            {
                purchaseInvoiceExpenses.ForEach(x =>
                {
                    x.PurchaseInvoiceId = purchaseInvoice.Id;
                });
                _purchaseInvoiceExpenseService.BulkUpdate(purchaseInvoiceExpenses);
            }

            purchaseInvoiceLines.ForEach(x =>
            {
                x.PurchaseInvoiceId = purchaseInvoice.Id;
            });
            _purchaseInvoiceLineService.BulkUpdate(purchaseInvoiceLines);

            //SECTION - Ledgerations
            var ledgerizations = new LedgerizationPurchaseInvoice();

            ledgerEntries = ledgerizations.CreateAllPurchaseInvoiceLedgerEntries(ledger.Id, purchaseInvoice, purchaseInvoiceLines, purchaseInvoiceExpenses);
            _ledgerEntryService.BulkUpdate(ledgerEntries);

            return new SuccessResult("Satınalma faturası güncellendi");
        }
    }
}
