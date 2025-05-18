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
    public class LedgerEntryManager : ILedgerEntryService
    {
        private readonly ILedgerEntryDal _ledgerEntryDal;

        public LedgerEntryManager(ILedgerEntryDal ledgerEntryDal)
        {
            _ledgerEntryDal = ledgerEntryDal;
        }

        public IDataResult<List<LedgerEntry>> GetList()
        {
            var result = _ledgerEntryDal.GetAll();
            return new SuccessDataResult<List<LedgerEntry>>(result);
        }

        public IDataResult<List<LedgerEntry>> GetListByLedgerId(int ledgerId)
        {
            var result = _ledgerEntryDal.GetAll(x => x.LedgerId == ledgerId);
            return new SuccessDataResult<List<LedgerEntry>>(result);
        }

        public IDataResult<LedgerEntry> GetById(int id)
        {
            var result = _ledgerEntryDal.Get(x => x.Id == id);
            return new SuccessDataResult<LedgerEntry>(result);
        }

        public IResult Add(LedgerEntry ledgerEntry)
        {
            _ledgerEntryDal.Add(ledgerEntry);
            return new SuccessResult("Muhasebe fişi satırı eklendi");
        }

        public IResult Update(LedgerEntry ledgerEntry)
        {
            _ledgerEntryDal.Update(ledgerEntry);
            return new SuccessResult("Muhasebe fişi satırı güncellendi");
        }

        public IResult Delete(LedgerEntry ledgerEntry)
        {
            _ledgerEntryDal.Delete(ledgerEntry);
            return new SuccessResult("Muhasebe fişi satırı silindi");
        }

        //SECTION - Bulk

        public IResult BulkAdd(List<LedgerEntry> ledgerEntries)
        {
            _ledgerEntryDal.BulkAdd(ledgerEntries);
            return new SuccessResult("Muhasebe fişi satırları eklendi");
        }

        public IResult BulkUpdate(List<LedgerEntry> ledgerEntries)
        {
            _ledgerEntryDal.MergeLinq(ledgerEntries, (source, target) =>
                source.Id == target.Id);
            return new SuccessResult("Muhasebe fişi satırları güncellendi");
        }

        public IResult BulkDeleteByLedgerId(int ledgerId)
        {
            var ledgerEntries = _ledgerEntryDal.GetAll(x => x.LedgerId == ledgerId);
            if (ledgerEntries != null && ledgerEntries.Count > 0)
            {
                _ledgerEntryDal.BulkDelete(ledgerEntries);
                return new SuccessResult("Muhasebe fişi satırları silindi");
            }
            else
            {
                return new ErrorResult("Silinecek muhasebe fişi satırı bulunamadı");
            }
        }
    }
}
