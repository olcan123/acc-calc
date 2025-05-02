using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IContactService
    {
        IDataResult<List<Contact>> GetList();
        IDataResult<List<Contact>> GetListInclude();
        IDataResult<Contact> GetById(int id);
        IDataResult<Contact> GetByIdInclude(int id);

        IResult Add(Contact contact);
        IResult Update(Contact contact);
        IResult Delete(Contact contact);

        //TODO: ContactDetail Delete islemleri icin gerekli metot
        IResult DeleteContactDetail(ContactDetail contactDetail);
        IResult UpdateContactDetail(ContactDetail contactDetail);

        // Crud With ContactDetail And Warehouse

        // IResult AddWithContactDetail(Contact contact, List<ContactDetail> contactDetails);
        // IResult DeleteWithContactDetail(int id);
        // IResult UpdateWithContactDetail(Contact contact, List<ContactDetail> contactDetails);

        IResult AddWithContactDetailAndWarehouse(int warehouseId, Contact contact, List<ContactDetail> contactDetails);
        IResult DeleteWithContactDetailAndWarehouse(int warehouseId, int contactId);
        IResult UpdateWithContactDetailAndWarehouse(Contact contact, List<ContactDetail> contactDetails);


    }
}