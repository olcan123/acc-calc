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
    public class AddressPartnerManager : IAddressPartnerService
    {
        private readonly IAddressPartnerDal _addressPartnerDal;
        private readonly IAddressService _addressService;

        public AddressPartnerManager(IAddressPartnerDal addressPartnerDal, IAddressService addressService)
        {
            _addressPartnerDal = addressPartnerDal;
            _addressService = addressService;
        }

        public IResult Add(int partnerId, Address address)
        {
            _addressService.Add(address);
            _addressPartnerDal.Add(new AddressPartner { AddressId = address.Id, PartnerId = partnerId });
            return new SuccessResult("Adres Eklendi");
        }

        public IResult Delete(int addressId, int partnerId)
        {
            _addressPartnerDal.Delete(new AddressPartner { AddressId = addressId, PartnerId = partnerId });

            var address = _addressService.GetById(addressId).Data;
            if (address == null)
            {
                return new ErrorResult("Adres bulunamadı.");
            }

            _addressService.Delete(address); // artık tracked entity ile siliniyor
            return new SuccessResult("Adres Silindi");
        }


        public IResult Update(Address address)
        {
            _addressService.Update(address);
            return new SuccessResult("Adres Güncellendi");
        }
    }
}