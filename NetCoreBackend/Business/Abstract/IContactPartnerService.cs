using Core.Utilities.Results;
using Entities.Concrate;

namespace Business.Abstract
{
    public interface IContactPartnerService
    {
        // ContactPartner many-to-many için temel işlemler
        IResult Add(int partnerId, int contactId);
        IResult Delete(int partnerId, int contactId);

        // SECTION - ContactPartner Controller için gerekli metot
        IDataResult<Contact> GetByIdInclude(int contactId);
        IDataResult<Partner> GetListContactsByPartnerId(int partnerId);

        // SECTION - Contact + ContactDetail işlemleri (partner özel)
        IResult AddWithContactDetailAndPartner(int partnerId, Contact contact, List<ContactDetail> contactDetails);
        IResult DeleteWithContactDetailAndPartner(int partnerId, int contactId);
        IResult UpdateWithContactDetailAndPartner(Contact contact, List<ContactDetail> contactDetails);
    }
}
