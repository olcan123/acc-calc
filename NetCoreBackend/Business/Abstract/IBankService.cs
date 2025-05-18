using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IBankService
    {
        Task<IDataResult<List<Bank>>> GetListAsync();
        IDataResult<List<Bank>> GetList();
        IDataResult<Bank> GetById(int id);
        IResult Add(Bank bank);
        IResult Update(Bank bank);
        IResult Delete(Bank bank);
    }
}