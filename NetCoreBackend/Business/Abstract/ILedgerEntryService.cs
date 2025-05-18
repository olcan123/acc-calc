using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface ILedgerEntryService
    {
        IDataResult<List<LedgerEntry>> GetList();
        IDataResult<List<LedgerEntry>> GetListByLedgerId(int ledgerId);
        IDataResult<LedgerEntry> GetById(int id);
        IResult Add(LedgerEntry ledgerEntry);
        IResult Update(LedgerEntry ledgerEntry);
        IResult Delete(LedgerEntry ledgerEntry);

        //SECTION - Bulk
        IResult BulkAdd(List<LedgerEntry> ledgerEntries);
        IResult BulkUpdate(List<LedgerEntry> ledgerEntries);
        IResult BulkDeleteByLedgerId(int ledgerId);
    }
}
