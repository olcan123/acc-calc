using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IWarehouseService
    {
        IDataResult<List<Warehouse>> GetAll();
        IDataResult<Warehouse> Get(int id);
        IResult Add(Warehouse warehouse);
        IResult Delete(Warehouse warehouse);
        IResult Update(Warehouse warehouse);
    }
}