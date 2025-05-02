using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Results;

namespace Business.Abstract
{
    public interface IContactWarehouseService
    {
        public IResult Add(int warehouseId, int contactId);
        public IResult Delete(int warehouseId, int contactId);
    }
}