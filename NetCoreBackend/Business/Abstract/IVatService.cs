using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IVatService
    {

        //SECTION - ASYNC OPERATIONS
        Task<IDataResult<List<Vat>>> GetListAsync();

        //SECTION - SYNC OPERATIONS
        IDataResult<List<Vat>> GetList();
        IDataResult<Vat> GetById(int id);

        IResult Add(Vat vat);
        IResult Update(Vat vat);
        IResult Delete(Vat vat);
    }
}