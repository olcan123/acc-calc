using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IUnitOfMeasureService
    {
        IDataResult<List<UnitOfMeasure>> GetList();
        IDataResult<UnitOfMeasure> GetById(int id);

        IResult Add(UnitOfMeasure unitOfMeasure);
        IResult Update(UnitOfMeasure unitOfMeasure);
        IResult Delete(UnitOfMeasure unitOfMeasure);
    }
}
