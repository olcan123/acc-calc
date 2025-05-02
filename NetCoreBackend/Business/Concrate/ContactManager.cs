using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrate
{
    public class ContactManager : IContactService
    {
        private readonly IContactDal _contactDal;
        private readonly IContactDetailService _contactDetailService;
        private readonly IContactWarehouseService _contactWarehouseService;

        public ContactManager(IContactDal contactDal, IContactDetailService contactDetailService, IContactWarehouseService contactWarehouseService)
        {
            _contactDal = contactDal;
            _contactDetailService = contactDetailService;
            _contactWarehouseService = contactWarehouseService;
        }

        public IDataResult<Contact> GetById(int id)
        {
            var contact = _contactDal.Get(c => c.Id == id);
            return new SuccessDataResult<Contact>(contact);
        }

        public IDataResult<Contact> GetByIdInclude(int id)
        {
            var contact = _contactDal.GetWithIncludeChain(x => x.Include(x => x.ContactDetails), x => x.Id == id);
            return new SuccessDataResult<Contact>(contact);
        }

        public IDataResult<List<Contact>> GetList()
        {
            var contacts = _contactDal.GetAll();
            return new SuccessDataResult<List<Contact>>(contacts);
        }

        public IDataResult<List<Contact>> GetListInclude()
        {
            var contacts = _contactDal.GetAllWithIncludeChain(x => x.Include(x => x.ContactDetails));
            return new SuccessDataResult<List<Contact>>(contacts);
        }

        [ValidationAspect(typeof(ContactValidator), Priority = 1)]
        public IResult Add(Contact contact)
        {
            _contactDal.Add(contact);
            return new SuccessResult("İletişim Bilgisi Eklendi");
        }

        public IResult Delete(Contact contact)
        {
            _contactDal.Delete(contact);
            return new SuccessResult("İletişim Bilgisi Silindi");
        }

        [ValidationAspect(typeof(ContactValidator), Priority = 1)]
        public IResult Update(Contact contact)
        {
            _contactDal.Update(contact);
            return new SuccessResult("İletişim Bilgisi Güncellendi");
        }

        //NOTE: ContactDetail islemleri icin gerekli metot 
        //NOTE: ContactDetail WebAPI tekrar cagirmak istememden bu metot icinde yapildi
        public IResult DeleteContactDetail(ContactDetail contactDetail)
        {
            _contactDetailService.Delete(contactDetail);
            return new SuccessResult("İletişim Bilgisi Silindi");
        }

        public IResult UpdateContactDetail(ContactDetail contactDetail)
        {
            _contactDetailService.Update(contactDetail);
            return new SuccessResult("İletişim Bilgisi Güncellendi");
        }


        //NOTE: Contact , ContactDetail ve ContactWarehouse ekleme islemleri icin gerekli metot
        [TransactionScopeAspect]
        public IResult AddWithContactDetailAndWarehouse(int warehouseId, Contact contact, List<ContactDetail> contactDetails)
        {
            Add(contact);
            contactDetails.ForEach(x => x.ContactId = contact.Id);
            _contactWarehouseService.Add(warehouseId, contact.Id);
            _contactDetailService.AddBulk(contactDetails);
            return new SuccessResult("İletişim Bilgisi Eklendi");
        }

        [TransactionScopeAspect]
        public IResult DeleteWithContactDetailAndWarehouse(int warehouseId, int contactId)
        {
            var contactWithDetails = _contactDal.GetWithIncludeChain(
                x => x.Include(x => x.ContactDetails),
                x => x.Id == contactId
            );

            if (contactWithDetails == null)
                return new ErrorResult("İletişim kaydı bulunamadı.");

            var contactDetails = contactWithDetails.ContactDetails?.ToList() ?? new List<ContactDetail>();

            if (contactDetails.Any())
                _contactDetailService.DeleteBulk(contactDetails);

            _contactWarehouseService.Delete(warehouseId, contactId);
            contactWithDetails.ContactDetails = null;
            _contactDal.Delete(contactWithDetails);

            return new SuccessResult("İletişim bilgileri başarıyla silindi.");
        }

        [TransactionScopeAspect]
        public IResult UpdateWithContactDetailAndWarehouse(Contact contact, List<ContactDetail> contactDetails)
        {
            Update(contact);
            contactDetails.ForEach(x => x.ContactId = contact.Id);
            _contactDetailService.AddOrUpdateBulk(contactDetails);
            return new SuccessResult("İletişim Bilgileri Güncellendi");
        }

    }
}