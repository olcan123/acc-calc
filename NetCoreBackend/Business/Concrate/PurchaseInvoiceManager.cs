using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
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
                q => q.Include(p => p.Partner)
                    .Include(p => p.Currency)
                    .Include(p => p.PurchaseInvoiceLines)
                    .Include(p => p.PurchaseInvoiceExpenses),
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
        public IResult AddInvoice(Ledger ledger, List<LedgerEntry> ledgerEntries, PurchaseInvoice purchaseInvoice, List<PurchaseInvoiceLine> purchaseInvoiceLines, List<PurchaseInvoiceExpense> purchaseInvoiceExpenses)
        {
            _ledgerService.Add(ledger);
            purchaseInvoice.LedgerId = ledger.Id;
            _purchaseInvoiceDal.Add(purchaseInvoice);

            //SECTION - Purchase Expenses and Lines
            if (purchaseInvoiceExpenses != null)
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

            //SECTION - Ledger Entries
            if (ledgerEntries != null)
            {
                ledgerEntries.ForEach(x =>
                {
                    x.LedgerId = ledger.Id;
                });
                _ledgerEntryService.BulkAdd(ledgerEntries);
            }

            return new SuccessResult("Satınalma faturası oluşturuldu");
        }
    }
}
