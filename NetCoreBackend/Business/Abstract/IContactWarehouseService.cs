using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IContactWarehouseService
    {
        public IResult Add(int warehouseId, int contactId);
        public IResult Delete(int warehouseId, int contactId);

        //SECTION - Contact Warehouse Controller icin gerekli metot
        IDataResult<Contact> GetByIdInclude(int id);
        IDataResult<Warehouse> GetListContactsByWarehouseId(int warehouseId);


        //SECTION - Contact ve ContactDetail ekleme, silme ve guncelleme islemleri icin gerekli metot
        IResult AddWithContactDetailAndWarehouse(int warehouseId, Contact contact, List<ContactDetail> contactDetails);
        IResult DeleteWithContactDetailAndWarehouse(int warehouseId, int contactId);
        IResult UpdateWithContactDetailAndWarehouse(Contact contact, List<ContactDetail> contactDetails);
    }
}