
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Business.Concrate
{
    public class PartnerManager : IPartnerService
    {
        private readonly IPartnerDal _partnerDal;
        private readonly IAddressPartnerService _addressPartnerService;

        public PartnerManager(IPartnerDal partnerDal, IAddressPartnerService addressPartnerService)
        {
            _partnerDal = partnerDal;
            _addressPartnerService = addressPartnerService;
        }
        
        //
        // Async Methods
        //
        
        public async Task<IDataResult<List<Partner>>> GetListAsync()
        {
            var result = await _partnerDal.GetAllAsync();
            return new SuccessDataResult<List<Partner>>(result);
        }

        public IDataResult<Partner> GetById(int id)
        {
            var result = _partnerDal.Get(p => p.Id == id);
            return new SuccessDataResult<Partner>(result);
        }

        public IDataResult<Partner> GetByIdInclude(int id)
        {
            var result = _partnerDal.GetWithIncludeChain(x => x.Include(x => x.AddressPartners).ThenInclude(x => x.Address)
            .Include(x => x.ContactPartners).ThenInclude(x => x.Contact)
            .Include(x => x.BankAccountPartners).ThenInclude(x => x.BankAccount), x => x.Id == id);
            return new SuccessDataResult<Partner>(result);
        }

        public IDataResult<List<Partner>> GetList()
        {
            var result = _partnerDal.GetAll();
            return new SuccessDataResult<List<Partner>>(result);
        }

        public IDataResult<List<Partner>> GetListInclude()
        {
            var result = _partnerDal.GetAllWithIncludeChain(x => x.Include(x => x.AddressPartners).ThenInclude(x => x.Address)
            .Include(x => x.ContactPartners).ThenInclude(x => x.Contact)
            .Include(x => x.BankAccountPartners).ThenInclude(x => x.BankAccount));
            return new SuccessDataResult<List<Partner>>(result);
        }

        //SECTION - ContactPartnerController icin gerekli ve kullanildi
        public IDataResult<Partner> GetListContactsByPartnerId(int partnerId)
        {
            var partner = _partnerDal.GetWithIncludeChain(x => x.Include(x => x.ContactPartners)
            .ThenInclude(x => x.Contact).ThenInclude(x => x.ContactDetails), x => x.Id == partnerId);
            return new SuccessDataResult<Partner>(partner);
        }

        [ValidationAspect(typeof(PartnerValidator))]
        public IResult Add(Partner partner)
        {
            _partnerDal.Add(partner);
            return new SuccessResult("Partner eklendi.");
        }

        public IResult Delete(Partner partner)
        {
            _partnerDal.Delete(partner);
            return new SuccessResult("Partner silindi.");
        }

        [ValidationAspect(typeof(PartnerValidator))]
        public IResult Update(Partner partner)
        {
            _partnerDal.Update(partner);
            return new SuccessResult("Partner güncellendi.");
        }

        //SECTION - Address - Partner

        [TransactionScopeAspect]
        public IResult AddWithAddress(Partner partner, Address address)
        {
            Add(partner);
            _addressPartnerService.Add(partner.Id, address);
            return new SuccessResult("Partner ve adresi eklendi.");

        }

        [TransactionScopeAspect]
        public IResult UpdateWithAddress(Partner partner, Address address)
        {
            Update(partner);
            _addressPartnerService.Update(address);
            return new SuccessResult("Partner ve adresi güncellendi.");
        }

        public IResult DeleteWithAddress(int addressId, int partnerId)
        {
            _addressPartnerService.Delete(addressId, partnerId);
            Delete(new Partner { Id = partnerId });
            return new SuccessResult("Partner ve adresi silindi.");
        }

    }
}
