using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class WarehouseManager : IWarehouseService
    {
        private readonly IWarehouseDal _warehouseDal;
        private readonly IAddressService _addressService;

        public WarehouseManager(IWarehouseDal warehouseDal, IAddressService addressService)
        {
            _warehouseDal = warehouseDal;
            _addressService = addressService;
        }

        public IDataResult<Warehouse> Get(int id)
        {
            var warehouse = _warehouseDal.GetWithIncludeChain(query => query.Include(x => x.Addresses), x => x.Id == id);
            return new SuccessDataResult<Warehouse>(warehouse);
        }

        public IDataResult<List<Warehouse>> GetAll()
        {
            var warehouses = _warehouseDal.GetAll();
            return new SuccessDataResult<List<Warehouse>>(warehouses);
        }

        public IResult Add(Warehouse warehouse)
        {
            _warehouseDal.AddWithChildren(warehouse);
            return new SuccessResult("Added");
        }

        public IResult Delete(Warehouse warehouse)
        {
            var warehouseResult = _warehouseDal.GetWithIncludeChain(query => query.Include(x => x.Addresses), x => x.Id == warehouse.Id);
            _warehouseDal.Delete(warehouseResult);
            _addressService.BulkDelete(warehouseResult.Addresses.ToList());
            return new SuccessResult("Deleted");
        }
        public IResult Update(Warehouse warehouse)
        {
            _warehouseDal.UpdateWithChildren(warehouse);
            return new SuccessResult("Updated");
        }
    }
}