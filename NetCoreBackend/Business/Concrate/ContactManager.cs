using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using System.Collections.Generic;

namespace Business.Concrate
{
    public class ContactManager : IContactService
    {
        private readonly IContactDal _contactDal;

        public ContactManager(IContactDal contactDal)
        {
            _contactDal = contactDal;
        }

        public IDataResult<List<Contact>> GetAll()
        {
            var contacts = _contactDal.GetAll();
            return new SuccessDataResult<List<Contact>>(contacts);
        }

        public IDataResult<Contact> Get(int id)
        {
            var contact = _contactDal.Get(x => x.Id == id);
            return new SuccessDataResult<Contact>(contact);
        }

        public IResult Add(Contact contact)
        {
            _contactDal.AddWithChildren(contact);
            return new SuccessResult("Contact added.");
        }

        public IResult Update(Contact contact)
        {
            _contactDal.Update(contact);
            return new SuccessResult("Contact updated.");
        }

        public IResult Delete(Contact contact)
        {
            _contactDal.Delete(contact);
            return new SuccessResult("Contact deleted.");
        }
    }
}
