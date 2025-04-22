using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using System.Collections.Generic;

namespace Business.Concrate
{
    public class ContactDetailManager : IContactDetailService
    {
        private readonly IContactDetailDal _contactDetailDal;

        public ContactDetailManager(IContactDetailDal contactDetailDal)
        {
            _contactDetailDal = contactDetailDal;
        }

        public IDataResult<List<ContactDetail>> GetAll()
        {
            var details = _contactDetailDal.GetAll();
            return new SuccessDataResult<List<ContactDetail>>(details);
        }

        public IDataResult<ContactDetail> Get(int id)
        {
            var detail = _contactDetailDal.Get(x => x.Id == id);
            return new SuccessDataResult<ContactDetail>(detail);
        }

        public IResult Add(ContactDetail detail)
        {
            _contactDetailDal.Add(detail);
            return new SuccessResult("Contact detail added.");
        }

        public IResult Update(ContactDetail detail)
        {
            _contactDetailDal.Update(detail);
            return new SuccessResult("Contact detail updated.");
        }

        public IResult Delete(ContactDetail detail)
        {
            _contactDetailDal.Delete(detail);
            return new SuccessResult("Contact detail deleted.");
        }
    }
}
