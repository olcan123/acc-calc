using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class AddressWarehouseManager : IAddressWarehouseService
    {
        private readonly IAddressWarehouseDal _addressWarehouseDal;

        public AddressWarehouseManager(IAddressWarehouseDal addressWarehouseDal)
        {
            _addressWarehouseDal = addressWarehouseDal;
        }

        public IDataResult<List<AddressWarehouse>> GetList()
        {
            var addressWarehouses = _addressWarehouseDal.GetAll();
            return new SuccessDataResult<List<AddressWarehouse>>(addressWarehouses);
        }

        public IDataResult<List<AddressWarehouse>> GetListByWarehouseId(int warehouseId)
        {
            var addressWarehouses = _addressWarehouseDal.GetAll(c => c.WarehouseId == warehouseId);
            return new SuccessDataResult<List<AddressWarehouse>>(addressWarehouses);
        }

        public IDataResult<List<AddressWarehouse>> GetListByAddressId(int addressId)
        {
            var addressWarehouses = _addressWarehouseDal.GetAll(c => c.AddressId == addressId);
            return new SuccessDataResult<List<AddressWarehouse>>(addressWarehouses);
        }

        public IDataResult<AddressWarehouse> GetByWarehouseId(int warehouseId)
        {
            var addressWarehouse = _addressWarehouseDal.Get(c => c.WarehouseId == warehouseId);
            return new SuccessDataResult<AddressWarehouse>(addressWarehouse);
        }

        public IDataResult<AddressWarehouse> GetByAddressId(int addressId)
        {
            var addressWarehouse = _addressWarehouseDal.Get(c => c.AddressId == addressId);
            return new SuccessDataResult<AddressWarehouse>(addressWarehouse);
        }

        public IResult Add(AddressWarehouse addressWarehouse)
        {
            _addressWarehouseDal.Add(addressWarehouse);
            return new SuccessResult("Adres Eklendi");
        }

        public IResult AddBulk(List<AddressWarehouse> addressWarehouses)
        {
            _addressWarehouseDal.BulkAdd(addressWarehouses);
            return new SuccessResult("Adres Eklendi");
        }

        public IResult Delete(AddressWarehouse addressWarehouse)
        {
            _addressWarehouseDal.Delete(addressWarehouse);
            return new SuccessResult("Adres Silindi");
        }

        public IResult DeleteBulk(List<AddressWarehouse> addressWarehouses)
        {
            _addressWarehouseDal.BulkDelete(addressWarehouses);
            return new SuccessResult("Adres Silindi");
        }


        public IResult Update(AddressWarehouse addressWarehouse)
        {
            _addressWarehouseDal.Update(addressWarehouse);
            return new SuccessResult("Adres Güncellendi");
        }
    }
}