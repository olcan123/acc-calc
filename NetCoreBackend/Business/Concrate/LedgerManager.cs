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

        public LedgerManager(ILedgerDal ledgerDal)
        {
            _ledgerDal = ledgerDal;
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
    }
}
