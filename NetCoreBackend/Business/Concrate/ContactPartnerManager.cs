using Business.Abstract;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class ContactPartnerManager : IContactPartnerService
    {
        private readonly IContactPartnerDal _contactPartnerDal;
        private readonly IContactService _contactService;
        private readonly IContactDetailService _contactDetailService;
        private readonly IPartnerService _partnerService;

        public ContactPartnerManager(
            IContactPartnerDal contactPartnerDal,
            IContactService contactService,
            IContactDetailService contactDetailService,
            IPartnerService partnerService)
        {
            _contactPartnerDal = contactPartnerDal;
            _contactService = contactService;
            _contactDetailService = contactDetailService;
            _partnerService = partnerService;
        }

        public IResult Add(int partnerId, int contactId)
        {
            _contactPartnerDal.Add(new ContactPartner { PartnerId = partnerId, ContactId = contactId });
            return new SuccessResult("İletişim bilgisi eklendi.");
        }

        public IResult Delete(int partnerId, int contactId)
        {
            _contactPartnerDal.Delete(new ContactPartner { PartnerId = partnerId, ContactId = contactId });
            return new SuccessResult("İletişim bilgisi silindi.");
        }

        public IDataResult<Contact> GetByIdInclude(int id)
        {
            var contact = _contactService.GetByIdInclude(id).Data;
            return new SuccessDataResult<Contact>(contact);
        }

        public IDataResult<Partner> GetListContactsByPartnerId(int partnerId)
        {
            var partner = _partnerService.GetListContactsByPartnerId(partnerId).Data;
            return new SuccessDataResult<Partner>(partner);
        }

        [TransactionScopeAspect]
        public IResult AddWithContactDetailAndPartner(int partnerId, Contact contact, List<ContactDetail> contactDetails)
        {
            _contactService.Add(contact);
            contactDetails.ForEach(x => x.ContactId = contact.Id);
            Add(partnerId, contact.Id);
            _contactDetailService.AddBulk(contactDetails);
            return new SuccessResult("İletişim bilgisi eklendi.");
        }

        [TransactionScopeAspect]
        public IResult DeleteWithContactDetailAndPartner(int partnerId, int contactId)
        {
            var contactWithDetails = _contactService.GetByIdInclude(contactId).Data;

            if (contactWithDetails == null)
                return new ErrorResult("İletişim kaydı bulunamadı.");

            var contactDetails = contactWithDetails.ContactDetails?.ToList() ?? new List<ContactDetail>();

            if (contactDetails.Any())
                _contactDetailService.DeleteBulk(contactDetails);

            Delete(partnerId, contactId);
            contactWithDetails.ContactDetails = null;
            _contactService.Delete(contactWithDetails);

            return new SuccessResult("İletişim bilgileri başarıyla silindi.");
        }

        [TransactionScopeAspect]
        public IResult UpdateWithContactDetailAndPartner(Contact contact, List<ContactDetail> contactDetails)
        {
            _contactService.Update(contact);
            contactDetails.ForEach(x => x.ContactId = contact.Id);
            _contactDetailService.AddOrUpdateBulk(contactDetails);
            return new SuccessResult("İletişim bilgileri güncellendi.");
        }
    }
}