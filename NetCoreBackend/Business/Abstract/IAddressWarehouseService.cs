using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IAddressWarehouseService
    {
        public IDataResult<List<AddressWarehouse>> GetList();
        public IDataResult<List<AddressWarehouse>> GetListByWarehouseId(int warehouseId);
        public IDataResult<List<AddressWarehouse>> GetListByAddressId(int addressId);
        public IDataResult<AddressWarehouse> GetByWarehouseId(int warehouseId);
        public IDataResult<AddressWarehouse> GetByAddressId(int addressId);
        public IResult Add(AddressWarehouse addressWarehouse);
        public IResult AddBulk(List<AddressWarehouse> addressWarehouses);
        public IResult Update(AddressWarehouse addressWarehouse);
        public IResult Delete(AddressWarehouse addressWarehouse);
        public IResult DeleteBulk(List<AddressWarehouse> addressWarehouses);
    }
}