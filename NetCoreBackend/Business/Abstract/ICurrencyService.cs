using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface ICurrencyService
    {
        IDataResult<List<Currency>> GetList();
        IDataResult<Currency> GetById(int id);
        IDataResult<Currency> GetByCode(string code);
        IResult Add(Currency currency);
        IResult Update(Currency currency);
        IResult Delete(Currency currency);
    }
}
