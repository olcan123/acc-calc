using Business.Abstract;
using Business.ValidationRules;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using EFCore.BulkExtensions;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{    public class WarehouseManager : IWarehouseService
    {
        private readonly IWarehouseDal _warehouseDal;
        private readonly IAddressService _addressService;
        private readonly IAddressWarehouseService _addressWarehouseService;

        public WarehouseManager(IWarehouseDal warehouseDal, IAddressService addressService, IAddressWarehouseService addressWarehouseService)
        {
            _warehouseDal = warehouseDal;
            _addressService = addressService;
            _addressWarehouseService = addressWarehouseService;
        }
        
        //
        // Async Methods
        //
        
        public async Task<IDataResult<List<Warehouse>>> GetListAsync()
        {
            var result = await _warehouseDal.GetAllAsync();
            return new SuccessDataResult<List<Warehouse>>(result);
        }

        public IDataResult<Warehouse> GetById(int id)
        {
            var result = _warehouseDal.Get(c => c.Id == id);
            return new SuccessDataResult<Warehouse>(result);
        }

        public IDataResult<Warehouse> GetByIdInclude(int id)
        {
            var result = _warehouseDal.GetWithIncludeChain(x => x.Include(x => x.AddressWarehouses).ThenInclude(x => x.Address), x => x.Id == id);
            return new SuccessDataResult<Warehouse>(result);
        }

        public IDataResult<List<Warehouse>> GetList()
        {
            var result = _warehouseDal.GetAll();
            return new SuccessDataResult<List<Warehouse>>(result);
        }

        public IDataResult<List<Warehouse>> GetListInclude()
        {
            var result = _warehouseDal.GetAllWithIncludeChain(query => query.Include(x => x.AddressWarehouses).ThenInclude(x => x.Address));
            return new SuccessDataResult<List<Warehouse>>(result);
        }


        [ValidationAspect(typeof(WarehouseValidator), Priority = 1)]
        public IResult Add(Warehouse warehouse)
        {
            _warehouseDal.Add(warehouse);
            return new SuccessResult("Depo Eklendi");
        }

        public IResult AddBulk(List<Warehouse> warehouses)
        {
            _warehouseDal.BulkAdd(warehouses, new BulkConfig { SetOutputIdentity = true });
            return new SuccessResult("Depo Eklendi");
        }

        public IResult Delete(Warehouse warehouse)
        {
            _warehouseDal.Delete(warehouse);
            return new SuccessResult("Depo Silindi");
        }

        [ValidationAspect(typeof(WarehouseValidator), Priority = 1)]
        public IResult Update(Warehouse warehouse)
        {
            _warehouseDal.Update(warehouse);
            return new SuccessResult("Depo Güncellendi");
        }

        public IResult UpdateBulk(List<Warehouse> warehouses)
        {
            _warehouseDal.BulkUpdate(warehouses);
            return new SuccessResult("Depo Güncellendi");
        }


        // Warehouse and Address Relations CRUD Operations

        //**Bu aslinda warehouse cagirmasina ragmen Contact bilgileri gostermesi icin kullanildi(Contact icin kullanilacak) ContactWarehouseController icin gerekli
        public IDataResult<Warehouse> GetListContactsByWarehouseId(int warehouseId)
        {
            var warehouse = _warehouseDal.GetWithIncludeChain(x => x.Include(x => x.ContactWarehouses)
            .ThenInclude(x => x.Contact).ThenInclude(x => x.ContactDetails), x => x.Id == warehouseId);
            return new SuccessDataResult<Warehouse>(warehouse);
        }

        //NOTE Warehouse ve Addressleri birlikte ekleme, silme ve güncelleme islemleri icin gerekli metot

        [TransactionScopeAspect]
        public IResult AddWarehouseWithAddresses(Warehouse warehouse, List<Address> addresses)
        {
            Add(warehouse);
            _addressService.AddBulk(addresses);
            _addressWarehouseService.AddBulk(addresses.Select(x => new AddressWarehouse { AddressId = x.Id, WarehouseId = warehouse.Id }).ToList());
            return new SuccessResult("Depo ve Adresleri Eklendi");
        }


        [TransactionScopeAspect]
        public IResult DeleteWarehouseWithAddresses(int WarehouseId)
        {
            var warehouse = _warehouseDal.GetWithIncludeChain(x => x.Include(x => x.AddressWarehouses).ThenInclude(x => x.Address), x => x.Id == WarehouseId);
            _addressWarehouseService.DeleteBulk(warehouse.AddressWarehouses.ToList());
            _addressService.DeleteBulk(warehouse.AddressWarehouses.Select(x => x.Address).ToList());
            warehouse.AddressWarehouses = null;
            _warehouseDal.Delete(warehouse);
            return new SuccessResult("Depo ve Adresleri Silindi");
        }

        [TransactionScopeAspect]
        public IResult UpdateWarehouseWithAddresses(Warehouse warehouse, List<Address> addresses)
        {
            Update(warehouse);
            var addressWarehouses = _addressWarehouseService.GetListByWarehouseId(warehouse.Id).Data;
            _addressWarehouseService.DeleteBulk(addressWarehouses);
            _addressService.UpdateBulk(addresses);
            _addressWarehouseService.AddBulk(addresses.Select(x => new AddressWarehouse { AddressId = x.Id, WarehouseId = warehouse.Id }).ToList());
            return new SuccessResult("Depo ve Adresleri Güncellendi");
        }
    }
}