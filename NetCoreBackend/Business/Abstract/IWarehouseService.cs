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
        IDataResult<List<Warehouse>> GetList();
        IDataResult<List<Warehouse>> GetListInclude();
        IDataResult<Warehouse> GetById(int id);
        IDataResult<Warehouse> GetByIdInclude(int id);

        IDataResult<Warehouse> GetListContactsByWarehouseId(int warehouseId);

        IResult Add(Warehouse warehouse);
        IResult AddBulk(List<Warehouse> warehouses);
        IResult Delete(Warehouse warehouse);
        IResult Update(Warehouse warehouse);
        IResult UpdateBulk(List<Warehouse> warehouses);

        // Warehouse And Addreses Methods CRUD
        IResult AddWarehouseWithAddresses(Warehouse warehouse, List<Address> addresses);
        IResult DeleteWarehouseWithAddresses(int warehouseId);
        IResult UpdateWarehouseWithAddresses(Warehouse warehouse, List<Address> addresses);
    }
}