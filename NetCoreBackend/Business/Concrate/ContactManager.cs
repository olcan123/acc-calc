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

        public ContactManager(IContactDal contactDal, IContactDetailService contactDetailService)
        {
            _contactDal = contactDal;
            _contactDetailService = contactDetailService;
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
      
    }
}