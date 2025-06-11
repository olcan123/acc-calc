using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class LedgerManager : ILedgerService
    {
        private readonly ILedgerDal _ledgerDal;
        private readonly ILedgerEntryService _ledgerEntryService;

        public LedgerManager(ILedgerDal ledgerDal, ILedgerEntryService ledgerEntryService)
        {
            _ledgerDal = ledgerDal;
            _ledgerEntryService = ledgerEntryService;
        }

        public IDataResult<List<Ledger>> GetList()
        {
            var result = _ledgerDal.GetAll();
            return new SuccessDataResult<List<Ledger>>(result);
        }

        public IDataResult<List<Ledger>> GetListWithEntries()
        {
            var result = _ledgerDal.GetAllWithIncludeChain(
                q => q.Include(l => l.LedgerEntries));
            return new SuccessDataResult<List<Ledger>>(result);
        }

        public IDataResult<Ledger> GetById(int id)
        {
            var result = _ledgerDal.Get(x => x.Id == id);
            return new SuccessDataResult<Ledger>(result);
        }

        public IDataResult<Ledger> GetByIdWithEntries(int id)
        {
            var result = _ledgerDal.GetWithIncludeChain(
                q => q.Include(l => l.LedgerEntries),
                x => x.Id == id);
            return new SuccessDataResult<Ledger>(result);
        }


        public IResult Add(Ledger ledger)
        {

            _ledgerDal.Add(ledger);
            return new SuccessResult("Muhasebe fişi oluşturuldu");
        }

        public IResult Update(Ledger ledger)
        {

            _ledgerDal.Update(ledger);
            return new SuccessResult("Muhasebe fişi güncellendi");
        }

        public IResult Delete(Ledger ledger)
        {

            _ledgerDal.Delete(ledger);
            return new SuccessResult("Muhasebe fişi silindi");
        }


        //SECTION - Ledgerization

        [TransactionScopeAspect]
        public IResult AddLedgerization(Ledger ledger, List<LedgerEntry> ledgerEntries)
        {
            Add(ledger);
            if (ledgerEntries != null && ledgerEntries.Count > 0)
            {
                foreach (var entry in ledgerEntries)
                {
                    entry.LedgerId = ledger.Id; // Set the LedgerId for each entry
                }
                _ledgerEntryService.BulkAdd(ledgerEntries);
            }

            return new SuccessResult("Muhasebe fişi ve girişleri eklendi");
        }

        public IResult UpdateLedgerization(Ledger ledger, List<LedgerEntry> ledgerEntries)
        {
            Update(ledger);
            if (ledgerEntries != null && ledgerEntries.Count > 0)
            {
                foreach (var entry in ledgerEntries)
                {
                    entry.LedgerId = ledger.Id; // Set the LedgerId for each entry
                }
                _ledgerEntryService.BulkUpdate(ledgerEntries);
            }

            return new SuccessResult("Muhasebe fişi ve girişleri güncellendi");
        }

        public IResult DeleteLedgerization(int ledgerId)
        {
            _ledgerEntryService.BulkDeleteByLedgerId(ledgerId);
            _ledgerDal.Delete(new Ledger { Id = ledgerId });
            return new SuccessResult("Muhasebe fişi ve girişleri silindi");
        }
    }
}
