using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using EFCore.BulkExtensions;
using Entities.Concrate;

namespace Business.Concrate
{
    public class AddressManager : IAddressService
    {
        private IAddressDal _addressDal;

        public AddressManager(IAddressDal addressDal)
        {
            _addressDal = addressDal;
        }

        public IDataResult<Address> GetById(int id)
        {
            var address = _addressDal.Get(c => c.Id == id);
            return new SuccessDataResult<Address>(address);
        }

        public IDataResult<List<Address>> GetList()
        {
            var addresses = _addressDal.GetAll();
            return new SuccessDataResult<List<Address>>(addresses);
        }

        [ValidationAspect(typeof(AddressValidator), Priority = 1)]
        public IResult Add(Address address)
        {
            _addressDal.Add(address);
            return new SuccessResult("Adres Eklendi");
        }

        [ValidationAspect(typeof(AddressValidator), Priority = 1)]
        public IResult AddBulk(List<Address> addresses)
        {
            _addressDal.BulkAdd(addresses, new BulkConfig { SetOutputIdentity = true });
            return new SuccessResult("Adres Eklendi");
        }

        public IResult Delete(Address address)
        {
            _addressDal.Delete(address);
            return new SuccessResult("Adres Silindi");
        }

        public IResult DeleteBulk(List<Address> addresses)
        {
            _addressDal.BulkDelete(addresses);
            return new SuccessResult("Adres Silindi");
        }

        [ValidationAspect(typeof(AddressValidator), Priority = 1)]
        public IResult Update(Address address)
        {
            _addressDal.Update(address);
            return new SuccessResult("Adres Güncellendi");
        }

        [ValidationAspect(typeof(AddressValidator), Priority = 1)]
        public IResult UpdateBulk(List<Address> addresses)
        {
            _addressDal.BulkUpdate(addresses);
            return new SuccessResult("Adres Güncellendi");
        }
    }
}