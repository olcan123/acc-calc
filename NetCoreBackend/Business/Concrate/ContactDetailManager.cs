using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class ContactDetailManager : IContactDetailService
    {
        private readonly IContactDetailDal _contactDetailDal;

        public ContactDetailManager(IContactDetailDal contactDetailDal)
        {
            _contactDetailDal = contactDetailDal;
        }

        public IDataResult<ContactDetail> GetById(int id)
        {
            var contactDetail = _contactDetailDal.Get(c => c.Id == id);
            return new SuccessDataResult<ContactDetail>(contactDetail);
        }

        public IDataResult<List<ContactDetail>> GetList()
        {
            var contactDetails = _contactDetailDal.GetAll();
            return new SuccessDataResult<List<ContactDetail>>(contactDetails);
        }

        public IDataResult<List<ContactDetail>> GetListByContactId(int contactId)
        {
            var contactDetails = _contactDetailDal.GetAll(c => c.ContactId == contactId);
            return new SuccessDataResult<List<ContactDetail>>(contactDetails);
        }

        [ValidationAspect(typeof(ContactDetailValidator), Priority = 1)]
        public IResult Add(ContactDetail contactDetail)
        {
            _contactDetailDal.Add(contactDetail);
            return new SuccessResult("İletişim Bilgisi Eklendi");
        }

        [ValidationAspect(typeof(ContactDetailValidator), Priority = 1)]
        public IResult AddBulk(List<ContactDetail> contactDetails)
        {
            _contactDetailDal.BulkAdd(contactDetails);
            return new SuccessResult("İletişim Bilgileri Eklendi");
        }

        public IResult Delete(ContactDetail contactDetail)
        {
            _contactDetailDal.Delete(contactDetail);
            return new SuccessResult("İletişim Bilgisi Silindi");
        }

        public IResult DeleteBulk(List<ContactDetail> contactDetails)
        {
            _contactDetailDal.BulkDelete(contactDetails);
            return new SuccessResult("İletişim Bilgileri Silindi");
        }

        [ValidationAspect(typeof(ContactDetailValidator), Priority = 1)]
        public IResult Update(ContactDetail contactDetail)
        {
            _contactDetailDal.Update(contactDetail);
            return new SuccessResult("İletişim Bilgisi Güncellendi");
        }

        [ValidationAspect(typeof(ContactDetailValidator), Priority = 1)]
        public IResult UpdateBulk(List<ContactDetail> contactDetails)
        {
            _contactDetailDal.BulkUpdate(contactDetails);
            return new SuccessResult("İletişim Bilgileri Güncellendi");
        }

        [ValidationAspect(typeof(ContactDetailValidator), Priority = 1)]
        public IResult AddOrUpdateBulk(List<ContactDetail> contactDetails)
        {
            _contactDetailDal.BulkAddOrUpdate(contactDetails);
            return new SuccessResult("İletişim Bilgileri Güncellendi");
        }
    }
}