using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class ContactWarehouseManager : IContactWarehouseService
    {
        private readonly IContactWarehouseDal _contactWarehouseDal;
        private readonly IContactService _contactService;
        private readonly IContactDetailService _contactDetailService;
        private readonly IWarehouseService _warehouseService;

        public ContactWarehouseManager(IContactWarehouseDal contactWarehouseDal, IContactService contactService, IContactDetailService contactDetailService, IWarehouseService warehouseService)
        {
            _contactWarehouseDal = contactWarehouseDal;
            _contactService = contactService;
            _contactDetailService = contactDetailService;
            _warehouseService = warehouseService;
        }

        public IResult Add(int warehouseId, int contactId)
        {
            _contactWarehouseDal.Add(new ContactWarehouse { WarehouseId = warehouseId, ContactId = contactId });
            return new SuccessResult("İletişim Bilgisi Eklendi");
        }

        public IResult Delete(int warehouseId, int contactId)
        {
            _contactWarehouseDal.Delete(new Entities.Concrate.ContactWarehouse { WarehouseId = warehouseId, ContactId = contactId });
            return new SuccessResult("İletişim Bilgisi Silindi");
        }

        //SECTION - CONTACT WAREHOUSE veri getirme islemleri icin gerekli metot Controller icinde kullanilacak

        public IDataResult<Contact> GetByIdInclude(int id)
        {
            var contact = _contactService.GetByIdInclude(id).Data;
            return new SuccessDataResult<Contact>(contact);
        }

        public IDataResult<Warehouse> GetListContactsByWarehouseId(int warehouseId)
        {
            var warehouse = _warehouseService.GetListContactsByWarehouseId(warehouseId).Data;
            return new SuccessDataResult<Warehouse>(warehouse);
        }


        //SECTION  - Contact ve ContactDetail ekleme, silme ve guncelleme islemleri icin gerekli metot
        [TransactionScopeAspect]
        public IResult AddWithContactDetailAndWarehouse(int warehouseId, Contact contact, List<ContactDetail> contactDetails)
        {
            _contactService.Add(contact);
            contactDetails.ForEach(x => x.ContactId = contact.Id);
            Add(warehouseId, contact.Id);
            _contactDetailService.AddBulk(contactDetails);
            return new SuccessResult("İletişim Bilgisi Eklendi");
        }

        [TransactionScopeAspect]
        public IResult DeleteWithContactDetailAndWarehouse(int warehouseId, int contactId)
        {
            var contactWithDetails = _contactService.GetByIdInclude(contactId).Data;

            if (contactWithDetails == null)
                return new ErrorResult("İletişim kaydı bulunamadı.");

            var contactDetails = contactWithDetails.ContactDetails?.ToList() ?? new List<ContactDetail>();

            if (contactDetails.Any())
                _contactDetailService.DeleteBulk(contactDetails);

            Delete(warehouseId, contactId);
            contactWithDetails.ContactDetails = null;
            //NOTE - Bu null ile Contact oldu
            _contactService.Delete(contactWithDetails);

            return new SuccessResult("İletişim bilgileri başarıyla silindi.");
        }

        [TransactionScopeAspect]
        public IResult UpdateWithContactDetailAndWarehouse(Contact contact, List<ContactDetail> contactDetails)
        {
            _contactService.Update(contact);
            contactDetails.ForEach(x => x.ContactId = contact.Id);
            _contactDetailService.AddOrUpdateBulk(contactDetails);
            return new SuccessResult("İletişim Bilgileri Güncellendi");
        }
    }
}