using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface ILedgerService
    {
        IDataResult<List<Ledger>> GetList();
        IDataResult<List<Ledger>> GetListWithEntries();
        IDataResult<Ledger> GetById(int id);
        IDataResult<Ledger> GetByIdWithEntries(int id);
        IResult Add(Ledger ledger);
        IResult Update(Ledger ledger);
        IResult Delete(Ledger ledger);

        // SeCTION - Ledgerization
        IResult AddLedgerization(Ledger ledger, List<LedgerEntry> ledgerEntries);
        IResult UpdateLedgerization(Ledger ledger, List<LedgerEntry> ledgerEntries);
        IResult DeleteLedgerization(int ledgerId);
    }
}
