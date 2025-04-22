using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class AddressManager : IAddressService
    {
        private readonly IAddressDal _addressDal;

        public AddressManager(IAddressDal addressDal)
        {
            _addressDal = addressDal;
        }

        public IDataResult<List<Address>> GetAll()
        {
            var addresses = _addressDal.GetAll();
            return new SuccessDataResult<List<Address>>(addresses);
        }

        public IDataResult<Address> Get(int id)
        {
            var address = _addressDal.Get(x => x.Id == id);
            return new SuccessDataResult<Address>(address);
        }

        public IResult Add(Address address)
        {
            _addressDal.Add(address);
            return new SuccessResult("Address added");
        }

        public IResult Update(Address address)
        {
            _addressDal.Update(address);
            return new SuccessResult("Address updated");
        }

        public IResult Delete(Address address)
        {
            _addressDal.Delete(address);
            return new SuccessResult("Address deleted");
        }

        public IResult BulkDelete(List<Address> addresses)
        {
            _addressDal.BulkDelete(addresses);
            return new SuccessResult("Addresses added");
        }
    }

}